using System.Text;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class StringHelper
    {
        public static string RecodeToFitMaxLength(string value, int maxBytesLength, Encoding encoding)
        {
            if (value == null)
            {
                return null;
            }

            if (encoding.GetByteCount(value) > maxBytesLength)
            {
                // Recode string to fit in maxBytesLength size.

                byte[] bytes = encoding.GetBytes(value);
                return Encoding.UTF8.GetString(bytes, 0, maxBytesLength);
            }

            return value;
        }
    }
}
