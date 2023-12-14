﻿namespace Orc.FilterBuilder.Tests.Converters;

using Catel.Data;
using NUnit.Framework;
using Orc.FilterBuilder.Converters;

[TestFixture]
public class ObjectToValueConverterFacts
{
    private class TestModel : ModelBase
    {
        public string Reference { get => "test"; }

        public int MyIntegerValue { get; set; }
    }

    [TestCase]
    public void ConvertsGetterPropertiesFromCatelModelBase()
    {
        var model = new TestModel
        {
            MyIntegerValue = 42
        };

        var converter = new ObjectToValueConverter(null);

        Assert.That(converter.Convert(model, null, nameof(TestModel.MyIntegerValue), null), Is.EqualTo(42));
        Assert.That(converter.Convert(model, null, nameof(TestModel.Reference), null), Is.EqualTo("test"));
    }
}