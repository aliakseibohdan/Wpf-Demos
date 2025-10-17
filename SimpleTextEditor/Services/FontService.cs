using System.Windows.Media;

namespace SimpleTextEditor.Services
{
    /// <summary>
    /// Implementation of IFontService providing font-related functionality.
    /// </summary>
    public class FontService : IFontService
    {
        /// <summary>
        /// Gets all available font families on the system.
        /// </summary>
        /// <returns>A sorted collection of available FontFamily objects.</returns>
        public IEnumerable<FontFamily> GetAvailableFontFamilies() => [.. Fonts.SystemFontFamilies.OrderBy(f => f.Source)];
    }
}