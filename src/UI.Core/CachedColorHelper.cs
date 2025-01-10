using System.Security.Cryptography;
using Wacton.Unicolour;

namespace UI.Core;

public readonly record struct NormalParameters(float Mean, float StandardDeviation);

public readonly record struct HsvNormal(NormalParameters Saturation, NormalParameters Value)
{
    public static readonly HsvNormal Default = new(
        new NormalParameters(0.5f, 0.2f),
        new NormalParameters(0.6f, 0.05f));
}

[GenerateOneOf]
public partial class ColorSpace : OneOfBase<HsvNormal>;

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

public static class CachedColorHelper
{
    private static readonly Dictionary<int, Unicolour> _cachedColors = new();

    public static Unicolour GetColorFromInteger(int integer, ColorSpace colorSpace)
    {
        if (_cachedColors.TryGetValue(integer, out var color))
        {
            return color;
        }

        color = ColorHelper.GetColorFromInteger(integer, colorSpace);
        _cachedColors[integer] = color;

        return color;
    }

    public static ColorString GetColorStringFromInteger(int integer, ColorSpace colorSpace)
    {
        var color = GetColorFromInteger(integer, colorSpace);
        return new ColorString(color.Hex);
    }
}

# region Garbage

public static class NormDist
{
    private static readonly float[] a =
    [
        -39.69683028665376f, 220.9460984245205f, -275.928137101359f, 138.357751867269f, -30.66479806614716f,
        2.506628277459239f,
    ];

    private static readonly float[] b =
    [
        -54.47609879822406f, 161.5858368580409f, -155.6989798598866f, 66.80131188771972f, -13.28068155288572f,
    ];

    private static readonly float[] c =
    [
        -7.784894002430293e-03f, -0.322396458041136f, -2.400758277161838f, -2.549732539343734f, 4.131722663299258f,
        3.975778608302636f,
    ];

    private static readonly float[] d =
    [
        0.000000000000000f, 0.010622412810379f, 0.005834142130105f, 0.000199467757246f, 0.000016806067288f,
        0.000000033018201f,
    ];

    // Norm.ppf implementation using approximation

    // Norm.ppf implementation with loc and scale support
    public static float InverseCumulativeDistribution(float p, float loc, float scale)
    {
        var sample = ErfcInv(2.0f * p) * MathF.Sqrt(2.0f);

        return loc - scale * sample;
    }

    public static float ErfcInv(float z)
    {
        if (z <= 0.0f)
        {
            return float.PositiveInfinity;
        }

        if (z >= 2.0f)
        {
            return float.NegativeInfinity;
        }

        float p, q, s;
        if (z > 1)
        {
            q = 2 - z;
            p = 1 - q;
            s = -1;
        }
        else
        {
            p = 1 - z;
            q = z;
            s = 1;
        }

        return ErfInvImpl(p, q, s);
    }

    private static readonly double[] ErvInvImpAn =
    {
        -0.000508781949658280665617,
        -0.00836874819741736770379,
        0.0334806625409744615033,
        -0.0126926147662974029034,
        -0.0365637971411762664006,
        0.0219878681111168899165,
        0.00822687874676915743155,
        -0.00538772965071242932965,
    };

    private static readonly double[] ErvInvImpAd =
    {
        1,
        -0.970005043303290640362,
        -1.56574558234175846809,
        1.56221558398423026363,
        0.662328840472002992063,
        -0.71228902341542847553,
        -0.0527396382340099713954,
        0.0795283687341571680018,
        -0.00233393759374190016776,
        0.000886216390456424707504,
    };

    private static readonly double[] ErvInvImpBn =
    {
        -0.202433508355938759655,
        0.105264680699391713268,
        8.37050328343119927838,
        17.6447298408374015486,
        -18.8510648058714251895,
        -44.6382324441786960818,
        17.445385985570866523,
        21.1294655448340526258,
        -3.67192254707729348546,
    };

    private static readonly double[] ErvInvImpBd =
    {
        1,
        6.24264124854247537712,
        3.9713437953343869095,
        -28.6608180499800029974,
        -20.1432634680485188801,
        48.5609213108739935468,
        10.8268667355460159008,
        -22.6436933413139721736,
        1.72114765761200282724,
    };

    private static readonly double[] ErvInvImpCn =
    {
        -0.131102781679951906451,
        -0.163794047193317060787,
        0.117030156341995252019,
        0.387079738972604337464,
        0.337785538912035898924,
        0.142869534408157156766,
        0.0290157910005329060432,
        0.00214558995388805277169,
        -0.679465575181126350155e-6,
        0.285225331782217055858e-7,
        -0.681149956853776992068e-9,
    };

    private static readonly double[] ErvInvImpCd =
    {
        1,
        3.46625407242567245975,
        5.38168345707006855425,
        4.77846592945843778382,
        2.59301921623620271374,
        0.848854343457902036425,
        0.152264338295331783612,
        0.01105924229346489121,
    };

    private static readonly double[] ErvInvImpDn =
    {
        -0.0350353787183177984712,
        -0.00222426529213447927281,
        0.0185573306514231072324,
        0.00950804701325919603619,
        0.00187123492819559223345,
        0.000157544617424960554631,
        0.460469890584317994083e-5,
        -0.230404776911882601748e-9,
        0.266339227425782031962e-11,
    };

    private static readonly double[] ErvInvImpDd =
    {
        1,
        1.3653349817554063097,
        0.762059164553623404043,
        0.220091105764131249824,
        0.0341589143670947727934,
        0.00263861676657015992959,
        0.764675292302794483503e-4,
    };

    private static readonly double[] ErvInvImpEn =
    {
        -0.0167431005076633737133,
        -0.00112951438745580278863,
        0.00105628862152492910091,
        0.000209386317487588078668,
        0.149624783758342370182e-4,
        0.449696789927706453732e-6,
        0.462596163522878599135e-8,
        -0.281128735628831791805e-13,
        0.99055709973310326855e-16,
    };

    private static readonly double[] ErvInvImpEd =
    {
        1,
        0.591429344886417493481,
        0.138151865749083321638,
        0.0160746087093676504695,
        0.000964011807005165528527,
        0.275335474764726041141e-4,
        0.282243172016108031869e-6,
    };

    private static readonly double[] ErvInvImpFn =
    {
        -0.0024978212791898131227,
        -0.779190719229053954292e-5,
        0.254723037413027451751e-4,
        0.162397777342510920873e-5,
        0.396341011304801168516e-7,
        0.411632831190944208473e-9,
        0.145596286718675035587e-11,
        -0.116765012397184275695e-17,
    };

    private static readonly double[] ErvInvImpFd =
    {
        1,
        0.207123112214422517181,
        0.0169410838120975906478,
        0.000690538265622684595676,
        0.145007359818232637924e-4,
        0.144437756628144157666e-6,
        0.509761276599778486139e-9,
    };

    private static readonly double[] ErvInvImpGn =
    {
        -0.000539042911019078575891,
        -0.28398759004727721098e-6,
        0.899465114892291446442e-6,
        0.229345859265920864296e-7,
        0.225561444863500149219e-9,
        0.947846627503022684216e-12,
        0.135880130108924861008e-14,
        -0.348890393399948882918e-21,
    };

    private static readonly double[] ErvInvImpGd =
    {
        1,
        0.0845746234001899436914,
        0.00282092984726264681981,
        0.468292921940894236786e-4,
        0.399968812193862100054e-6,
        0.161809290887904476097e-8,
        0.231558608310259605225e-11,
    };


    private static float ErfInvImpl(float p, float q, float s)
    {
        float result;

        if (p <= 0.5)
        {
            const float y = 0.0891314744949340820313f;
            var g = p * (p + 10);
            var r = EvaluatePolynomial(p, ErvInvImpAn) / EvaluatePolynomial(p, ErvInvImpAd);
            result = g * y + g * r;
        }
        else if (q >= 0.25)
        {
            const float y = 2.249481201171875f;
            var g = MathF.Sqrt(-2 * MathF.Log(q));
            var xs = q - 0.25f;
            var r = EvaluatePolynomial(xs, ErvInvImpBn) / EvaluatePolynomial(xs, ErvInvImpBd);
            result = g / (y + r);
        }
        else
        {
            var x = MathF.Sqrt(-MathF.Log(q));
            if (x < 3)
            {
                // Max error found: 1.089051e-20
                const float y = 0.807220458984375f;
                var xs = x - 1.125f;
                var r = EvaluatePolynomial(xs, ErvInvImpCn) / EvaluatePolynomial(xs, ErvInvImpCd);
                result = y * x + r * x;
            }
            else if (x < 6)
            {
                // Max error found: 8.389174e-21
                const float y = 0.93995571136474609375f;
                var xs = x - 3;
                var r = EvaluatePolynomial(xs, ErvInvImpDn) / EvaluatePolynomial(xs, ErvInvImpDd);
                result = y * x + r * x;
            }
            else if (x < 18)
            {
                // Max error found: 1.481312e-19
                const float y = 0.98362827301025390625f;
                var xs = x - 6;
                var r = EvaluatePolynomial(xs, ErvInvImpEn) / EvaluatePolynomial(xs, ErvInvImpEd);
                result = y * x + r * x;
            }
            else if (x < 44)
            {
                // Max error found: 5.697761e-20
                const float y = 0.99714565277099609375f;
                var xs = x - 18;
                var r = EvaluatePolynomial(xs, ErvInvImpFn) / EvaluatePolynomial(xs, ErvInvImpFd);
                result = y * x + r * x;
            }
            else
            {
                // Max error found: 1.279746e-20
                const float y = 0.99941349029541015625f;
                var xs = x - 44;
                var r = EvaluatePolynomial(xs, ErvInvImpGn) / EvaluatePolynomial(xs, ErvInvImpGd);
                result = y * x + r * x;
            }
        }

        return s * result;
    }

    private static float EvaluatePolynomial(float z, double[] coefficients)
    {
        if (coefficients == null)
        {
            throw new ArgumentNullException(nameof(coefficients));
        }

        var n = coefficients.Length;
        if (n == 0)
        {
            return 0;
        }

        var sum = coefficients[n - 1];
        for (var i = n - 2; i >= 0; --i)
        {
            sum *= z;
            sum += coefficients[i];
        }

        return (float)sum;
    }


}

# endregion