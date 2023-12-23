//using Carter;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using NoteProject.API.Contracts.Course;
//using NoteProject.API.Contracts.User;
//using NoteProject.API.Database;
//using NoteProject.API.Shared;

//namespace NoteProject.API.Features.Users.Queries;

//public static class GetUserById
//{
//    public class Query : IRequest<Result<GetUserByIdResponse>>
//    {
//        public Guid Id { get; set; }
//    }

//    internal sealed class Handler : IRequestHandler<Query, Result<GetUserByIdResponse>>
//    {
//        private readonly AppDbContext _context;

//        public Handler(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<Result<GetUserByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
//        {
//            var userByIdResponse = await _context.Users
//                .Where(user => user.Id == request.Id)
//                .Select(user => new GetUserByIdResponse
//                {
//                    Id = user.Id,
//                    Password = user.Password,
//                    Username = user.Username,
//                    Email = user.Email,
//                    RegistrationDate = user.RegistrationDate
//                }).FirstOrDefaultAsync(cancellationToken);

//            if(userByIdResponse == null)
//                return Result.Failure<GetUserByIdResponse>(new Error("GetUserById.Null",
//                        "The user with the specified id was not found"));

//            return userByIdResponse;
//        }
//    }
//}

//public class GetUserByIdEndpoint : ICarterModule
//{
//    public void AddRoutes(IEndpointRouteBuilder app)
//    {
//        app.MapGet("api/users/{id}", async (Guid id, ISender sender) =>
//        {
//            var query = new GetUserById.Query { Id = id };
//            var result = await sender.Send(query);

//            if (result.IsFailure)
//            {
//                return Results.NotFound(result.Error);
//            }

//            return Results.Ok(result.Value);
//        });
//    }
//}