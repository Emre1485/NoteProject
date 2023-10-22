using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using NoteProject.API.Contracts.User;
using NoteProject.API.Database;
using NoteProject.API.Entities;
using NoteProject.API.Shared;
using System.ComponentModel.DataAnnotations;
using static NoteProject.API.Features.Users.Commands.CreateUser;

namespace NoteProject.API.Features.Users.Commands;

public static class CreateUser
{
    public class Command : IRequest<Result<Guid>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator() 
        {
            RuleFor(u => u.Username).NotEmpty().MinimumLength(3).MaximumLength(20);
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6).MaximumLength(12);
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly AppDbContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(IValidator<Command> validator, AppDbContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
                return Result.Failure<Guid>(new Error("CreateUser.Validation", validationResult.ToString()));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                RegistrationDate = DateTime.UtcNow

            };

            _context.Add(user);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(user.Id);
        }
    }
}

public class CreateUserEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUser.Command>();
            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        });
    }
}