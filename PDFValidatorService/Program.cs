using Microsoft.OpenApi.Models;
var AllowAllOrigins = "Allow_All";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAllOrigins,
                      policy =>
                      {
                          // CORS - Allow requests from any origin
                          // It's just a sample service, nothing serious here
                          policy.AllowAnyOrigin();
                      });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PDF Validator Service", Description = "A .NET Core WebAPI for digital signatures validation within PDF documents using iText Library.", Version = "v1.0" });
});
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors(AllowAllOrigins);
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();