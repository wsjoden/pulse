using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pulse.Api.Data;
using Pulse.Api.Model;

namespace Pulse.Api.Endpoints
{
    public record RegisterRequest(string Username, string Password);

    public record LoginRequest(string Username, string Password);

    public static class AuthEndpoints
    {
        public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
        {
            var hasher = new PasswordHasher<User>();

            group.MapPost(
                "/register",
                async (AppDbContext context, RegisterRequest req) =>
                {
                    var exists = await context.Users.AnyAsync(u => u.Username == req.Username);
                    if (exists)
                        return Results.Conflict("Username already taken");

                    var user = new User { Username = req.Username };
                    user.PasswordHash = hasher.HashPassword(user, req.Password);

                    context.Users.Add(user);
                    await context.SaveChangesAsync();

                    return Results.Created(
                        $"/api/auth/users/{user.Id}",
                        new { user.Id, user.Username }
                    );
                }
            );

            group.MapPost(
                "/login",
                async (AppDbContext context, LoginRequest req) =>
                {
                    var user = await context.Users.FirstOrDefaultAsync(u =>
                        u.Username == req.Username
                    );
                    if (user is null)
                        return Results.Unauthorized();

                    var result = hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
                    if (result == PasswordVerificationResult.Failed)
                        return Results.Unauthorized();

                    // JWT-generering kommer härnäst
                    return Results.Ok(new { user.Id, user.Username });
                }
            );

            return group;
        }
    }
}
