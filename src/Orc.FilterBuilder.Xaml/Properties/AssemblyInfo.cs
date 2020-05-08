// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="WildGums">
//   Copyright (c) 2008 - 2018 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Markup;

// All other assembly info is defined in SolutionAssemblyInfo.cs

[assembly: AssemblyTitle("Orc.FilterBuilder.Xaml")]
[assembly: AssemblyProduct("Orc.FilterBuilder.Xaml")]
[assembly: AssemblyDescription("Orc.FilterBuilder.Xaml library")]
[assembly: NeutralResourcesLanguage("en-US")]

[assembly: XmlnsPrefix("http://schemas.wildgums.com/orc/filterbuilder", "orcfilterbuilder")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder.Behaviors")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder.Converters")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder.Markup")]
[assembly: XmlnsDefinition("http://schemas.wildgums.com/orc/filterbuilder", "Orc.FilterBuilder.Views")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
    //(used if a resource is not found in the page, 
    // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
    //(used if a resource is not found in the page, 
    // app, or any theme specific resource dictionaries)
)]
