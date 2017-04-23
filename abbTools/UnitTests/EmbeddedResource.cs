using System.IO;
using System.Linq;
using System.Reflection;

namespace abbTools.UnitTests
{
    class EmbeddedResource
    {
        public static string getResource(string name)
        {
            Assembly currAssembly = Assembly.GetExecutingAssembly();
            string resourceName = currAssembly.GetManifestResourceNames().SingleOrDefault(x => x.EndsWith(name));
            Stream resourceStream = currAssembly.GetManifestResourceStream(resourceName);

            return new StreamReader(resourceStream).ReadToEnd();
        }
    }
}
