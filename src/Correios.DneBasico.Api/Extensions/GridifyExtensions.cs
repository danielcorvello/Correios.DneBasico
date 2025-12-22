using System.Text;

namespace Correios.DneBasico.Api.Extensions;

public static class GridifyExtensions
{
    public static string GetMappingList<T>(this GridifyMapper<T> mapper)
    {
        var mappings = mapper.GetCurrentMaps();
        return mappings.Select(m => m.From).ToList().ConcatenateWithAnd("e");
    }

    private static string ConcatenateWithAnd(this List<string> list, string andWord = "and")
    {
        var and = andWord;

        if (list.Count == 0) return string.Empty;
        if (list.Count == 1) return list[0];
        if (list.Count == 2) return $"{list[0]} {and} {list[1]}";

        var sb = new StringBuilder();
        sb.AppendJoin(", ", list.Take(list.Count - 1));
        sb.Append($" {and} ");
        sb.Append(list.Last());

        return sb.ToString();
    }
}