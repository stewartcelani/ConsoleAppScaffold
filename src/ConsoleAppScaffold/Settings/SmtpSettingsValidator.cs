using FluentValidation;

namespace ConsoleAppScaffold.Settings;

public class SmtpSettingsValidator : AbstractValidator<SmtpSettings>
{
    public SmtpSettingsValidator()
    {
        // Server must be non-empty and a valid hostname.
        RuleFor(x => x.Server)
            .NotEmpty().WithMessage("SMTP server is required.")
            .Must(BeAValidHostname).WithMessage("SMTP server must be a valid hostname.");

        // Port should be between 1 and 65535.
        RuleFor(x => x.Port)
            .InclusiveBetween(1, 65535).WithMessage("SMTP port must be between 1 and 65535.");

        // FromAddress must be a valid email address.
        RuleFor(x => x.FromAddress)
            .NotEmpty().WithMessage("From address is required.")
            .EmailAddress().WithMessage("From address must be a valid email address.");

        // CcAddresses: each address must be a valid email.
        RuleForEach(x => x.CcAddresses)
            .EmailAddress().WithMessage("All CC addresses must be valid email addresses.");

        // BccAddresses: each address must be a valid email.
        RuleForEach(x => x.BccAddresses)
            .EmailAddress().WithMessage("All BCC addresses must be valid email addresses.");
    }

    private static bool BeAValidHostname(string hostname)
    {
        // This is a basic hostname validation. Consider more comprehensive checks if needed.
        return Uri.CheckHostName(hostname) != UriHostNameType.Unknown;
    }
}