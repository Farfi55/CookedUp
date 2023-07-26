
namespace ThinkEngine.Mappers
{
    public static class ASPMapperHelper
    {
        internal static string AspFormat(string hierarchyLevel)
        {
            string clean = "";
            for (int i = 0; i < hierarchyLevel.Length; i++)
            {
                if (char.IsLetterOrDigit(hierarchyLevel[i]))
                {
                    clean += hierarchyLevel[i];
                }
            }
            clean = char.ToLower(clean[0]) + (clean.Length > 1 ? clean.Substring(1) : "");
            return clean;
        }

    }
}
