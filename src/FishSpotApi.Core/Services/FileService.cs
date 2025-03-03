using FishSpotApi.Domain.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Core.Services;

public class FileService(IWebHostEnvironment environment, IStringLocalizerFactory factory)
{
    private readonly IStringLocalizer _localizer = factory.Create(typeof(FishSpotResource));
    private readonly string _contentPath = environment.ContentRootPath;
    private readonly string _uploadPath = "Uploads";
    
    public string GetFileExtension(string fileName) => Path.Combine(_contentPath, _uploadPath, fileName).Split('.').Last();
    
    public bool Exists(string fileName) =>  File.Exists(Path.Combine(_contentPath, _uploadPath, fileName));
    
    public string SaveFile(IFormFile imageFile, List<string> allowedFileExtensions)
    {
        if (imageFile == null)
        {
            throw new ArgumentNullException(nameof(imageFile));
        }
        
        var path = Path.Combine(_contentPath, _uploadPath);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var fileExtension = Path.GetExtension(imageFile.FileName);
        if (!allowedFileExtensions.Contains(fileExtension))
        {
            throw new ArgumentException(_localizer["resource_file_allowed", string.Join(",", allowedFileExtensions)]);
        }
        
        var fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
        var fileNameWithPath = Path.Combine(path, fileName);
        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
        {
            imageFile.CopyTo(stream);
            return fileName;
        }
    }
    
    public FileStream ReadFile(string fileName)
    {
        if (!Exists(fileName))
        {
            throw new FileNotFoundException(_localizer["resource_file_not_found", fileName]);
        }
        return new FileStream(Path.Combine(_contentPath, _uploadPath, fileName), FileMode.Open);
    }
    
    public void DeleteFile(string fileName)
    {
        if (!Exists(fileName))
        {
            throw new FileNotFoundException(_localizer["resource_file_not_found", fileName]);
        }
        File.Delete(Path.Combine(_contentPath, _uploadPath, fileName));
    }

    
}