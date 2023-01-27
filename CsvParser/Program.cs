using CsvParser.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IParserService, ParserService>();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/parse-csv", async ([FromServices] IParserService parser, [FromForm] IFormFile formFile) =>
{
    if (Path.GetExtension(formFile.FileName) != ".csv")
    {
        return Results.BadRequest("Only csv files are supported");
    }

    var parsedCsv = await parser.ParseCsvAsync(formFile);

    return Results.Ok(parsedCsv);
})
.Accepts<IFormFile>("multipart/form-data")
.Produces<string>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.WithName("ParseCsv")
.WithDescription("Endpoint to parse csv files that will return the each column surrounded by a square brackets")
.WithOpenApi();

app.Run();

public partial class Program { }

