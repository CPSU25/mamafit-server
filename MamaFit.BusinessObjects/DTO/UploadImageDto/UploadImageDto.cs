using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MamaFit.BusinessObjects.DTO.UploadImageDto;

public class UploadImageDto
{
    [Required]
    public string Id { get; set; }
    [Required]
    public IFormFile NewImage { get; set; }
}