using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoteProject.API.Contracts;
using NoteProject.API.Database;
using NoteProject.API.Features.Courses.Queries;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Courses.Commands
{
    public static class DeleteCourse
    {
        public class Command : IRequest<Result<Guid>>
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var courseToDelete = await _context.Courses
                    .SingleOrDefaultAsync(course => course.Id == request.Id);
                if (courseToDelete != null)
                {
                    _context.Courses.Remove(courseToDelete);
                    await _context.SaveChangesAsync();
                    return Result.Success(request.Id);
                }
                else
                    return Result.Failure<Guid>(new Error("GetCourseById.Null",
                        "The course with the specified id was not found"));

            }
        }

    }

    public class DeleteCourseEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/courses/{id}", async (Guid id, ISender sender) =>
            {
                var command = new DeleteCourse.Command { Id = id };
                var result = await sender.Send(command);

                if (!result.IsSuccess)
                {
                    return Results.BadRequest(result.Error);
                }

                return Results.Ok(result.Value);
            });
        }
    }
}
