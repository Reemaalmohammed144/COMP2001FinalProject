using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Reena.MSSQL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ReemaContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("COMP2001_RAlmugharriq"));
    });


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint($"./v1/swagger.json", "SapWeb v1");
    c.RoutePrefix = "swagger";
});

var options = new RewriteOptions()
    .AddRewrite("^default.htm$", "redirect.html", skipRemainingRules: true);
app.UseRewriter(options);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
