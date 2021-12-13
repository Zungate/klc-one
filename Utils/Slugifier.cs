using System.Text.RegularExpressions;

namespace klc_one.Utils;

public class Slugifier : ISlugifier
{
    public string CreateSlug(string value)
    {
        //First to lower case
        value = value.ToLowerInvariant();

        //Replace spaces
        value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

        //Replace Æ
        value = Regex.Replace(value, @"æ", "ae", RegexOptions.Compiled);

        //Replace Æ
        value = Regex.Replace(value, @"ø", "oe", RegexOptions.Compiled);

        //Replace Æ
        value = Regex.Replace(value, @"å", "aa", RegexOptions.Compiled);

        //Remove invalid chars
        value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

        //Trim dashes from end
        value = value.Trim('-', '_');

        //Replace double occurences of - or _
        value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);
        return value;
    }
}