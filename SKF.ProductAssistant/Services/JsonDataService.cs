using Newtonsoft.Json;
using SKF.ProductAssistant.Models;

namespace SKF.ProductAssistant.Services
{
    public class JsonDataService
    {
        private readonly List<Product> _products;

        public JsonDataService()
        {
            var basePath = AppContext.BaseDirectory;

            var file1 = File.ReadAllText(Path.Combine(basePath, "Data", "6205N.json"));
            var file2 = File.ReadAllText(Path.Combine(basePath, "Data", "6205.json"));

            var product1 = JsonConvert.DeserializeObject<Product>(file1);
            var product2 = JsonConvert.DeserializeObject<Product>(file2);

            _products = new List<Product> { product1, product2 };
        }

        public string GetAttribute(string designation, string attribute)
        {
            var product = _products
                .FirstOrDefault(p =>
                    p.designation.Replace(" ", "").ToLower() ==
                    designation.Replace(" ", "").ToLower());

            if (product == null)
                return null;

            var dimension = product.dimensions
                .FirstOrDefault(d =>
                    d.name.ToLower().Contains(attribute.ToLower()));

            if (dimension == null)
                return null;

            return $"{dimension.value} {dimension.unit}";
        }
    }
}