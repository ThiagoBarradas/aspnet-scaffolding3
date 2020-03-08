using System;
using System.IO;
using System.Text;

namespace AspNetScaffolding.Extensions.StreamExt
{
    public static class StreamExtension
    {
        public static string AsString(this Stream stream)
        {
            try
            {
                var result = "";
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                {
                    result = reader.ReadToEnd();
                }
                stream.Position = 0;

                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
