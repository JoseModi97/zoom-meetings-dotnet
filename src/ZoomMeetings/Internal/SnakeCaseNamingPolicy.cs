using System.Text;
using System.Text.Json;

namespace ZoomMeetings.Internal;

/// <summary>
/// Converts .NET PascalCase property names (e.g. "NextPageToken") to Zoom's snake_case wire format
/// (e.g. "next_page_token"). Hand-written rather than relying on the built-in
/// JsonNamingPolicy.SnakeCaseLower (added in .NET 8) so behavior is identical across netstandard2.0,
/// net8.0, and net10.0.
/// </summary>
internal sealed class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static readonly SnakeCaseNamingPolicy Instance = new();

    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        var sb = new StringBuilder(name.Length + 8);
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (char.IsUpper(c))
            {
                if (i > 0 && (char.IsLower(name[i - 1]) || (i + 1 < name.Length && char.IsLower(name[i + 1]))))
                {
                    sb.Append('_');
                }
                sb.Append(char.ToLowerInvariant(c));
            }
            else
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
