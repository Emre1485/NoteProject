using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoteProject.API.Contracts.Course;
using NoteProject.API.Database;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Courses.Queries;

public static class GetCourse
{
    public class Query : IRequest<Result<IEnumerable<GetCourseResponse>>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<IEnumerable<GetCourseResponse>>>
    {
        private readonly AppDbContext _context;

        public Handler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<GetCourseResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var allCourses = await _context.Courses
                .Select(course => new GetCourseResponse
                {
                    Id = course.Id,
                    CourseName = course.CourseName,
                    CourseGrade = course.CourseGrade,
                    CourseSemester = course.CourseSemester
                })
                .ToListAsync();
            if (allCourses == null || !allCourses.Any())
                return Result.Failure<IEnumerable<GetCourseResponse>>(new Error("GetCourse.Null", "There are no course"));

            return allCourses;
        }
    }
}

public class GetCourseEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/courses", async (ISender sender) =>
        {
            var query = new GetCourse.Query();

            var result = await sender.Send(query);

            if (result.IsFailure)
            {
                return Results.NotFound(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}
