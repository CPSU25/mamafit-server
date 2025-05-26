namespace MamaFit.Repositories.Helper;

public class PhotoUploadResult
{
    public string Url { get; set; }
    public string PublicId { get; set; }
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
}
