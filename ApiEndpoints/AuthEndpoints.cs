using Microsoft.AspNetCore.Authorization;
using MinimalAPI.Models;
using MinimalAPI.Services;

namespace MinimalAPI.ApiEndpoints;

public static class AuthEndpoints
{
    public static void MapAutenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/login", [AllowAnonymous] (UserModel userModel, ITokenService tokenService) =>
        {
            if(userModel == null)
            {
                return Results.BadRequest("Invalid login");
            }
            if(userModel.UserName == "mmm" && userModel.Password == "123456")
            {
                var tokenString = tokenService.GetToken(app.Configuration["Jwt:Key"],
                    app.Configuration["Jwt:Issuer"],
                    app.Configuration["Jwt:Audience"],
                    userModel);
                return Results.Ok(new { token = tokenString });
            }
            else
            {
                return Results.BadRequest("Invalid Login");
            }
        }).Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status200OK)
        .WithName("Login")
        .WithTags("Authentication");
    }
}
