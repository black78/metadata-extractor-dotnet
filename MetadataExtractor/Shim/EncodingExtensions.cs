namespace System.Text
{
    using System;

    public static class EncodingExtensions
    {
        public static string GetString(this Encoding encoding, byte[] data)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            return encoding.GetString(data, 0, data.Length);
        }
    }
}
