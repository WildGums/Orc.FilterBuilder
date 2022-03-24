namespace Orc.FilterBuilder.Automation
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Windows;
    using Orc.Automation;

    public class SerializableFilterSchemeEditInfo
    {
        public IEnumerable RawCollection { get; set; }
        public FilterScheme FilterScheme { get; set; }
        public bool AllowLivePreview { get; set; }
        public bool EnableAutoCompletion { get; set; }
    }

    public class FilterSchemeEditInfoSerializationConverter : SerializationValueConverterBase<FilterSchemeEditInfo, SerializableFilterSchemeEditInfo>
    {
        public override object ConvertFrom(FilterSchemeEditInfo value)
        {
            var serializableFilterSchemeEditInfo = new SerializableFilterSchemeEditInfo();
            serializableFilterSchemeEditInfo.RawCollection = value.RawCollection;
            serializableFilterSchemeEditInfo.FilterScheme = value.FilterScheme;
            serializableFilterSchemeEditInfo.AllowLivePreview = value.AllowLivePreview;
            serializableFilterSchemeEditInfo.EnableAutoCompletion = value.EnableAutoCompletion;

            return serializableFilterSchemeEditInfo;
        }

        public override object ConvertTo(SerializableFilterSchemeEditInfo value)
        {
            var filterSchemeEditInfo = new FilterSchemeEditInfo(value.FilterScheme, value.RawCollection, true, true);

            return filterSchemeEditInfo;
        }
    }
}
