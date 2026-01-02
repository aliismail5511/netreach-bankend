namespace NetReach.Api.Services
{
    public class ProductService
    {
        private readonly string _basePath = AppContext.BaseDirectory;

        public List<string> GetRandomItems(int type, int quantity)
        {
            string fileName = type switch
            {
                0 => "twitterr.txt",
                1 => "instagram.txt",
                2 => "proxyy.txt",
                3 => "codes.txt",
                _ => throw new ArgumentException("Invalid product type")
            };

            string filePath = Path.Combine(_basePath, "Products", fileName);
            if (!File.Exists(filePath))
            {
                return new List<string> { "Product file not found. Please contact support." };
            }

            var allLines = File.ReadAllLines(filePath).Where(l => !string.IsNullOrWhiteSpace(l)).ToList();
            if (allLines.Count == 0)
            {
                return new List<string> { "Out of stock. Please contact support." };
            }

            var random = new Random();
            return allLines.OrderBy(x => random.Next()).Take(quantity).ToList();
        }

        public string GetProductName(int type)
        {
            return type switch
            {
                0 => "Twitter Account",
                1 => "Instagram Account",
                2 => "Proxy",
                3 => "Tool/Code",
                _ => "Product"
            };
        }
    }
}
