using System.IO.Compression;

namespace Clock.Maui.Utilities;

public class ZipUtilities
{
    public static async Task<byte[]> Unzip(MemoryStream inputStream, CancellationToken cancel = default)
    {
        inputStream.Position = 0;
        using (var outputStream = new MemoryStream())
        {
            using (var compressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
            {
                await compressionStream.CopyToAsync(outputStream, cancel);
            }
            return outputStream.ToArray();
        }
    }
}