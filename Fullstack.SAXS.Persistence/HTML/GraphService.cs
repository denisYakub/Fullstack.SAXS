using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Fullstack.SAXS.Domain.Contracts;
using Fullstack.SAXS.Persistence.Contracts;

namespace Fullstack.SAXS.Persistence.HTML
{
    public class GraphService(IStringService @string) : IGraphService
    {
        public async Task<string> GetHtmlPage(float[] x, float[] y)
        {
            var request = new { x, y };
            var json = JsonSerializer.Serialize(request);

            using var client = new HttpClient();

            var content = new StringContent(
                json, 
                Encoding.UTF8, 
                "application/json"
            );

            var response = await client.PostAsync(@string.GetGraphUriPath(), content);

            var html = await response.Content.ReadAsStringAsync();

            return html;
        }
    }
}
