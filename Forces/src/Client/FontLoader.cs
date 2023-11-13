using DevExpress.Blazor.Reporting;
using DevExpress.Drawing;
using DevExpress.XtraReports.Native;

namespace Forces.Client
{
    public static class FontLoader
    {
        public async static Task LoadFonts(HttpClient httpClient, List<string> fontNames)
        {
            foreach (var fontName in fontNames)
            {
                var fontBytes = await httpClient.GetByteArrayAsync($"fonts/{fontName}");
                DXFontRepository.Instance.AddFont(fontBytes);
            }
        }
    }
}
