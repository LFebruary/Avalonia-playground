using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Playground.ViewModels;
using System;

namespace Playground.Views
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
