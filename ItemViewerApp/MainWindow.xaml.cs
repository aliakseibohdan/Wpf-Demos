using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ItemViewerApp.ViewModel;

namespace ItemViewerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// Handles UI-level input events for item editing and focus management.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ItemTile_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is MyObjectViewModel vm)
            {
                if (vm.IsEditing)
                {
                    e.Handled = true;
                    return;
                }

                border.Focus();
                vm.IsEditing = true;
                e.Handled = true;
            }
        }

        private void ItemTile_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is Border border && border.DataContext is MyObjectViewModel vm)
            {
                vm.IsEditing = true;
            }
        }

        private void ItemTile_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!((UIElement)sender).IsKeyboardFocusWithin)
            {
                if (sender is Border border && border.DataContext is MyObjectViewModel vm)
                {
                    if (vm.IsEditing)
                    {
                        vm.IsEditing = false;
                    }
                }
            }
        }

        private void ItemTile_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender is Border border && border.DataContext is MyObjectViewModel vm)
                {
                    border.Focus();
                    vm.IsEditing = false;
                    e.Handled = true;
                }
            }
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject clickedElement = (DependencyObject)e.OriginalSource;
            ListBoxItem? itemContainer = FindAncestor<ListBoxItem>(clickedElement);

            if (itemContainer != null)
            {
                return;
            }

            UIElement focusedElement = (UIElement)Keyboard.FocusedElement;
            Border? focusedTile = FindAncestor<Border>(focusedElement);

            if (focusedTile != null && focusedTile.DataContext is MyObjectViewModel vm)
            {
                if (vm.IsEditing)
                {
                    vm.IsEditing = false;
                    FocusManager.SetFocusedElement(this, null);
                }
            }

            if (ItemsListBox.SelectedIndex != -1)
            {
                ItemsListBox.SelectedIndex = -1;
            }
        }

        private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null)
            {
                if (current is T ancestor)
                {
                    return ancestor;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            return null;
        }
    }
}