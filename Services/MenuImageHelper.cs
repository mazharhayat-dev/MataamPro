namespace MataamPro.Services;

public static class MenuImageHelper
{
    private const string Placeholder = "/images/menu/placeholder.svg";

    public static string GetUrl(string? imageUrl, IWebHostEnvironment environment)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Placeholder;
        }

        if (Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
        {
            return imageUrl;
        }

        var relativePath = imageUrl.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var filePath = Path.Combine(environment.WebRootPath, relativePath);
        return File.Exists(filePath) ? imageUrl : Placeholder;
    }
}
