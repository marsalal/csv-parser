using System;
using System.Text;

namespace CsvParser.Services
{
    public class ParserService : IParserService
    {
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

            var rows = fileContent.Split(new[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < rows.Length; i++)
            {
                var valuesInRow = rows[i].Trim('"')
                    .Split(new string[] { "\",\"" }, StringSplitOptions.TrimEntries);

                //remove any space or last character for last value in row.
                int last = valuesInRow.Length - 1;
                valuesInRow[last] = valuesInRow[last].Substring(0, valuesInRow[last].Length - 1);

                for (int j = 0; j < valuesInRow.Length; j++)
                {
                    // check if there is a trailing " in the string.
                    if (valuesInRow[j].IndexOf('\"') > 0)
                    {
                        // go over the possible scenario when a value might have a comma inside and cant be splitted using the delimetter.
                        var valuesWithComma = valuesInRow[j].Split(new[] { '\"' }, StringSplitOptions.TrimEntries);
                        for (int k = 0; k < valuesWithComma.Length; k++)
                        {
                            string? value = valuesWithComma[k];
                            formattedString.Append($"{openBracket}{value.Trim(',')}{closeBracket}");

                            if (k < valuesWithComma.Length - 1)
                            {
                                formattedString.Append(' ');
                            }
                        }
                    }
                    else
                    {
                        formattedString.Append($"{openBracket}{valuesInRow[j].Trim('"')}{closeBracket}");
                    }

                    if (j < valuesInRow.Length - 1)
                    {
                        formattedString.Append(' ');
                    }
                }

                //Append new line only to the last item of the row
                if (i < rows.Length - 1)
                {
                    formattedString.AppendLine();
                }
            }

            return formattedString.ToString();
        }
    }
}

