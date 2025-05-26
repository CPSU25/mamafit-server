using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using MamaFit.Repositories.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MamaFit.Services.ExternalService;

public class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    
    public CloudinaryService(IOptions<CloudinarySettings> config)
    {
        var account = new Account(
            config.Value.CloudName,
            config.Value.ApiKey,
            config.Value.ApiSecret
            );
        _cloudinary = new Cloudinary(account);
    }
    
    public async Task<PhotoUploadResult> AddPhotoAsync(IFormFile file)
    {
        var result = new PhotoUploadResult();

        if (file == null || file.Length == 0)
        {
            result.IsSuccess = false;
            result.ErrorMessage = "File is empty.";
            return result;
        }

        using var stream = file.OpenReadStream();
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Folder = "mamafit_photos",
            PublicId = $"mamafit_{Guid.NewGuid()}",
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            result.IsSuccess = false;
            result.ErrorMessage = uploadResult.Error.Message;
        }
        else
        {
            result.IsSuccess = true;
            result.Url = uploadResult.SecureUrl?.ToString();
            result.PublicId = uploadResult.PublicId;
            result.ErrorMessage = null;
        }

        return result;
    }

    public async Task<DeletionResult> DeletePhotoAsync(string publicId)
    {
        var deleteParams = new DeletionParams(publicId);
        var result = await _cloudinary.DestroyAsync(deleteParams);
        return result;
    }
}