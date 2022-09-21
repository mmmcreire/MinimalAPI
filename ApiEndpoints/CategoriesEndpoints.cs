using Microsoft.EntityFrameworkCore;
using MinimalAPI.Context;
using MinimalAPI.Models;

namespace MinimalAPI.ApiEndpoints;

public static class CategoriesEndpoints
{
    public static void MapCategoriesEndpoints(this WebApplication app)
    {
        app.MapGet("/categories", async (AppDBContext db) => await db.Categories.ToListAsync())
            .WithTags("Categories");

        app.MapGet("/categories/id_{id:int}", async (int id, AppDBContext db) =>
        {
            return await db.Categories.FindAsync(id)
            is Category category
                ? Results.Ok(category)
                : Results.NotFound();
        }).WithTags("Categories");

        app.MapPost("/categories", async (Category category, AppDBContext db) =>
        {
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return Results.Created($"/categories/{category.CategoryId}", category);
        }).WithTags("Categories");

        app.MapPut("/categories/{id:int}", async (int id, Category category, AppDBContext db) =>
        {
            if(category.CategoryId != id)
                return Results.BadRequest();

            var categoryDB = await db.Categories.FindAsync(id);

            if(categoryDB is null)
                return Results.NotFound();

            categoryDB.Name = category.Name;
            categoryDB.Description = category.Description;

            await db.SaveChangesAsync();
            return Results.Ok(categoryDB);
        }).WithTags("Categories");

        app.MapDelete("/categories/delete/id_{id:int}", async (int id, AppDBContext db) =>
        {
            var category = await db.Categories.FindAsync(id);

            if(category is null)
                return Results.NotFound();

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Categories");

        app.MapGet("/categoryproducts", async (AppDBContext db) => await db.Categories.Include(c => c.Products).ToListAsync())
            .Produces<List<Category>>(StatusCodes.Status200OK)
            .WithTags("Categories");
    }
}
