namespace Orc.FilterBuilder.Automation;

using System;
using System.Windows.Automation;
using Controls;
using Orc.Automation;
using Orc.Automation.Controls;
using DateTimePicker = Controls.Automation.DateTimePicker;
using NumericUpDown = Controls.Automation.NumericUpDown;
using TimeSpanPicker = Controls.Automation.TimeSpanPicker;

public class EditFilterPropertyValueEditorPart : AutomationBase
{
    public EditFilterPropertyValueEditorPart(AutomationElement element)
        : base(element)
    {
    }

    //Value editors
    public Edit? ValueEdit => By.Id("PART_ValueTextBox").One<Edit>();
    public NumericUpDown? ValueNumericUpDown => By.Id("PART_ValueNumericUpDown").One<NumericUpDown>();
    public DateTimePicker? ValueDateTimePicker => By.Id("PART_ValueDateTimePicker").One<DateTimePicker>();
    public ComboBox? BooleanValueComboBox => By.Id("PART_BooleanValueComboBox").One<ComboBox>();
    public ComboBox? EnumValueComboBox => By.Id("PART_EnumValueComboBox").One<ComboBox>();
    public TimeSpanPicker? ValueTimeSpanPicker => By.Id("PART_ValueTimeSpanPicker").One<TimeSpanPicker>();

    public object? Value
    {
        get => GetEditorValue();
        set => SetEditorValue(value);
    }

    private object? GetEditorValue()
    {
        var valueEdit = ValueEdit;
        if (valueEdit?.IsVisible() ?? false)
        {
            return valueEdit.Text;
        }

        var numericUpDown = ValueNumericUpDown;
        if (numericUpDown?.IsVisible() ?? false)
        {
            return numericUpDown.Value;
        }

        var dateTimePicker = ValueDateTimePicker;
        if (dateTimePicker?.IsVisible() ?? false)
        {
            return dateTimePicker.Value;
        }

        var booleanCombobox = BooleanValueComboBox;
        if (booleanCombobox?.IsVisible() ?? false)
        {
            return booleanCombobox.GetSelectedValue<bool>();
        }

        //TODO:won't work
        var enumCombobox = EnumValueComboBox;
        if (enumCombobox?.IsVisible() ?? false)
        {
            return enumCombobox.GetSelectedValue();
        }

        var timeSpanPicker = ValueTimeSpanPicker;
        if (timeSpanPicker?.IsVisible() ?? false)
        {
            return timeSpanPicker.Value;
        }

        return null;
    }

    private void SetEditorValue(object? value)
    {
        var valueEdit = ValueEdit;
        if (valueEdit is null)
        {
            return;
        }

        if (valueEdit.IsVisible())
        {
            valueEdit.Text = (string?)value ?? string.Empty;
            return;
        }

        var numericUpDown = ValueNumericUpDown;
        if (numericUpDown?.IsVisible() ?? false)
        {
            var numberValue = new Number(value!);
            numericUpDown.Value = numberValue;
            return;
        }

        var dateTimePicker = ValueDateTimePicker;
        if (dateTimePicker?.IsVisible() ?? false)
        {
            dateTimePicker.Value = (DateTime?)value;
            return;
        }

        var booleanCombobox = BooleanValueComboBox;
        if (booleanCombobox?.IsVisible() ?? false)
        {
            booleanCombobox.SelectValue(value);
            return;
        }

        var enumCombobox = EnumValueComboBox;
        if (enumCombobox?.IsVisible() ?? false)
        {
            enumCombobox.SelectValue(value);
            return;
        }

        var timeSpanPicker = ValueTimeSpanPicker;
        if (timeSpanPicker?.IsVisible() ?? false)
        {
            timeSpanPicker.Value = (TimeSpan?)value;
            return;
        }
    }
}
