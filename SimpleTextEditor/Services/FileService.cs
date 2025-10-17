using Microsoft.Win32;
using System.IO;

namespace SimpleTextEditor.Services
{
    /// <summary>
    /// Implementation of IFileService providing file operation functionality.
    /// </summary>
    public class FileService : IFileService
    {
        /// <summary>
        /// Shows an open file dialog and returns the result.
        /// </summary>
        /// <returns>A FileDialogResult containing the selected file path.</returns>
        public FileDialogResult ShowOpenDialog()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Open Text File"
            };

            var result = dialog.ShowDialog();
            return new FileDialogResult
            {
                IsSuccess = result == true,
                FilePath = dialog.FileName
            };
        }

        /// <summary>
        /// Shows a save file dialog and returns the result.
        /// </summary>
        /// <returns>A FileDialogResult containing the selected file path.</returns>
        public FileDialogResult ShowSaveDialog()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Save Text File"
            };

            var result = dialog.ShowDialog();
            return new FileDialogResult
            {
                IsSuccess = result == true,
                FilePath = dialog.FileName
            };
        }

        /// <summary>
        /// Loads content from the specified file path.
        /// </summary>
        /// <param name="filePath">The path of the file to load.</param>
        /// <returns>A FileContentResult containing the file content.</returns>
        public FileContentResult LoadFile(string filePath)
        {
            try
            {
                var content = File.ReadAllText(filePath);
                return new FileContentResult
                {
                    IsSuccess = true,
                    Content = content
                };
            }
            catch (Exception ex)
            {
                return new FileContentResult
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        /// <summary>
        /// Saves content to the specified file path.
        /// </summary>
        /// <param name="filePath">The path where to save the file.</param>
        /// <param name="content">The content to save.</param>
        /// <returns>A FileContentResult indicating the success of the operation.</returns>
        public FileContentResult SaveFile(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                return new FileContentResult
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new FileContentResult
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}