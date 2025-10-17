using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using ItemViewerApp.Model;

namespace ItemViewerApp.ViewModel
{
    /// <summary>
    /// The main ViewModel for the application, managing the collection of items and global state.
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private int _generationCount = 5;
        private int _totalCount;
        private readonly Random _random = new();

        /// <summary>
        /// Gets the observable collection of <see cref="MyObjectViewModel"/> items displayed in the main list.
        /// </summary>
        /// <value>A collection of item ViewModels.</value>
        public ObservableCollection<MyObjectViewModel> Items { get; } = [];

        /// <summary>
        /// Gets or sets the number of items to generate when the Generate command is executed.
        /// </summary>
        public int GenerationCount
        {
            get => _generationCount;
            set { _generationCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets or sets the total count of all items across the collection.
        /// </summary>
        public int TotalCount
        {
            get => _totalCount;
            set { _totalCount = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Gets the command to generate a new set of random items based on <see cref="GenerationCount"/>.
        /// </summary>
        public ICommand GenerateCommand { get; }

        /// <summary>
        /// Gets the command to add a new, empty item to the collection.
        /// </summary>
        public ICommand AddNewCommand { get; }

        /// <summary>
        /// Gets the command to remove a specific item from the collection.
        /// The command parameter is expected to be a <see cref="MyObjectViewModel"/>.
        /// </summary>
        public ICommand RemoveItemCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// Sets up commands, initializes the collection with a couple of default items, and calculates the initial total.
        /// </summary>
        public MainViewModel()
        {
            GenerateCommand = new RelayCommand(GenerateItems);
            AddNewCommand = new RelayCommand(AddNewItem);
            RemoveItemCommand = new RelayCommand(RemoveItem);

            var composite = new CompositeCollection
            {
                new CollectionContainer { Collection = Items },
                new AddNewMarker()
            };

            CompositeItems = composite;

            Items.Add(new MyObjectViewModel(new MyObject { Name = "name 1", ItemsCount = 1 }, this));
            Items.Add(new MyObjectViewModel(new MyObject { Name = "name 2", ItemsCount = 3 }, this));
            UpdateTotal();
        }

        /// <summary>
        /// Gets the composite collection used for binding to the ListBox, which includes
        /// the main items and the <see cref="AddNewMarker"/> for the "Add New" button UI.
        /// </summary>
        public CompositeCollection CompositeItems { get; }

        private void GenerateItems(object? parameter)
        {
            Items.Clear();
            for (int i = 1; i <= GenerationCount; i++)
            {
                var newObject = new MyObject
                {
                    Name = $"Item {i}",
                    ItemsCount = _random.Next(1, 11)
                };
                Items.Add(new MyObjectViewModel(newObject, this));
            }
            UpdateTotal();
        }

        private void AddNewItem(object? parameter)
        {
            int index = Items.Count + 1;
            string newName = $"New Item {index}";
            while (IsNameDuplicate(newName))
            {
                index++;
                newName = $"New Item {index}";
            }

            var newItem = new MyObjectViewModel(new MyObject { Name = newName, ItemsCount = 1 }, this);
            Items.Add(newItem);

            newItem.IsEditing = true;
            UpdateTotal();
        }

        private void RemoveItem(object? parameter)
        {
            if (parameter is MyObjectViewModel item)
            {
                Items.Remove(item);
                UpdateTotal();
            }
        }

        /// <summary>
        /// Recalculates the total count of all items in the <see cref="Items"/> collection
        /// and updates the <see cref="TotalCount"/> property.
        /// </summary>
        public void UpdateTotal()
        {
            TotalCount = Items.Sum(i => i.ItemsCount);
        }

        /// <summary>
        /// Checks if the name of the given item is a duplicate among other items in the collection.
        /// </summary>
        /// <param name="currentItem">The item ViewModel being checked.</param>
        /// <returns><c>true</c> if the item's name is a duplicate; otherwise, <c>false</c>.</returns>
        public bool IsNameDuplicate(MyObjectViewModel currentItem)
        {
            return Items.Any(i => i != currentItem && i.Name == currentItem.Name);
        }

        /// <summary>
        /// Checks if a given name string is a duplicate of any existing item's name in the collection.
        /// </summary>
        /// <param name="name">The name string to check.</param>
        /// <returns><c>true</c> if the name is a duplicate; otherwise, <c>false</c>.</returns>
        public bool IsNameDuplicate(string name)
        {
            return Items.Any(i => i.Name == name);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. Default is the calling member name.</param>
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// A simple implementation of <see cref="ICommand"/> that relays its execution to an action delegate.
    /// </summary>
    /// <param name="execute">The action to execute when the command is called.</param>
    public class RelayCommand(Action<object?> execute) : ICommand
    {
        private readonly Action<object?> _execute = execute;

        /// <summary>
        /// This event is intentionally stubbed out as the command is always executable.
        /// </summary>
        public event EventHandler? CanExecuteChanged { add { } remove { } }

        /// <summary>
        /// Determines whether the command can execute. Always returns <c>true</c>.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns><c>true</c> if this command can be executed; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object? parameter) => true;

        /// <summary>
        /// Executes the command's action delegate.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object? parameter) => _execute(parameter);
    }

    /// <summary>
    /// A dummy marker class used in a <see cref="CompositeCollection"/> to represent the "Add New Item" UI element.
    /// </summary>
    public class AddNewMarker { }
}