using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikiDataExtractor.Tests.Util
{
    internal class ResourceReader
    {
        internal static MemoryStream ReadTextResAsStream(string resourceName)
        {
            string? text = WikiDataExtractor.Tests.Properties.Resources.ResourceManager.GetString(resourceName);
            if (text == null) throw new FileNotFoundException($"String resource : {resourceName}");
            byte[] data = Encoding.UTF8.GetBytes(text);
            return new MemoryStream(data);
        }
    }
}
