using System.Text;
using System.Text.Json;
using Fullstack.SAXS.Domain.Contracts;

namespace Fullstack.SAXS.Persistence.HTML
{
    public class GraphService(IStringService @string) : IGraphService
    {
        public async Task<string> GetHtmlPageAsync(double[] x, double[] y, string xLable, string yLable, string title)
        {
            var client = new HttpClient()
            {
                BaseAddress = new Uri(@string.GetGraphUriPath())
            };
            using var jsonContent = new StringContent(
                JsonSerializer.Serialize(
                    new { 
                        x = x, 
                        y = y, 
                        x_label = xLable,
                        y_label = yLable,
                        title = title
                    }
                ),
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
