using FishSpotApi.Core.Repository;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http.Request;
using Microsoft.AspNetCore.Http;

namespace FishSpotApi.Core.Services;

public class ResourcesService(SpotRepository spotRepository, FileService fileService)
{
    public (FileStream resource, string Extesion) GetResource(string fileName)
    {
        if (!fileService.Exists(fileName))
        {
            throw new ImageNotFoundException("Resource not found");
        }
            
        var file = fileService.ReadFile(fileName);
        var extension = fileService.GetFileExtension(fileName);
            
        return (resource: file, extension);
    }
    
    public IEnumerable<string> AttachSpotResources(AttachResourcesToSpotRequest attachRequest)
    {
        var spot = spotRepository.Get(attachRequest.SpotId);
        if (spot is null)
        {
            throw new SpotNotFoundException("Spot not found");
        }

        var allowedFileExtensions = new [] { ".jpg", ".jpeg", ".png" };
        var savedFiles = new List<string>();
        try
        {
            foreach (var file in attachRequest.Files)
            {
                var savedFile = fileService.SaveFile(file, allowedFileExtensions);
                savedFiles.Add(savedFile);
                spot.Images = spot.Images.Append(savedFile);
            }
            spotRepository.Update(spot);
            return savedFiles;
        }
        catch (Exception e)
        {
            foreach (var file in savedFiles)
            {
                fileService.DeleteFile(file);
            }
            
            throw new InvalidImageException("Unable to save files", e); 
        }
    }


    public void DetachSpotResources(DetachResourcesFromSpotRequest detachRequest)
    {
        var spot = spotRepository.Get(detachRequest.SpotId);
        if (spot is null)
        {
            throw new SpotNotFoundException("Spot not found");
        }

        if (!detachRequest.Files.Any(file => spot.Images.Contains(file)))
        {
            throw new ImageNotFoundException("Image not found");
        }
        
        var deletedFiles = new List<string>();
        try
        {
            foreach (var file in detachRequest.Files)
            {
                fileService.DeleteFile(file);
                deletedFiles.Add(file);
            }
        }
        catch (Exception e)
        {
            throw new InvalidImageException("An error was caught when tried to detach the resources", e);
        }
        finally
        {
            spot.Images = spot.Images.Where(img => deletedFiles.Contains(img));
            spotRepository.Update(spot);
        }
        
    }
}