using System.ComponentModel;
using System.Runtime.CompilerServices;
using ItemViewerApp.Model;

namespace ItemViewerApp.ViewModel
{
    /// <summary>
    /// ViewModel for a single <see cref="MyObject"/> model, handling UI-specific logic,
    /// validation, and edit state.
    /// </summary>
    /// <param name="model">The underlying data model.</param>
    /// <param name="mainVm">The parent <see cref="MainViewModel"/> for coordination.</param>
    public class MyObjectViewModel(MyObject model, MainViewModel mainVm) : INotifyPropertyChanged
    {
        private readonly MyObject _model = model;
        private bool _isEditing;
        private string _originalName = model.Name;
        private int _originalCount = model.ItemsCount;
        private readonly MainViewModel _mainViewModel = mainVm;

        /// <summary>
        /// Gets or sets the name of the item. This property notifies changes and triggers validation.
        /// </summary>
        public string Name
        {
            get => _model.Name;
            set
            {
                if (_model.Name != value)
                {
                    _model.Name = value;
                    OnPropertyChanged();
                    ValidateName();
                }
            }
        }

        /// <summary>
        /// Gets or sets the count of the item. This property notifies changes and updates the total count in the main ViewModel.
        /// </summary>
        public int ItemsCount
        {
            get => _model.ItemsCount;
            set
            {
                if (_model.ItemsCount != value)
                {
                    _model.ItemsCount = value;
                    OnPropertyChanged();
                    _mainViewModel.UpdateTotal();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is currently in an editing state in the UI.
        /// Setting this to <c>false</c> triggers the <see cref="FinalizeEdit"/> logic.
        /// </summary>
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                if (_isEditing != value)
                {
                    _isEditing = value;
                    OnPropertyChanged();

                    if (!_isEditing)
                    {
                        FinalizeEdit();
                    }
                    else
                    {
                        _originalName = Name;
                        _originalCount = ItemsCount;
                    }
                }
            }
        }

        private string? _nameError;

        /// <summary>
        /// Gets or sets the validation error message for the <see cref="Name"/> property.
        /// </summary>
        public string? NameError
        {
            get => _nameError;
            set { _nameError = value; OnPropertyChanged(); }
        }

        private void ValidateName()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                NameError = "Name cannot be empty.";
            }
            else if (_mainViewModel.IsNameDuplicate(this))
            {
                NameError = "Name must be unique.";
            }
            else
            {
                NameError = null;
            }
        }

        private void FinalizeEdit()
        {
            ValidateName();

            if (NameError != null)
            {
                Name = _originalName;
                ItemsCount = _originalCount;
                IsEditing = false;
            }
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
}