using Blazored.LocalStorage;
using Forces.Client.Infrastructure.Settings;
using Forces.Shared.Constants.Storage;
using Forces.Shared.Settings;
using Forces.Shared.Wrapper;
using Microsoft.Extensions.Localization;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Forces.Client.Infrastructure.Managers.Preferences
{
    public class ClientPreferenceManager : IClientPreferenceManager
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IStringLocalizer<ClientPreferenceManager> _localizer;

        public ClientPreferenceManager(
            ILocalStorageService localStorageService,
            IStringLocalizer<ClientPreferenceManager> localizer)
        {
            _localStorageService = localStorageService;
            _localizer = localizer;
        }
        public async Task<string> GetCurrentLanguage()
        {
            var preference = await GetPreference() as ClientPreference;
            if (!string.IsNullOrEmpty(preference.LanguageCode))
            {
                return preference.LanguageCode;
         
            }

            return "en-US";
        }

        public async Task<bool> ToggleDarkModeAsync()
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                preference.IsDarkMode = !preference.IsDarkMode;
                await SetPreference(preference);
                return !preference.IsDarkMode;
            }

            return false;
        }
        public async Task<bool> ToggDrawer(bool Toggle)
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                preference.IsDrawerOpen = Toggle;
                await SetPreference(preference);
                return preference.IsDrawerOpen;
            }
            return false;
        }
        public async Task<bool> IsDrawerOpen()
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                return preference.IsDrawerOpen;
            }
            return true;
        }
        public async Task<bool> ToggleLayoutDirection()
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                preference.IsRTL = !preference.IsRTL;
                await SetPreference(preference);
                return preference.IsRTL;
            }
            return false;
        }
      
        public async Task<IResult> ChangeLanguageAsync(string languageCode)
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                preference.LanguageCode = languageCode;
                preference.IsRTL = languageCode == "ar-AR";
                await SetPreference(preference);
                return new Result
                {
                    Succeeded = true,
                    Messages = new List<string> { _localizer["Client Language has been changed"] }
                };
            }

            return new Result
            {
                Succeeded = false,
                Messages = new List<string> { _localizer["Failed to get client preferences"] }
            };
        }

        public async Task<MudTheme> GetCurrentThemeAsync()
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                if (preference.IsDarkMode == true) return BlazorHeroTheme.DarkTheme;
            }
            if (preference.LanguageCode == "ar-AR") 
            {
                BlazorHeroTheme.FontFamily = new[] { "Cairo" , "Libre Franklin" , "Montserrat", "Helvetica", "sans-serif" };
            }
 
            return BlazorHeroTheme.DefaultTheme;
        }
        public async Task<bool> IsRTL()
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                return preference.IsRTL;
            }
            return false;
        }

        public async Task<IPreference> GetPreference()
        {
            return await _localStorageService.GetItemAsync<ClientPreference>(StorageConstants.Local.Preference) ?? new ClientPreference();
        }

        public async Task SetPreference(IPreference preference)
        {
            await _localStorageService.SetItemAsync(StorageConstants.Local.Preference, preference as ClientPreference);
        }
    }
}