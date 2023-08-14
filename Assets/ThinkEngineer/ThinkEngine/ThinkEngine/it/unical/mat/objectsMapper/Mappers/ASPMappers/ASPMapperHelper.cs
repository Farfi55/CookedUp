
using System;

namespace ThinkEngine.Mappers
{
    public static class ASPMapperHelper
    {
        internal static string AspFormat(string hierarchyLevel)
        {
            string clean = "";
            for (int i = 0; i < hierarchyLevel.Length; i++)
            {
                // first char must be a letter
                if (clean == "" && !char.IsLetter(hierarchyLevel[i]))
                    continue;

                // other chars must be letters, digits or _
                if (char.IsLetterOrDigit(hierarchyLevel[i]) || hierarchyLevel[i] == '_')
                {
                    clean += hierarchyLevel[i];
                }
            }

            if (clean == "")
                throw new ArgumentException("InvalidName, cannot generate a valid ASP name from " + hierarchyLevel);

            // first letter must be lowercase
            clean = char.ToLower(clean[0]) + (clean.Length > 1 ? clean.Substring(1) : "");


            return clean;
        }

    }
}
