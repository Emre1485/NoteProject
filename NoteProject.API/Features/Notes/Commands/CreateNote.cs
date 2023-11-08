using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NoteProject.API.Contracts.Note;
using NoteProject.API.Database;
using NoteProject.API.Entities;
using NoteProject.API.Shared;
using System;
using System.Reflection;

namespace NoteProject.API.Features.Notes.Commands;

public static class CreateNote
{
    public class Command : IRequest<Result<Guid>>
    {
        public string FileName { get; set; }

        public byte[] FileData { get; set; }
        //public IFormFile FileData { get; set; }
        public FileType FileType { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(n => n.FileName).NotEmpty();
            RuleFor(n => n.FileData).NotNull();
            RuleFor(n => n.FileType).IsInEnum();
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
                    "CreateNote.Validation",
                    validationResult.ToString()));
            }

            if (request.FileData != null && request.FileData.Length > 0)
            {
                var fileName = request.FileName;
                var fileType = request.FileType;
                //using (var stream = new MemoryStream(request.FileData))
                //{

                    var fileEntity = new FileDetails
                    {
                        Id = Guid.NewGuid(),
                        FileName = fileName,
                        FileType = fileType,
                        FileData = request.FileData
                    };

                    _context.Notes.Add(fileEntity);
                    await _context.SaveChangesAsync(cancellationToken);

                    return Result.Success(fileEntity.Id);
                //}
                
            }

            else
            {
                return Result.Failure<Guid>(new Error("CreateNote.Validation", "Dosya eksik veya boş."));
            }


        }
    }
}

public class CreateNoteEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/notes", async (CreateNoteRequest request, ISender sender) =>
        {

            var command = request.Adapt<CreateNote.Command>();

            var result = await sender.Send(command);

            if (!result.IsSuccess)
            {
                return Results.BadRequest(result.Error);
            }

            return Results.Ok(result.Value);
        });
    }
}