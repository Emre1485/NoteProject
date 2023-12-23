using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NoteProject.API.Contracts.Course;
using NoteProject.API.Contracts.User;
using NoteProject.API.Database;
using NoteProject.API.Entities;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Users.Commands;

public static class CreateUser
{
    public class Command : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordConfirm { get; set; } = string.Empty;

    }
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(u => u.UserName).NotEmpty();
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.PasswordConfirm).NotEmpty();
            RuleFor(u => u.Password.Equals(u.PasswordConfirm));
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly AppDbContext _context;
        private readonly IValidator<Command> _validator;
        private readonly UserManager<AppUser> _userManager;

        public Handler(AppDbContext context, IValidator<Command> validator, UserManager<AppUser> userManager)
        {
            _context = context;
            _validator = validator;
            _userManager = userManager;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {

            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure<Guid>(new Error(
                       "CreateUser.Validation",
                       validationResult.ToString()));
            }

            var user = new AppUser
            {
                Id = request.Id.ToString(),
                UserName = request.UserName,
                Email = request.Email
            };

            if(_context.Users.Any(u => u.Id == user.Id))
            {
                return Result.Failure<Guid>(Error.ConditionNotMet);
            }

            var identityResult = await _userManager.CreateAsync(user, request.Password);

            if (identityResult.Succeeded)
            {
                return Result.Success<Guid>(Guid.Parse(user.Id));
            }
            else
            {
                return Result.Failure<Guid>(Error.ConditionNotMet);
            }
        }
    }
}

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUser.Command>();

            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}