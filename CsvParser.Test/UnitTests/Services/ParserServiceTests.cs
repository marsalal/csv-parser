using System;
using System.Text;
using System.Linq;
using Bogus;
using CsvParser.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Reflection;

namespace CsvParser.Test.Services
{
    public class ParserServiceTests
    {
        private readonly Faker _faker;
        private readonly IParserService _parserService;

        public ParserServiceTests()
        {
            _parserService = new ParserService();
            _faker = new Faker();
        }

        [Fact]
        public async Task ParserService_throws_when_file_is_null_test()
        {
            IFormFile? formFile = null;
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => _parserService.ParseCsvAsync(formFile));
            Assert.Equal("Value cannot be null. (Parameter 'formFile')", ex.Message);
        }

        [Fact]
        public async Task ParserService_returns_string_empty_when_file_does_not_have_content_test()
        {
            var fakeFileName = _faker.System.FileName();
            var fakeFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(string.Empty)), 0, 0, fakeFileName, $"{fakeFileName}.csv");
                        
            var result = await _parserService.ParseCsvAsync(fakeFile);
                        
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task ParserService_returns_formatted_string_test()
        {
            var expectedParsedContent =
                @"[Patient Name] [SSN] [Age] [Phone Number] [Status]
[Prescott, Zeke] [542-51-6641] [21] [801-555-2134] [Opratory=2,PCP=1]
[Goldstein, Bucky] [635-45-1254] [42] [435-555-1541] [Opratory=1,PCP=1]
[Vox, Bono] [414-45-1475] [51] [801-555-2100] [Opratory=3,PCP=2]";

            var fakeFileName = Path.GetFileNameWithoutExtension(_faker.System.FileName());
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(directory, "resources/testfile.csv");
            using var fileStream = File.OpenRead(path);
            var fileMock = new FormFile(fileStream, 0, fileStream.Length, fakeFileName, $"{fakeFileName}.csv");

            var result = await _parserService.ParseCsvAsync(fileMock);

            Assert.Equal(expectedParsedContent, result);
        }
    }
}

