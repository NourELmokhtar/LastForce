using Forces.Application.Interfaces.Serialization.Options;
using System.Text.Json;

namespace Forces.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}