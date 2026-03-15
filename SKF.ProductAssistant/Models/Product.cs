namespace SKF.ProductAssistant.Models
{
    public class Product
    {
        public string designation { get; set; }

        public List<Dimension> dimensions { get; set; }
    }
}

namespace SKF.ProductAssistant.Models
{
    public class Dimension
    {
        public string name { get; set; }

        public double value { get; set; }

        public string unit { get; set; }

        public string symbol { get; set; }
    }
}