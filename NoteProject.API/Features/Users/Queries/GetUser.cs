using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using NoteProject.API.Contracts.Course;
using NoteProject.API.Contracts.User;
using NoteProject.API.Database;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Users.Queries;

public static class GetUser
{
    public class Query : IRequest<Result<IEnumerable<GetUserResponse>>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<IEnumerable<GetUserResponse>>>
    {
        private readonly AppDbContext _context;

        public Handler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<GetUserResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var allUsers = await _context.Users
                .Select(u => new GetUserResponse
                {
                    Id = u.Id,
                    Email = u.Email,
                    Password = u.Password,
                    Username = u.Username,
                    RegistrationDate = DateTime.UtcNow
                }).ToListAsync();
            
            if(allUsers == null || !allUsers.Any())
                return Result.Failure<IEnumerable<GetUserResponse>>(new Error("GetCourse.Null", "There are no course"));

            return allUsers;
        }
    }
}

public class GetUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users", async (ISender sender) =>
        {
            var query = new GetUser.Query();
            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        });
    }
}
