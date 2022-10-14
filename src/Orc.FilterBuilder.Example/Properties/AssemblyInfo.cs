﻿// All other assembly info is defined in SharedAssembly.cs

using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

[assembly: AssemblyTitle("Orc.FilterBuilder.Example")]
[assembly: AssemblyProduct("Orc.FilterBuilder.Example")]
[assembly: AssemblyDescription("Orc.FilterBuilder.Example library")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

#if !PCL
[assembly: ComVisible(false)]
#endif

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page, 
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page, 
                                              // app, or any theme specific resource dictionaries)
    )]
