public static class ProductRepository
{
    public static List<Product> Products { get; set; } = new List<Product>();

    public static void Init(IConfiguration configuration)
    {
        var products = configuration.GetSection("Products").Get<List<Product>>();

        Products = products;
    }

    public static List<Product> GetAll()
    {
        return Products;
    }

    public static Product GetBy(string code)
    {
        return Products.FirstOrDefault(x => x.Code == code);
    }

    public static Product Add(Product product)
    {
        Products ??= new List<Product>();

        Products.Add(product);

        return product;
    }

    public static Product Update(Product product)
    {
        var data = Products.FirstOrDefault(x => x.Code == product.Code);

        if (data == null)
            return null;

        data.Name = product.Name;

        return data;
    }

    public static bool Remove(string code)
    {
        var data = Products.FirstOrDefault(x => x.Code == code);

        if (data == null)
            return false;

        Products.Remove(data);

        return true;
    }
}
