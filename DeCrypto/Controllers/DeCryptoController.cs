using Microsoft.AspNetCore.Mvc;
namespace DeCrypto.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeCryptoController : ControllerBase
    {
        private readonly ILogger<DeCryptoController> _logger;
        C5_02_SignatureInfo signatureInfo = new C5_02_SignatureInfo();
        private static string filePath;
        private static int MaxFileSize = 5000000;
        public DeCryptoController(ILogger<DeCryptoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public PDFSignatureInfo Get(string url)
        {
            try
            {
                signatureInfo.InspectSignatures(url);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error verifying signatures");
                return new PDFSignatureInfo { Error = e.Message };
            }

            return signatureInfo.PDFSignatureInfo;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPdf(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Por favor adicione um arquivo PDF com formato válido");
                }

                if (Path.GetExtension(file.FileName).ToLower() != ".pdf")
                {
                    return BadRequest("Formato inválido, por favor adicione um arquivo PDF");
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
                    return BadRequest("Por favor adicionar um documento com a capacidade permitida");
                }

                signatureInfo.InspectSignatures(filePath);

                if (System.IO.File.Exists(filePath))
                {
                    //System.IO.File.Delete(filePath);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error verifying signatures");
                return BadRequest(e.Message);
            }
            return Ok(new
            {
                //message = "File uploaded successfully",
                //filePath,
                signatureInfo.PDFSignatureInfo
            });
        }

    }
}
