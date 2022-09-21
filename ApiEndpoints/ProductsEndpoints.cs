using Microsoft.EntityFrameworkCore;
using MinimalAPI.Context;
using MinimalAPI.Models;

namespace MinimalAPI.ApiEndpoints;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this WebApplication app)
    {
        app.MapGet("/products", async (AppDBContext db) => await db.Products.ToListAsync())
            .Produces<List<Product>>(StatusCodes.Status200OK)
            .WithTags("Products")
            .RequireAuthorization();

        app.MapGet("/products/id_{id:int}", async (int id, AppDBContext db) =>
        {
            return await db.Products.FindAsync(id)
            is Product product
                ? Results.Ok(product)
                : Results.NotFound("Product Not Found");
        })
            .Produces<List<Product>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Products");

        app.MapPost("/products", async (Product product, AppDBContext db) =>
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Results.Created($"/products/{product.ProductId}", product);
        })
            .Produces<Product>(StatusCodes.Status201Created)
            .WithName("CreateNewProduct")
            .WithTags("Products");

        app.MapPut("/products/update/id_{id:int}", async (
            int id, Product product, string pName, string pDescription, string pImgUrl,
            decimal pPrice, int pCategoryId, int pInventory, AppDBContext db) =>
        {
            if(product.ProductId != id)
                return Results.BadRequest();

            var productDB = db.Products.SingleOrDefault(s => s.ProductId == id);

            if(productDB is null)
                return Results.NotFound("Product Not Found");

            productDB.Name = pName;
            productDB.Description = pDescription;
            productDB.Price = pPrice;
            productDB.Inventory = pInventory;
            productDB.CategoryId = pCategoryId;
            productDB.ImageUrl = pImgUrl;

            await db.SaveChangesAsync();
            return Results.Ok(productDB);
        })
            .Produces<Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Update Product")
            .WithTags("Products");

        app.MapPut("/products/updatepname/id_{id:int}", async (int id, Product product, AppDBContext db) =>
        {
            if(product.ProductId != id)
                return Results.BadRequest();

            var productDB = db.Products.SingleOrDefault(s => s.ProductId == id);

            if(productDB is null)
                return Results.NotFound("Product Not Found");

            productDB.Name = product.Name;

            await db.SaveChangesAsync();
            return Results.Ok(productDB);
        })
            .Produces<Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Update Product Name")
            .WithTags("Products");

        app.MapDelete("/products/delete/id_{id:int}", async (int id, AppDBContext db) =>
        {
            var productDB = await db.Products.FindAsync(id);

            if(productDB is null)
                return Results.NotFound("Product Not Found");

            db.Products.Remove(productDB);
            await db.SaveChangesAsync();

            return Results.Ok(productDB);
        })
            .Produces<Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Delete Product")
            .WithTags("Products");

        app.MapGet("/products/name/{nameFilter}", (string nameFilter, AppDBContext db) =>
        {
            var selectedProducts = db.Products.Where(x => x.Name.ToLower().Contains(nameFilter)).ToList();
            return selectedProducts.Count > 0
                ? Results.Ok(selectedProducts)
                : Results.NotFound("Not found any product containig this string");
        })
            .Produces<List<Product>>(StatusCodes.Status200OK)
            .WithName("Name filter")
            .WithTags("Products");
    }
}
