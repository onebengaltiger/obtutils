#region Using directives

using System;
using System.Reflection;
using System.Runtime.InteropServices;

#endregion

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("OBTMultSQLCodeGen: Program to generate SQL code for multiple " +
                         "known database providers")]
[assembly: AssemblyDescription("Program to generate SQL code for multiple " +
                               "known database providers")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(OBTUtils.OBTApplicationInformation.appCompany)]
[assembly: AssemblyProduct("OBTMultSQLCodeGen")]
[assembly: AssemblyCopyright(OBTUtils.OBTApplicationInformation.appAuthorCopyright)]
[assembly: AssemblyTrademark(OBTUtils.OBTApplicationInformation.appCompany)]
[assembly: AssemblyCulture("")]

// This sets the default COM visibility of types in the assembly to invisible.
// If you need to expose a type to COM, use [ComVisible(true)] on that type.
[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

// The assembly version has following format :
//
// Major.Minor.Build.Revision
//
// You can specify all the values or you can use the default the Revision and
// Build Numbers by using the '*' as shown below:
[assembly: AssemblyVersion(OBTUtils.OBTApplicationInformation.appVersion)]