using System;
using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CsvParser.Test.IntegrationTests
{
    public class CsvParserEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CsvParserEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task CsvParser_endpoint_returns_ok_test()
        {
            var client = _factory.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/parse-csv");
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes("test1"))), "formFile", "testfile.csv" }
            };

            request.Content = content;

            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CsvParser_endpoint_returns_badRequest_test()
        {
            var client = _factory.CreateClient();
            using var request = new HttpRequestMessage(HttpMethod.Post, "api/parse-csv");
            using var content = new MultipartFormDataContent
            {
                { new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes("test1"))), "formFile", "testfile.txt" }
            };

            request.Content = content;

            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}

