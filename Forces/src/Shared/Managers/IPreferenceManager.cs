using Forces.Shared.Settings;
using Forces.Shared.Wrapper;
using System.Threading.Tasks;

namespace Forces.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}