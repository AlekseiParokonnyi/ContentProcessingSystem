using FileProcessingApplication;
using FileProcessingApplication.Models;
using FileProcessingCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FileProcessingApi;

internal static class FilesEndpoints
{
    internal static RouteGroupBuilder MapFilesApi(this RouteGroupBuilder group)
    {
        group.MapPost("", UploadFile)
            .DisableAntiforgery()
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/", GetAllFiles)
            .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id}", GetFileById)
            .WithName("GetFile")
            .Produces(StatusCodes.Status200OK, contentType: "application/octet-stream")
            .Produces(StatusCodes.Status404NotFound);

        return group;
    }

    private static async Task<Results<Ok<FileUploadResult>, BadRequest<string>>> UploadFile(
        IFormFile file,
        IFileProcessingService fileProcessingService,
        CancellationToken ct)
    {
        if (file is not { Length: not 0 })
            return TypedResults.BadRequest("File is empty");

        await using var stream = file.OpenReadStream();
        var result = await fileProcessingService.StoreFileAsync(file.FileName, stream, ct);
        return TypedResults.Ok(result);
    }

    private static async Task<Ok<IEnumerable<FileInfoModel>>> GetAllFiles(
        IFileProcessingService service,
        CancellationToken ct)
    {
        var files = await service.GetAllFilesAsync(ct);
        return TypedResults.Ok(files);
    }

    private static async Task<Results<FileStreamHttpResult, NotFound>> GetFileById(
        Guid id,
        IFileProcessingService fileProcessingService,
        CancellationToken ct)
    {
        var file = await fileProcessingService.GetFileContentAsync(id, ct);

        if (file is null)
            return TypedResults.NotFound();

        return TypedResults.File(file.Content, "application/octet-stream", file.FileInfoModel.FileName);
    }
}
