using SimpleTextEditor.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SimpleTextEditor
{
    /// <summary>
    /// Represents the main window of the text editor application.
    /// Provides functionality for text editing, font customization, and file operations.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly FontService _fontService;
        private readonly FileService _fileService;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            _fontService = new FontService();
            _fileService = new FileService();

            InitializeFontControls();
            UpdateStatus("Application started successfully");
        }

        private void InitializeFontControls()
        {
            FontFamilyComboBox.ItemsSource = _fontService.GetAvailableFontFamilies();
            FontFamilyComboBox.SelectedItem = EditorTextBox.FontFamily;

            var fontSizes = new[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            FontSizeComboBox.ItemsSource = fontSizes;
            FontSizeComboBox.SelectedItem = (int)EditorTextBox.FontSize;
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontFamilyComboBox.SelectedItem is FontFamily selectedFont)
            {
                EditorTextBox.FontFamily = selectedFont;
                UpdateStatus($"Font family changed to {selectedFont.Source}");
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontSizeComboBox.SelectedItem is int size)
            {
                EditorTextBox.FontSize = size;
                UpdateStatus($"Font size changed to {size}");
            }
            else if (double.TryParse(FontSizeComboBox.Text, out double customSize))
            {
                EditorTextBox.FontSize = customSize;
                UpdateStatus($"Font size changed to {customSize}");
            }
        }

        private void BoldCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            EditorTextBox.FontWeight = BoldCheckBox.IsChecked == true ? FontWeights.Bold : FontWeights.Normal;
            UpdateStatus($"Bold {(BoldCheckBox.IsChecked == true ? "enabled" : "disabled")}");
        }

        private void ItalicCheckBox_Changed(object sender, RoutedEventArgs e)
        {
            EditorTextBox.FontStyle = ItalicCheckBox.IsChecked == true ? FontStyles.Italic : FontStyles.Normal;
            UpdateStatus($"Italic {(ItalicCheckBox.IsChecked == true ? "enabled" : "disabled")}");
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _fileService.ShowOpenDialog();
            if (result.IsSuccess && result.FilePath != null)
            {
                var contentResult = _fileService.LoadFile(result.FilePath);
                if (contentResult.IsSuccess)
                {
                    EditorTextBox.Text = contentResult.Content;
                    UpdateStatus($"File loaded: {result.FilePath}");
                }
                else
                {
                    ShowError(contentResult.ErrorMessage ?? "Failed to load file");
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var result = _fileService.ShowSaveDialog();
            if (result.IsSuccess && result.FilePath != null)
            {
                var saveResult = _fileService.SaveFile(result.FilePath, EditorTextBox.Text);
                if (saveResult.IsSuccess)
                {
                    UpdateStatus($"File saved: {result.FilePath}");
                }
                else
                {
                    ShowError(saveResult.ErrorMessage ?? "Failed to save file");
                }
            }
        }

        private void UpdateStatus(string message)
        {
            StatusTextBlock.Text = message;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            UpdateStatus($"Error: {message}");
        }
    }
}