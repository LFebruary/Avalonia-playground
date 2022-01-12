using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;

namespace Playground.SerialReadSocketSend
{
    public partial class SerialWindow : BaseWindow<SerialWindowVM>
    {
        public SerialWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            DataContext = new SerialWindowVM(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this); 
        }

        internal T FindControlNullSafe<T>(string name) where T : class, IControl, new()
        {
            Contract.Requires<ArgumentNullException>(this != null);
            Contract.Requires<ArgumentNullException>(name != null);

            T? foundControl = this.FindControl<T>(name);
            return foundControl ?? new T();
        }

        #region Focus methods
        internal void FocusBaudRatePicker()             => FindControlNullSafe<ComboBox>("baudRateCombobox").IsDropDownOpen = true;
        internal void FocusDatabitsPicker()             => FindControlNullSafe<ComboBox>("databitsCombobox").IsDropDownOpen = true;
        internal void FocusNumberOfReadings()           => FindControlNullSafe<NumericUpDown>("sequentialReadingsSpinner").Focus();
        internal void FocusPortPicker()                 => FindControlNullSafe<ComboBox>("serialPortCombobox").IsDropDownOpen = true;
        internal void FocusRequiredLength()             => FindControlNullSafe<NumericUpDown>("requiredLengthSpinner").Focus();
        internal void FocusScaleStringEndPosition()     => FindControlNullSafe<NumericUpDown>("weightEndSpinner").Focus();
        internal void FocusScaleStringStartPosition()   => FindControlNullSafe<NumericUpDown>("weightStartSpinner").Focus();
        internal void FocusStabilityIndicatorSnippet()  => FindControlNullSafe<TextBox>("stabilityIndicatorTextbox").Focus();
        internal void FocusStartingPosition()           => FindControlNullSafe<NumericUpDown>("stabilityIndicatorStartSpinner").Focus();
        internal void FocusStopBitsPicker()             => FindControlNullSafe<ComboBox>("stopBitsCombobox").IsDropDownOpen = true;
        #endregion

    }
}
