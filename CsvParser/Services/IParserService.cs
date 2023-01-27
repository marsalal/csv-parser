namespace CsvParser.Services
{
    public interface IParserService
    {
        Task<string> ParseCsvAsync(IFormFile formFile);
    }
}