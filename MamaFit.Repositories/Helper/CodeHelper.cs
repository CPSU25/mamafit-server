namespace MamaFit.Repositories.Helper;

public static class CodeHelper
{
    public static string GenerateCode(char prefix)
    {
        var random = new Random();
        var randomNumber = random.Next(0, 100000);
        string numberPart = randomNumber.ToString("D5");
        return $"{prefix}{numberPart}";
    }
}