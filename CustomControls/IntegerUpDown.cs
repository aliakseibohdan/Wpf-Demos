using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CustomControls
{
    public class IntegerUpDown : Control
    {
        static IntegerUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerUpDown), new FrameworkPropertyMetadata(typeof(IntegerUpDown)));
        }

        #region Dependency Properties

        public static readonly DependencyProperty CurrentValueProperty =
            DependencyProperty.Register(nameof(CurrentValue), typeof(int), typeof(IntegerUpDown),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnCurrentValueChanged, CoerceCurrentValue));

        public static readonly DependencyProperty MinimumValueProperty =
            DependencyProperty.Register(nameof(MinimumValue), typeof(int), typeof(IntegerUpDown),
                new FrameworkPropertyMetadata(int.MinValue, OnMinMaxChanged));

        public static readonly DependencyProperty MaximumValueProperty =
            DependencyProperty.Register(nameof(MaximumValue), typeof(int), typeof(IntegerUpDown),
                new FrameworkPropertyMetadata(int.MaxValue, OnMinMaxChanged));

        public int CurrentValue
        {
            get => (int)GetValue(CurrentValueProperty);
            set => SetValue(CurrentValueProperty, value);
        }

        public int MinimumValue
        {
            get => (int)GetValue(MinimumValueProperty);
            set => SetValue(MinimumValueProperty, value);
        }

        public int MaximumValue
        {
            get => (int)GetValue(MaximumValueProperty);
            set => SetValue(MaximumValueProperty, value);
        }

        #endregion

        #region Property Changed Callbacks and Coercion

        private static void OnCurrentValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (IntegerUpDown)d;

            if (control._inputTextBox != null && e.NewValue is int newValue)
            {
                var newValueString = newValue.ToString(CultureInfo.InvariantCulture);

                if (!control._inputTextBox.Text.Equals(newValueString, StringComparison.Ordinal))
                {
                    control._inputTextBox.TextChanged -= control.TextBox_TextChanged;
                    control._inputTextBox.Text = newValueString;
                    control._inputTextBox.TextChanged += control.TextBox_TextChanged;
                }
            }
        }

        private static object CoerceCurrentValue(DependencyObject d, object baseValue)
        {
            var control = (IntegerUpDown)d;
            var newValue = (int)baseValue;

            if (newValue < control.MinimumValue)
            {
                return control.MinimumValue;
            }
            if (newValue > control.MaximumValue)
            {
                return control.MaximumValue;
            }

            return newValue;
        }

        private static void OnMinMaxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (IntegerUpDown)d;
            control.CoerceValue(CurrentValueProperty);
        }

        #endregion

        #region Input Handling and Logic

        private TextBox _inputTextBox = new();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _inputTextBox = (TextBox)GetTemplateChild("PART_InputTextBox");

            if (GetTemplateChild("PART_IncrementButton") is RepeatButton incrementButton)
            {
                incrementButton.Click += (s, e) => Increment();
            }
            if (GetTemplateChild("PART_DecrementButton") is RepeatButton decrementButton)
            {
                decrementButton.Click += (s, e) => Decrement();
            }

            if (_inputTextBox != null)
            {
                _inputTextBox.LostFocus += TextBox_LostFocus;
                _inputTextBox.TextChanged += TextBox_TextChanged;
                _inputTextBox.PreviewKeyDown += TextBox_PreviewKeyDown;
                _inputTextBox.Text = CurrentValue.ToString(CultureInfo.InvariantCulture);
            }
        }

        private void Increment()
        {
            if (CurrentValue < MaximumValue)
            {
                CurrentValue++;
            }
        }

        private void Decrement()
        {
            if (CurrentValue > MinimumValue)
            {
                CurrentValue--;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var isDigit = char.IsDigit(e.Text, e.Text.Length - 1);
            var isNegativeSign = e.Text == "-" && _inputTextBox.CaretIndex == 0 && !_inputTextBox.Text.Contains('-') && MinimumValue < 0;

            e.Handled = !(isDigit || isNegativeSign);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }

            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                UpdateCurrentValueFromTextBox();
            }

            if (e.Key == Key.Up)
            {
                Increment();
                e.Handled = true;
            }
            if (e.Key == Key.Down)
            {
                Decrement();
                e.Handled = true;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateCurrentValueFromTextBox();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!Equals(_inputTextBox.Text, CurrentValue.ToString(CultureInfo.InvariantCulture)))
            {
                if (string.IsNullOrWhiteSpace(_inputTextBox.Text))
                {
                    CurrentValue = MinimumValue;
                }
                else
                {
                    if (int.TryParse(_inputTextBox.Text, out int result))
                    {
                        CurrentValue = Math.Max(MinimumValue, Math.Min(MaximumValue, result));
                    }
                }
            }
        }

        private void UpdateCurrentValueFromTextBox()
        {
            if (_inputTextBox == null) return;

            if (int.TryParse(_inputTextBox.Text, out int result))
            {
                int coercedValue = Math.Max(MinimumValue, Math.Min(MaximumValue, result));
                CurrentValue = coercedValue;
                _inputTextBox.Text = CurrentValue.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                _inputTextBox.Text = CurrentValue.ToString(CultureInfo.InvariantCulture);
            }

            _inputTextBox.CaretIndex = _inputTextBox.Text.Length;
        }

        #endregion
    }
}