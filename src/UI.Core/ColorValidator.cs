using FluentValidation;

namespace UI.Core;

public class ColorStringValidator
    : AbstractValidator<string>
{
    private readonly bool _supportsAlpha;
    private readonly bool _supportsSingleLetterChannels;

    private const string ColorCharacters = "0123456789ABCDEFabcdef";

    private bool Length4Allowed => _supportsSingleLetterChannels;
    private bool Length5Allowed => _supportsAlpha && _supportsSingleLetterChannels;
    private bool Length7Allowed => true;
    private bool Length9Allowed => _supportsAlpha;

    private IEnumerable<int> GetAllowedLengths()
    {
        if (Length4Allowed)
        {
            yield return 4;
        }

        if (Length5Allowed)
        {
            yield return 5;
        }

        if (Length7Allowed)
        {
            yield return 7;
        }

        if (Length9Allowed)
        {
            yield return 9;
        }
    }

    private string AllowedLengthsMessage =>
        $"Color value must be one of the following lengths: {string.Join(", ", GetAllowedLengths())}";

    public ColorStringValidator(bool supportsAlpha = true, bool supportsSingleLetterChannels = true)
    {
        _supportsAlpha = supportsAlpha;
        _supportsSingleLetterChannels = supportsSingleLetterChannels;

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x))
            .WithMessage("No color value provided")
            .Must(x => GetAllowedLengths()
                .Contains(x.Length))
            .WithMessage(AllowedLengthsMessage)
            .Must(x => x[0] == '#')
            .WithMessage("Color value must start with #")
            .Must(x => x
                .Skip(1)
                .All(c => ColorCharacters.Contains(c)))
            .WithMessage("Color values must be hexadecimal");
    }
}