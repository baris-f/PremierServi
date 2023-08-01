using System.IO;

namespace Modules.Technical.ScriptUtils.Runtime
{
    public static class UtilsGenerator
    {
        public static string GenerateAlphaNum4Code() => Path.GetRandomFileName()[..4].ToUpper();
    }
}