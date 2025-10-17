namespace SimpleTextEditor.Services
{
    /// <summary>
    /// Represents the result of a file dialog operation.
    /// </summary>
    public class FileDialogResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the selected file path.
        /// </summary>
        public string? FilePath { get; set; }

        /// <summary>
        /// Gets or sets the error message if the operation failed.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Represents the result of a file content operation.
    /// </summary>
    public class FileContentResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets the file content.
        /// </summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the error message if the operation failed.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Provides file operation services for the text editor.
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Shows an open file dialog and returns the result.
        /// </summary>
        /// <returns>A FileDialogResult containing the selected file path.</returns>
        FileDialogResult ShowOpenDialog();

        /// <summary>
        /// Shows a save file dialog and returns the result.
        /// </summary>
        /// <returns>A FileDialogResult containing the selected file path.</returns>
        FileDialogResult ShowSaveDialog();

        /// <summary>
        /// Loads content from the specified file path.
        /// </summary>
        /// <param name="filePath">The path of the file to load.</param>
        /// <returns>A FileContentResult containing the file content.</returns>
        FileContentResult LoadFile(string filePath);

        /// <summary>
        /// Saves content to the specified file path.
        /// </summary>
        /// <param name="filePath">The path where to save the file.</param>
        /// <param name="content">The content to save.</param>
        /// <returns>A FileContentResult indicating the success of the operation.</returns>
        FileContentResult SaveFile(string filePath, string content);
    }
}