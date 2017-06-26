using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCompany("Fabrício Godoy")]
[assembly: AssemblyCopyright("© Fabrício Godoy. All rights reserved.")]
[assembly: AssemblyProduct("System.Threading.Tasks")]
[assembly: AssemblyDescription("Provides types that simplify the work of writing concurrent and asynchronous code")]
[assembly: CLSCompliant(true)]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyInformationalVersion("1.2.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Retail")]
#endif

[assembly: InternalsVisibleTo("Tasks.Tests, PublicKey=" + Consts.SkarllotPublicKey)]
