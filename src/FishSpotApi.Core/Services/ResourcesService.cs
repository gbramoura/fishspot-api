using FishSpotApi.Core.Repository;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http.Request;

namespace FishSpotApi.Core.Services;

public class ResourcesService(SpotRepository spotRepository, FileService fileService)
{
    private readonly List<string> _allowedFileExtensions = [".jpg", ".jpeg", ".png"];
    
    public (FileStream resource, string Extesion) GetResource(string fileName)
    {
        if (!fileService.Exists(fileName))
        {
            throw new ImageNotFoundException("resource_not_found");
        }
        
        return (resource: fileService.ReadFile(fileName), fileService.GetFileExtension(fileName));
    }
    
    public List<string> AttachSpotResources(AttachResourcesToSpotRequest attachRequest)
    {
        var spot = spotRepository.Get(attachRequest.SpotId);
        if (spot is null)
        {
            throw new SpotNotFoundException("spot_not_found");
        }

        var savedFiles = new List<string>();
        try
        {
            attachRequest.Files.ToList().ForEach(file =>
            {
                savedFiles.Add(fileService.SaveFile(file, _allowedFileExtensions));
            });

            spot.Images.AddRange(savedFiles);
            spotRepository.Update(spot);
            return savedFiles;
        }
        catch (Exception e)
        {
            savedFiles.ForEach(fileService.DeleteFile);
            throw new InvalidImageException("resource_unable_to_save", e); 
        }
    }
    
    public void DetachSpotResources(DetachResourcesFromSpotRequest detachRequest)
    {
        var spot = spotRepository.Get(detachRequest.SpotId);
        if (spot is null)
        {
            throw new SpotNotFoundException("user_not_found");
        }

        if (!detachRequest.Files.Any(file => spot.Images.Contains(file)))
        {
            throw new ImageNotFoundException("resource_not_found");
        }
        
        var deletedFiles = new List<string>();
        try
        {
            detachRequest.Files.ToList().ForEach(file =>
            {
                fileService.DeleteFile(file);
                deletedFiles.Add(file);
            });
        }
        catch (Exception e)
        {
            throw new InvalidImageException("resource_unable_to_detach", e);
        }
        finally
        {
            spot.Images = spot.Images.Where(img => !deletedFiles.Contains(img)).ToList();
            spotRepository.Update(spot);
        }
    }
}