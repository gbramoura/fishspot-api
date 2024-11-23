using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Core.Services;

public class FileService(IWebHostEnvironment environment)
{

    public string SaveFile(IFormFile imageFile, string[] allowedFileExtensions)
    {
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }

        var contentPath = environment.WebRootPath;
        var path = Path.Combine(contentPath, "Uploads");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var fileExtension = Path.GetExtension(imageFile.FileName);
        if (!allowedFileExtensions.Contains(fileExtension))
        {
            throw new ArgumentException($"Only {string.Join(",", allowedFileExtensions)} are allowed.");
        }
        
        var fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
        var fileNameWithPath = Path.Combine(path, fileName);
        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
        {
            imageFile.CopyTo(stream);
            return fileName;
        }
    }
    
    public void DeleteFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }
        
        var contentPath = environment.WebRootPath;
        var path = Path.Combine(contentPath, $"Uploads", fileName);

        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Invalid file path");
        }
        
        File.Delete(path);
    }

}