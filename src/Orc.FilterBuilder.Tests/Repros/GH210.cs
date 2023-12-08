namespace Orc.FilterBuilder.Tests.Repros;

using System;
using System.IO;
using System.Threading.Tasks;
using Catel.Data;
using Catel.IoC;
using Catel.Runtime.Serialization.Xml;
using NUnit.Framework;
using Orc.FileSystem;

// See https://github.com/WildGums/Orc.FilterBuilder/issues/210

[TestFixture]
public class GH210
{
    [Test]
    [Category("Basic Save")]
    public void OrcFilterBasicSaveScenario()
    {
        Type targetType = typeof(DataPointFilter);
        var filterscheme = new FilterScheme(targetType, "All Data");

        var fSchemes = new FilterSchemes();
        fSchemes.Schemes.Add(filterscheme);

        using var tempFileContext = new TemporaryFilesContext("filters");
        var tempFile = tempFileContext.GetFile($"GH210-basic-save.xml", true);

        fSchemes.SaveAsXml(tempFile);

        Assert.That(System.IO.File.Exists(tempFile), Is.True);
        Assert.That(new System.IO.FileInfo(tempFile).Length, Is.GreaterThan(0));
    }

    [Test]
    [Category("Save")]
    public async Task OrcFilterSaveScenarioAsync()
    {
        var fileService = new FileService();
        var filterSerializationService = new FilterSerializationService(fileService,
            ServiceLocator.Default.ResolveType<IXmlSerializer>());

        Type targetType = typeof(DataPointFilter);
        FilterScheme filterscheme = new FilterScheme(targetType, "All Data");

        PropertyExpression pe = new PropertyExpression();
        var iex = new EnumExpression<Phase>(false);
        var pManager = new InstanceProperties(targetType);

        var fSchemes = new FilterSchemes();
        fSchemes.Schemes.Add(filterscheme);

        iex.SelectedCondition = Condition.EqualTo;
        iex.Value = Phase.Isokinetic;
        pe.Property = pManager.GetProperty("Phase");
        pe.DataTypeExpression = iex;

        filterscheme.Root.Items.Add(pe);

        using var tempFileContext = new TemporaryFilesContext("filters");
        var tempFile = tempFileContext.GetFile($"GH210-save.xml", true);

        await filterSerializationService.SaveFiltersAsync(tempFile, fSchemes);

        Assert.That(System.IO.File.Exists(tempFile), Is.True);
        Assert.That(new System.IO.FileInfo(tempFile).Length, Is.GreaterThan(0));

        var clonedFilterSchemes = await filterSerializationService.LoadFiltersAsync(tempFile);

        var conditionGroup = clonedFilterSchemes.Schemes[0].ConditionItems[0] as ConditionGroup;
        Assert.That(conditionGroup, Is.Not.Null);
        Assert.That(conditionGroup.Items.Count, Is.EqualTo(1));

        var clonedPropertyExpression = conditionGroup.Items[0] as PropertyExpression;
        Assert.That(clonedPropertyExpression, Is.Not.Null);

        var clonedDataTypeExpression = clonedPropertyExpression.DataTypeExpression as EnumExpression<Phase>;
        Assert.That(clonedDataTypeExpression, Is.Not.Null);
        Assert.That(clonedDataTypeExpression.Value, Is.EqualTo(Phase.Isokinetic));
    }

    [Test]
    [Category("Serialize")]
    public async Task OrcFilterSerializeScenarioAsync()
    {
        var xmlSerializer = new XmlSerializer(ServiceLocator.Default.ResolveRequiredType<Catel.Runtime.Serialization.SerializationManager>(),
            ServiceLocator.Default.ResolveRequiredType<IDataContractSerializerFactory>(),
            ServiceLocator.Default.ResolveRequiredType<IXmlNamespaceManager>(),
            ServiceLocator.Default.ResolveRequiredType<ITypeFactory>(),
            ServiceLocator.Default.ResolveRequiredType<Catel.Runtime.Serialization.IObjectAdapter>());

        using (var memoryStream = new MemoryStream())
        {
            xmlSerializer.Serialize(new FilterScheme(), memoryStream);
            var xml = ReadAll(memoryStream);
        }

        var filterSchemes = new FilterSchemes();

        using var tempFileContext = new TemporaryFilesContext("filters");
        var tempFile = tempFileContext.GetFile($"GH210-serialize.xml", true);

        using (var fs = new FileStream(tempFile, FileMode.Create))
        {
            xmlSerializer.Serialize(filterSchemes, fs);
        }

        var fileService = new FileService();
        var fSchemes = new FilterSerializationService(fileService,
            ServiceLocator.Default.ResolveRequiredType<Catel.Runtime.Serialization.Xml.IXmlSerializer>());
        var filters = await fSchemes.LoadFiltersAsync(tempFile);
        var res = filters;
    }

    public string ReadAll(MemoryStream memStream)
    {
        var pos = memStream.Position;
        memStream.Position = 0;

        using var reader = new StreamReader(memStream);
        var str = reader.ReadToEnd();

        // Reset the position so that subsequent writes are correct.
        memStream.Position = pos;

        return str;
    }
}

public class DataPointFilter
{
    /// <summary>
    ///     ''' Index für Array-Zugriff und DB
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public int Id { get; set; }
    /// <summary>
    ///     ''' Bezeichnet die Zeilennr in der Rohdatei des Messpunktes 
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public int Row { get; set; }
    /// <summary>
    ///     ''' Zeitstempel des Datenpunkts in Sekunden
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public double Time { get; set; }
    /// <summary>
    ///     ''' Relative Position in Meter zum Nullpunkt
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public double Position { get; set; }
    /// <summary>
    ///     ''' Resultierendes Drehmoment in Nm bzw. resultierende Kraft in N
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public double Torque { get; set; }
    /// <summary>
    ///     ''' Geschwindigkeit in °/sek bzw. m/sek
    ///     ''' </summary>
    ///     ''' <remarks></remarks>
    public double Speed { get; set; }
    public double TorqueWithoutGravity { get; set; }
    // Public Property Repetition As Integer
    public int Set { get; set; }
    public double TorqueOnDyno { get; set; }
    public double ForceRight { get; set; }
    public double ForceLeft { get; set; }
    public Movement Movement { get; set; }
    public int RepPerSet { get; set; }
    public Side Side { get; set; }
    public Phase Phase { get; set; }
    public double Work { get; set; }
    public double Acceleration { get; set; }
    public double Power { get; set; }
    public byte RepChange { get; set; }
    public int RepetionRaw { get; set; }
    public Contraction Contraction { get; set; }

    public int Repetition { get; set; }
}


public enum Contraction
{
    concentric = 0,
    excentric
}

public enum TestMode
{
    kon_kon = 1,
    kon_exz,
    exz_kon,
    exz_exz,
    isometrisch = 6,
    aktiv_assistiv,
    stat_koord_Stabilisation = 12,
    athletic = 21
}

public enum Movement : byte
{
    B1 = 0,
    B2 = 1
}

[Flags]
public enum Side : byte
{
    Right = 1,
    Left,
    Both
}

public enum Phase : ushort
{
    Acceleration = 0,
    Isokinetic,
    Deceleration,
    Rest
}

public enum ContractionMode
{
    Undefiniert = -1,
    Statisch,
    Konz_Konz,
    Exz_Konz,
    Konz_Exz,
    Exz_Exz
}
