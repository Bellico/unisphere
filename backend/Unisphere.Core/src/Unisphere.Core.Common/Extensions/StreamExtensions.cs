using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Unisphere.Core.Common.Extensions;

public static class StreamExtensions
{
    public static byte[] ToArray(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using (var memoryStreamTemp = new MemoryStream())
        {
            stream.CopyTo(memoryStreamTemp);
            return memoryStreamTemp.ToArray();
        }
    }

    public static bool IsSameAs(this Stream a, Stream b)
    {
        if (a == null && b == null)
        {
            return true;
        }

        if (a == null || b == null)
        {
            throw new ArgumentNullException(a == null ? nameof(a) : nameof(b));
        }

        if (a.CanSeek && b.CanSeek && (a.Length != b.Length))
        {
            return false;
        }

        while (true)
        {
            int aByte = a.ReadByte();
            int bByte = b.ReadByte();

            if (aByte == -1 && bByte == -1)
            {
                return true;
            }

            if (aByte == -1 || bByte == -1)
            {
                return false;
            }

            if (aByte.CompareTo(bByte) != 0)
            {
                return false;
            }
        }
    }

    public static string Sha256(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        stream.ResetPosition();
        using (var sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(stream);

            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2", CultureInfo.InvariantCulture));
            }

            stream.ResetPosition();

            return builder.ToString();
        }
    }

    public static Stream ResetPosition(this Stream stream)
    {
        if (stream != null && stream.CanSeek && stream.Position != 0)
        {
            stream.Position = 0;
        }

        return stream!;
    }

    public static string Base64(this Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);

        var result = Convert.ToBase64String(stream.ResetPosition().ToArray());
        stream.ResetPosition();

        return result;
    }
}
