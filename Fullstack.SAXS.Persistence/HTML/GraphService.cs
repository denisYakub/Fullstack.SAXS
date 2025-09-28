using System.Text;
using System.Text.Json;
using Fullstack.SAXS.Domain.Abstractions.Services;
using Fullstack.SAXS.Persistence.Options;
using Microsoft.Extensions.Options;

namespace Fullstack.SAXS.Persistence.HTML
{
    internal class GraphService(IOptions<GraphOptions> options) : IGraphService
    {
        public async Task<string> GetHtmlPageAsync(double[] x, double[] y, string xLable, string yLable, string title)
        {
            using var client = new HttpClient()
            {
                BaseAddress = options.Value.Uri
            };
            using var jsonContent = new StringContent(
                JsonSerializer.Serialize(
                    new { 
                        x, 
                        y, 
                        x_label = xLable,
                        y_label = yLable,
                        title
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );
            using var response = await client
                .PostAsync(new Uri("/plot", UriKind.Relative), jsonContent)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var html = await response.Content
                .ReadAsStringAsync()
                .ConfigureAwait(false);

            return html;
        }
    }
}
