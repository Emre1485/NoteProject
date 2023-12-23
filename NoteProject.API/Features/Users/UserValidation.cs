using Carter;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NoteProject.API.Entities;
using NoteProject.API.Shared;
using static NoteProject.API.Features.Users.UserValidation;


namespace NoteProject.API.Features.Users;

public static class UserValidation
{
    public class UserValidationRequest : IRequest<Result>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UserValidationHandler : IRequestHandler<UserValidationRequest, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserValidationHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result> Handle(UserValidationRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                // Kullanıcı doğrulama başarılı ise giriş yapabilirsiniz.
                await _signInManager.SignInAsync(user, isPersistent: false);

                return Result.Success();
            }

            return Result.Failure(new Error("InvalidCredentials", "Kullanıcı adı veya şifre hatalı.")); // Hata nesnesini oluştururken Message özelliğini kullan
        }
    }
}

// UsersModule.cs
public class UsersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/validate", async (UserValidationRequest request, ISender sender) =>
        {
            var result = await sender.Send(request);

            if (result.IsSuccess)
            {
                return Results.Ok();
            }

            return Results.BadRequest(result.Error);
        });
    }
}