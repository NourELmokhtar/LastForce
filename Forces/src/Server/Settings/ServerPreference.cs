using Forces.Shared.Constants.Localization;
using Forces.Shared.Settings;
using System.Linq;

namespace Forces.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";

        //TODO - add server preferences
    }
}