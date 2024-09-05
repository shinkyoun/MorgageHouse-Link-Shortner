using FluentValidation;
using MorgageHouse_Link_Shortner.Server.Models;

namespace MorgageHouse_Link_Shortner.Server.Validators
{
    public class UrlRegistrationValidator : AbstractValidator<UrlShortRegistration>
    {
        public UrlRegistrationValidator()
        {
            RuleFor(x => x.FullUrl)
                .NotNull()
                .NotEmpty()
                .Must(LinkMustBeUrl)
                .WithMessage($"FullUrl is expect to be valid url");
        }

        private static bool LinkMustBeUrl(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            //Courtesy of @Pure.Krome's comment and https://stackoverflow.com/a/25654227/563532
            Uri outUri;
            var isOK = Uri.TryCreate(link, UriKind.Absolute, out outUri);
            if (!isOK)
            {
                return false;
            }

            return outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps;
        }
    }
}
