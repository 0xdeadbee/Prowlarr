using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using NzbDrone.Common.Extensions;
using NzbDrone.Core.Annotations;
using NzbDrone.Core.ThingiProvider;
using NzbDrone.Core.Validation;

namespace NzbDrone.Core.Indexers.Newznab
{
    public class NewznabSettingsValidator : AbstractValidator<NewznabSettings>
    {
        private static readonly string[] ApiKeyWhiteList =
        {
            "nzbs.org",
            "nzb.su",
            "dognzb.cr",
            "nzbplanet.net",
            "nzbid.org",
            "nzbndx.com",
            "nzbindex.in"
        };

        private static bool ShouldHaveApiKey(NewznabSettings settings)
        {
            if (settings.BaseUrl == null)
            {
                return false;
            }

            return ApiKeyWhiteList.Any(c => settings.BaseUrl.ToLowerInvariant().Contains(c));
        }

        private static readonly Regex AdditionalParametersRegex = new Regex(@"(&.+?\=.+?)+", RegexOptions.Compiled);

        public NewznabSettingsValidator()
        {
            RuleFor(c => c.BaseUrl).ValidRootUrl();
            RuleFor(c => c.ApiPath).ValidUrlBase("/api");
            RuleFor(c => c.ApiKey).NotEmpty().When(ShouldHaveApiKey);
            RuleFor(c => c.AdditionalParameters).Matches(AdditionalParametersRegex)
                                                .When(c => !c.AdditionalParameters.IsNullOrWhiteSpace());
        }
    }

    public class NewznabSettings : IProviderConfig
    {
        private static readonly NewznabSettingsValidator Validator = new NewznabSettingsValidator();

        public NewznabSettings()
        {
            ApiPath = "/api";
        }

        [FieldDefinition(0, Label = "URL")]
        public string BaseUrl { get; set; }

        [FieldDefinition(1, Label = "API Path", HelpText = "Path to the api, usually /api", Advanced = true)]
        public string ApiPath { get; set; }

        [FieldDefinition(2, Label = "API Key", Privacy = PrivacyLevel.ApiKey)]
        public string ApiKey { get; set; }

        [FieldDefinition(5, Label = "Additional Parameters", HelpText = "Additional Newznab parameters", Advanced = true)]
        public string AdditionalParameters { get; set; }

        public List<IndexerCategory> Categories { get; set; }

        // Field 8 is used by TorznabSettings MinimumSeeders
        // If you need to add another field here, update TorznabSettings as well and this comment
        public virtual NzbDroneValidationResult Validate()
        {
            return new NzbDroneValidationResult(Validator.Validate(this));
        }
    }
}
