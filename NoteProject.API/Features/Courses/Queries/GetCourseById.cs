using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoteProject.API.Contracts.Course;
using NoteProject.API.Database;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Courses.Queries
{
    public static class GetCourseById
    {
        public class Query : IRequest<Result<CourseResponseById>>
        {
            public Guid Id { get; set; }
        }

        internal sealed class Handler : IRequestHandler<Query, Result<CourseResponseById>>
        {
            private readonly AppDbContext _context;

            public Handler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<Result<CourseResponseById>> Handle(Query request, CancellationToken cancellationToken)
            {
                var courseByIdResponse = await _context.Courses
                    .Where(course => course.Id == request.Id)
                    .Select(course => new CourseResponseById
                    {
                        Id = course.Id,
                        CourseName = course.CourseName,
                        CourseGrade = course.CourseGrade,
                        CourseSemester = course.CourseSemester
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (courseByIdResponse == null)
                {
                    return Result.Failure<CourseResponseById>(new Error("GetCourseById.Null",
                        "The course with the specified id was not found"));
                }

                return courseByIdResponse;

            }
        }
    }

    public class GetCourseByIdEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("api/courses/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetCourseById.Query { Id = id };

                var result = await sender.Send(query);

                if (result.IsFailure)
                {
                    return Results.NotFound(result.Error);
                }

                return Results.Ok(result.Value);
            });
        }
    }
}
