
using Forces.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace Forces.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}