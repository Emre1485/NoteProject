using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using NoteProject.API.Contracts.Course;
using NoteProject.API.Database;
using NoteProject.API.Entities;
using NoteProject.API.Shared;

namespace NoteProject.API.Features.Courses.Commands
{
    public static class CreateCourse
    {
        public class Command : IRequest<Result<Guid>>
        {
            public string CourseName { get; set; } = string.Empty;
            public int CourseGrade { get; set; }
            public int CourseSemester { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.CourseName).NotEmpty();
                RuleFor(c => c.CourseGrade).NotEmpty().InclusiveBetween(1,4);
                RuleFor(c => c.CourseSemester).NotEmpty().InclusiveBetween(1, 2);
            }
        }

        internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
        {
            private readonly AppDbContext _context;
            private readonly IValidator<Command> _validator;
            public Handler(AppDbContext context, IValidator<Command> validator)
            {
                _context = context;
                _validator = validator;
            }

            public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
            {
                var validationResult = _validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<Guid>(new Error(
                        "CreateCourse.Validation",
                        validationResult.ToString()));
                }

                var course = new Course
                {
                    Id = Guid.NewGuid(),
                    CourseName = request.CourseName,
                    CourseGrade = request.CourseGrade,
                    CourseSemester = request.CourseSemester
                };

                _context.Add(course);

                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success(course.Id);
            }
        }

        
    }

    public class CreateCourseEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/courses", async (CreateCourseRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateCourse.Command>();

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
