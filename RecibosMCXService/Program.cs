using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RecibosMCXService.Models;

var AllowAllOrigins = "Allow_All";
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("RecibosMCXDb") ?? "Data Source=RecibosMCXDb.db";
builder.Services.AddSqlite<RecibosMCXDb>(connectionString);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAllOrigins,
                      policy =>
                      {
                          // Do not use in production mode.
                          policy.AllowAnyOrigin();
                      });
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Recibos MCX Express",
        Description = "Plataforma de validação de comprovativos de transferências bancárias emitidas pelo aplicativo MULTICAIXA EXPRESS.",
        Version = "v1.0"
    });
});
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors(AllowAllOrigins);
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();