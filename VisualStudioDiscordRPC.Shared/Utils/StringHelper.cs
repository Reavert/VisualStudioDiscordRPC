using System.Text;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class StringHelper
    {
        public static string ReEncodeWithMaxLength(string value, int maxBytesLength, Encoding encoding)
        {
            if (encoding.GetByteCount(value) > maxBytesLength)
            {
                // ReEncode string to fit in 128 bytes.

                byte[] bytes = encoding.GetBytes(value);
                return Encoding.UTF8.GetString(bytes, 0, 128);
            }

            return value;
        }
    }
}
