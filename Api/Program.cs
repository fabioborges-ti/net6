using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();
var configuration = app.Configuration;

//ProductRepository.Init(configuration);

app.MapGet("/products", (ApplicationDbContext context) =>
{
    var products = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Tags)
                    .ToList();

    return Results.Ok(products);
});

app.MapGet("/products/{code}", ([FromRoute] string code, ApplicationDbContext context) =>
{
    var product = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Tags)
                    .FirstOrDefault(p => p.Code.Equals(code));

    if (product != null)
        return Results.Ok(product);

    return Results.NotFound("Product not found"); ;
});

app.MapPost("/products", (ProductRequest request, ApplicationDbContext context) =>
{
    var tags = new List<Tag>();

    if (request.Tags != null)
        request.Tags.ForEach(tag =>
            tags.Add(new Tag { Name = tag })
        );

    var category = context.Categories.Where(c => c.Id.Equals(request.CategoryId)).FirstOrDefault();

    var product = new Product
    {
        Code = request.Code,
        Name = request.Name,
        Description = request.Description,
        Category = category,
        Tags = tags
    };

    context.Products.Add(product);
    context.SaveChanges();

    return Results.Created($"/products/{product.Code}", product);
});

app.MapPut("/products/{id}", ([FromRoute] int id, ProductRequest request, ApplicationDbContext context) =>
{
    var product = context.Products
                    .Include(p => p.Category)
                    .Include(p => p.Tags)
                    .FirstOrDefault(p => p.Id.Equals(id));

    if (product == null)
        return Results.NotFound("Product not found.");

    var category = context.Categories
                    .FirstOrDefault(c => c.Id.Equals(request.CategoryId));

    var tags = new List<Tag>();

    if (request.Tags != null)
        request.Tags.ForEach(tag =>
            tags.Add(new Tag { ProductId = product.Id, Name = tag })
        );

    product.Code = request.Code;
    product.Name = request.Name;
    product.Description = request.Description;
    product.Category = category;
    product.Tags = tags;

    context.Products.Update(product);
    context.SaveChanges();

    return Results.Ok(product);
});

app.MapDelete("/products/{code}", ([FromRoute] string code, ApplicationDbContext context) =>
{
    var product = context.Products.FirstOrDefault(p => p.Code.Equals(code));

    if (product == null)
        return Results.NotFound("Product not found.");

    context.Products.Remove(product);
    context.SaveChanges();

    return Results.Ok("Product removed successfully.");
});

if (app.Environment.IsStaging())
    app.MapGet("/configuration/database", (IConfiguration configuration) =>
    {
        return Results.Ok(configuration["database:connection"]);
    });

app.Run();
