using System.Globalization;
using System.Text.RegularExpressions;

namespace AuraHotel.Services;

public static partial class CurrencyFormatter
{
    private static readonly NumberFormatInfo VndNumberFormat = new()
    {
        NumberGroupSeparator = ".",
        NumberDecimalSeparator = ",",
        NumberGroupSizes = [3]
    };

    public static string FormatVnd(decimal amount) =>
        $"{amount.ToString("#,##0", VndNumberFormat)} đ";

    public static bool TryParseVndAmount(string? rawInput, out decimal amount)
    {
        amount = 0m;
        if (string.IsNullOrWhiteSpace(rawInput))
        {
            return false;
        }

        var cleaned = CurrencySuffixRegex()
            .Replace(rawInput.Trim(), string.Empty)
            .Replace(" ", string.Empty, StringComparison.Ordinal)
            .Replace("\u00A0", string.Empty, StringComparison.Ordinal);

        if (cleaned.Length == 0)
        {
            return false;
        }

        string normalized;
        if (DotGroupedThousandsRegex().IsMatch(cleaned))
        {
            normalized = cleaned.Replace(".", string.Empty, StringComparison.Ordinal);
        }
        else if (CommaGroupedThousandsRegex().IsMatch(cleaned))
        {
            normalized = cleaned.Replace(",", string.Empty, StringComparison.Ordinal);
        }
        else if (WholeIntegerRegex().IsMatch(cleaned))
        {
            normalized = cleaned;
        }
        else
        {
            return false;
        }

        return decimal.TryParse(
            normalized,
            NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,
            out amount);
    }

    [GeneratedRegex(@"(?i)\s*(vnđ|vnd|đ)\s*$")]
    private static partial Regex CurrencySuffixRegex();

    [GeneratedRegex(@"^[+-]?\d{1,3}(\.\d{3})+$")]
    private static partial Regex DotGroupedThousandsRegex();

    [GeneratedRegex(@"^[+-]?\d{1,3}(,\d{3})+$")]
    private static partial Regex CommaGroupedThousandsRegex();

    [GeneratedRegex(@"^[+-]?\d+$")]
    private static partial Regex WholeIntegerRegex();
}
