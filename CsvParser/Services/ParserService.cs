using System;
using System.Text;

namespace CsvParser.Services
{
    public class ParserService : IParserService
    {
        private const char Delimiter = ',';

        public async Task<string> ParseCsvAsync(IFormFile formFile)
        {
            if (formFile is null)
            {
                throw new ArgumentNullException(nameof(formFile));
            }

            string fileContent = "";
            const string openBracket = "[";
            const string closeBracket = "]";

            using var streamReader = new StreamReader(formFile.OpenReadStream());
            fileContent = await streamReader.ReadToEndAsync();

            if (fileContent.Length == 0)
            {
                return string.Empty;
            }

            var formattedString = new StringBuilder(fileContent.Length);

            var rows = fileContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < rows.Length; i++)
            {
                var valuesInRow = rows[i].Split(Delimiter);

                for (int j = 0; j < valuesInRow.Length; j++)
                {
                    formattedString.Append(openBracket + rows[i].Trim() + closeBracket);
                }

                if (i < rows.Length - 1)
                {
                    formattedString.Append('\n');
                }
            }

            return formattedString.ToString();
        }
    }
}

