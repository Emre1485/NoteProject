using Microsoft.EntityFrameworkCore;

namespace NoteProject.API.Shared
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string noteName);
        void DeleteFile(string filePath);
    }
    public class FileService : IFileService
    {
        private readonly string _uploadDirectory = "Uploads";
        private readonly List<string> AllowedExtensions = new List<string> { ".pdf", ".jpeg", ".jpg", ".doc", ".docx", ".txt" };

        public void DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), _uploadDirectory, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string noteName)
        {
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!AllowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Geçersiz dosya türü. Desteklenen uzantılar: pdf, jpeg, jpg, doc, docx, txt");
            }

            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), _uploadDirectory);
            var uniqueFileName = $"{noteName}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, uniqueFileName);

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return uniqueFileName;
        }


    }
}
