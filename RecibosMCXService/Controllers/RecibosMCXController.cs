using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecibosMCXService.Models;

namespace RecibosMCXService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecibosMCXController : ControllerBase
    {
        private readonly ILogger<RecibosMCXController> _logger;
        private readonly RecibosMCXDb _context;
        SignatureInfo signatureInfo = new SignatureInfo();
        private static string filePath;
        private static int MaxFileSize = 2000000;
        public RecibosMCXController(ILogger<RecibosMCXController> logger, RecibosMCXDb context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public Signer Get(string url)
        {
            try
            {
                signatureInfo.InspectSignatures(url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao verificar o documento");
                return new Signer { Output = e.Message };
            }

            return signatureInfo.Signer;
        }

        [HttpGet]
        [Route("requests")]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _context.Requests.ToListAsync();
            return Ok(requests);
        }

        [HttpPost]
        //[RequestSizeLimit(2 * 1024 * 1024)] // 2 MB limit
        [Route("validate")]
        public async Task<IActionResult> UploadPdf(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Por favor adicione um documento em formato PDF válido");
                }

                if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {
                    return BadRequest("Formato inválido, por favor adicione um documento em formato PDF");
                }

                if (file.Length > 0 && file.Length < MaxFileSize)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = Convert.ToString(Guid.NewGuid()) + "." + file.FileName.Split('.').Last();

                    filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                else
                {
                    return BadRequest("Por favor adicione um documento com a capacidade permitida");
                }

                //Inspect signatures
                signatureInfo.InspectSignatures(filePath);
                //Log request to database
                Requests request = new Requests
                {
                    //IpAddress = ,
                    BrowserAgent = Request.Headers["User-Agent"].ToString(),
                    Date = DateTime.Now,
                };

                await _context.Requests.AddAsync(request);
                await _context.SaveChangesAsync();

                if (System.IO.File.Exists(filePath))
                {
                    //Testing files do not remove yet
                    //System.IO.File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro ao verificar o documento");
                return BadRequest(e.Message);
            }
            return Ok(new
            {
                //message = "File uploaded successfully",
                //filePath,
                signatureInfo.Signer
            });
        }

    }
}
