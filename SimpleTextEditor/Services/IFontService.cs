using System.Windows.Media;

namespace SimpleTextEditor.Services
{
    /// <summary>
    /// Provides font-related services for the text editor.
    /// </summary>
    public interface IFontService
    {
        /// <summary>
        /// Gets all available font families on the system.
        /// </summary>
        /// <returns>A collection of available FontFamily objects.</returns>
        IEnumerable<FontFamily> GetAvailableFontFamilies();
    }
}