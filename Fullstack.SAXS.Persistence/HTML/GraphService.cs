﻿using System.Text;
using System.Text.Json;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Persistence.Contracts;

namespace Fullstack.SAXS.Persistence.HTML
{
    public class GraphService(IStringService @string) : IGraphService
    {
        public async Task<string> GetHtmlPage(float[] x, float[] y)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(@string.GetGraphUriPath())
            };
            using var jsonContent = new StringContent(
                JsonSerializer.Serialize(new { x, y }),
                Encoding.UTF8,
                "application/json"
            );
            using var response = await client.PostAsync("/plot", jsonContent);

            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            return html;
        }
    }
}
