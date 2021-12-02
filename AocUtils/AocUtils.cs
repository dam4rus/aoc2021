using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AocUtils
{
    public static class AocUtils
    {
        public static IEnumerable<string> ReadLinesFromAssemblyResource(Assembly assembly, string resourceName)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var streamReader = new StreamReader(stream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        yield return line;
                    }
                }
            }
        }
    }
}
