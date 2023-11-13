using DevExpress.Blazor.Localization;
using Humanizer.Localisation;
using System.Globalization;

namespace Forces.Client.Extensions
{
    public class LocalizationService : DxLocalizationService, IDxLocalizationService
    {
        string? IDxLocalizationService.GetString(string key)
        {
            return 
                base.GetString("ar-AR");
        }
    }
}
