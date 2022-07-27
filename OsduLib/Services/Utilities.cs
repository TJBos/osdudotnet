using System.Text;
using System.Text.RegularExpressions;

namespace OsduLib.Services;

public static class Utilities
{
    public static string Base64Encode(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string? Base64Decode(string? base64EncodedData)
    {
        if (string.IsNullOrEmpty(base64EncodedData)) return base64EncodedData;

        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

    // Code from https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
    public static string Slugify(string phrase)
    {
        // invalid chars           
        string str = Regex.Replace(phrase, @"[^a-z0-9\s-]", "");
        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim();
        // cut and trim 
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-"); // hyphens   
        return str;
    }
}