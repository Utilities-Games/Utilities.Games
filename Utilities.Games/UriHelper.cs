using System.Collections.Generic;
using System.IO;

namespace Utilities.Games
{
    public static class UriHelper
    {
        public static string CleanAndEncodeImageUrl(string imageUrl) {
            string[] paths = imageUrl.Split("/");
            var newPaths = new List<string>();
            char[] invalidChars = new char[] { '<', '>', ':', '"', '/', '\\', '|', '?', '*' };
            foreach (string path in paths)
            {
                newPaths.Add(string.Concat(path.Split(invalidChars)));
            }
            return string.Join('/', newPaths.ToArray());
        }
    }
}
