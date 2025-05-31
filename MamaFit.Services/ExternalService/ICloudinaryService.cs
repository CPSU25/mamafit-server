using CloudinaryDotNet.Actions;
using MamaFit.Repositories.Helper;
using Microsoft.AspNetCore.Http;

namespace MamaFit.Services.ExternalService;

public interface ICloudinaryService
{
    Task<DeletionResult> DeletePhotoAsync(string publicId);
    Task<PhotoUploadResult> AddPhotoAsync(IFormFile file);
    string GetCloudinaryPublicIdFromUrl(string imageUrl);
}