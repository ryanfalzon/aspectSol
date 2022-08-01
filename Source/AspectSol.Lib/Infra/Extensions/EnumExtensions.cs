using System.ComponentModel;

namespace AspectSol.Lib.Infra.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T value) where T: Enum
    {
        var fi = value.GetType().GetField(value.ToString());

        if (fi == null) return string.Empty;

        var attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

        return attributes.Length > 0
            ? attributes[0].Description
            : value.ToString();
    }
}