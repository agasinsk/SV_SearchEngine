using System.Text.RegularExpressions;

namespace SearchEngine.Service.Extensions
{
    public static class StringExtensions
    {
        public static string SplitPascalCase(this string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? input
                : Regex.Replace(input, "[a-z][A-Z]", m => $"{m.Value[0]} {m.Value[1]}").TrimStart();
        }
    }
}