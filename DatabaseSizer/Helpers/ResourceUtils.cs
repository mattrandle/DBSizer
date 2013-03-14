using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace DatabaseSizer.Helpers
{
    public sealed class ResourceUtils
    {
        /// <summary>
        ///     Gets text resource from assembly.
        ///     MUST BE public resource or will give null reference exception
        /// </summary>
        public static string GetFileResourceAsString(Assembly assembly, string resourceName)
        {
            var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new NullReferenceException(string.Format("Cannot load file identified by resource {0}", resourceName));
            }
            
            string result;
            using (var streamReader = new StreamReader(stream))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        ///     Gets bitmap resource from assembly.
        ///     MUST BE public resource or will give null reference exception
        /// </summary>
        public static Bitmap GetBitmapResource(Assembly assembly, string resourceName)
        {
            var imgStream = assembly.GetManifestResourceStream(resourceName);

            if (imgStream == null)
            {
                throw new NullReferenceException(string.Format("Cannot load bitmap identified by resource {0}", resourceName));
            }

            return (Bitmap) Image.FromStream(imgStream);
        }
    }
}