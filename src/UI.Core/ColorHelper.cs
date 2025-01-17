using System.Security.Cryptography;
using Wacton.Unicolour;

namespace UI.Core;

public static class ColorHelper
{
    public static Unicolour GetColorFromInteger(int integer, ColorSpace colorSpace)
    {
        return colorSpace.Match(hsvNormal => GetColorFromIntegerHsvNormal(integer, hsvNormal));
    }

    private static Unicolour GetColorFromIntegerHsvNormal(int integer, HsvNormal hsvNormal)
    {
        var s = (integer * 7 * 3).ToString();
        var bytes = System.Text.Encoding.UTF8.GetBytes(s);

        var hashData = MD5.HashData(bytes);

        var hashString = Convert.ToHexStringLower(hashData);

        var hSnippet = hashString[..8];
        var hVal = Convert.ToUInt32(hSnippet, 16);
        var uniformHue = hVal % 360f;

        var sSnippet = hashString[8..12];
        var sVal = Convert.ToUInt32(sSnippet, 16);
        var uniformSaturation = sVal % 1000f / 1000;

        var vSnippet = hashString[12..16];
        var vVal = Convert.ToUInt32(vSnippet, 16);
        var uniformValue = vVal % 1000f / 1000;

        var saturation = NormDist.InverseCumulativeDistribution(uniformSaturation,
            hsvNormal.Saturation.Mean,
            hsvNormal.Saturation.StandardDeviation);
        saturation = Math.Clamp(saturation, 0, 1);

        var value = NormDist.InverseCumulativeDistribution(uniformValue,
            hsvNormal.Value.Mean,
            hsvNormal.Value.StandardDeviation);
        value = Math.Clamp(value, 0, 1);

        var unicolour = new Unicolour(ColourSpace.Hsl, uniformHue, saturation, value);

        if (unicolour.Hex == "-")
        {
            throw new InvalidOperationException("Invalid colour");
        }

        return unicolour;
    }
}