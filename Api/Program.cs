using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/products", () =>
{
    var data = ProductRepository.GetAll();

    return data;
});

app.MapGet("/products/{code}", ([FromRoute] string code) =>
{
    var result = ProductRepository.GetBy(code);

    return result;
});

app.MapPost("/products", (Product product) =>
{
    var result = ProductRepository.Add(product);

    return result;
});

app.MapPut("/products", (Product product) =>
{
    var result = ProductRepository.Update(product);

    return result;
});

app.MapDelete("/products/{code}", ([FromRoute] string code) =>
{
    var result = ProductRepository.Remove(code);

    return result;
});

app.Run();

public class Product
{
    public string Code { get; set; }
    public string Name { get; set; }
}

public static class ProductRepository
{
    public static List<Product> Products { get; set; }

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

        if (data != null)
            data.Name = product.Name;

        return data;
    }

    public static bool Remove(string code)
    {
        var data = Products.FirstOrDefault(x => x.Code == code);

        if (data != null)
            Products.Remove(data);

        return data != null;
    }
}
