using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

using Mono.Cecil;

using Xamarin.Tests;
using Xamarin.Utils;

#nullable enable

namespace Cecil.Tests {

	public static class Helper {

		static Dictionary<string, AssemblyDefinition> cache = new Dictionary<string, AssemblyDefinition> ();

		// make sure we load assemblies only once into memory
		public static AssemblyDefinition GetAssembly (string assembly, ReaderParameters? parameters = null, bool readSymbols = false)
		{
			Assert.That (assembly, Does.Exist, "Assembly existence");
			if (!cache.TryGetValue (assembly, out var ad)) {
				if (parameters == null) {
					var resolver = new DefaultAssemblyResolver ();
					resolver.AddSearchDirectory (GetBCLDirectory (assembly));
					parameters = new ReaderParameters () {
						AssemblyResolver = resolver,
						ReadSymbols = readSymbols,
					};
				}

				ad = AssemblyDefinition.ReadAssembly (assembly, parameters);
				cache.Add (assembly, ad);
			}
			return ad;
		}

		// Enumerates all the methods in the assembly, for all types (including nested types), potentially providing a custom filter function.
		public static IEnumerable<MethodDefinition> EnumerateMethods (this AssemblyDefinition assembly, Func<MethodDefinition, bool>? filter = null)
		{
			foreach (var type in EnumerateTypes (assembly)) {
				if (!type.HasMethods)
					continue;

				foreach (var method in type.Methods) {
					if (filter is null || filter (method))
						yield return method;
				}
			}
		}

		// Enumerates all the properties in the assembly, for all types (including nested types), potentially providing a custom filter function.
		public static IEnumerable<PropertyDefinition> EnumerateProperties (this AssemblyDefinition assembly, Func<PropertyDefinition, bool>? filter = null)
		{
			foreach (var type in EnumerateTypes (assembly)) {
				if (!type.HasProperties)
					continue;

				foreach (var property in type.Properties) {
					if (filter is null || filter (property))
						yield return property;
				}
			}
		}

		// Enumerates all the events in the assembly, for all types (including nested types), potentially providing a custom filter function.
		public static IEnumerable<EventDefinition> EnumerateEvents (this AssemblyDefinition assembly, Func<EventDefinition, bool>? filter = null)
		{
			foreach (var type in EnumerateTypes (assembly)) {
				if (!type.HasEvents)
					continue;

				foreach (var @event in type.Events) {
					if (filter is null || filter (@event))
						yield return @event;
				}
			}
		}

		// Recursively enumerates all the nested types for the given type, potentially providing a custom filter function.
		static IEnumerable<TypeDefinition> EnumerateNestedTypes (TypeDefinition type, Func<TypeDefinition, bool>? filter)
		{
			if (!type.HasNestedTypes)
				yield break;

			foreach (var nestedType in type.NestedTypes) {
				foreach (var nn in EnumerateNestedTypes (nestedType, filter))
					yield return nn;

				if (filter is null || filter (nestedType))
					yield return nestedType;
			}
		}

		// Enumerates all the types in the assembly, including nested types, potentially providing a custom filter function.
		public static IEnumerable<TypeDefinition> EnumerateTypes (this AssemblyDefinition assembly, Func<TypeDefinition, bool>? filter = null)
		{
			foreach (var module in assembly.Modules) {
				if (!module.HasTypes)
					continue;

				foreach (var type in module.Types) {
					if (filter is null || filter (type))
						yield return type;

					foreach (var nestedType in EnumerateNestedTypes (type, filter))
						yield return nestedType;
				}
			}
		}

		// Enumerates all the fields in the assembly, for all types (including nested types), potentially providing a custom filter function.
		public static IEnumerable<FieldDefinition> EnumerateFields (this AssemblyDefinition assembly, Func<FieldDefinition, bool>? filter = null)
		{
			foreach (var type in EnumerateTypes (assembly)) {
				if (!type.HasFields)
					continue;

				foreach (var field in type.Fields) {
					if (filter is null || filter (field))
						yield return field;
				}
			}
		}

		public static string GetBCLDirectory (string assembly)
		{
			var rv = string.Empty;
			var isDotNet = !assembly.Contains ("Library/Frameworks/Xamarin.iOS.framework") && !assembly.Contains ("Library/Frameworks/Xamarin.Mac.framework");

			switch (Configuration.GetPlatform (assembly, isDotNet)) {
			case ApplePlatform.iOS:
				rv = Path.GetDirectoryName (Configuration.XamarinIOSDll);
				break;
			case ApplePlatform.WatchOS:
				rv = Path.GetDirectoryName (Configuration.XamarinWatchOSDll);
				break;
			case ApplePlatform.TVOS:
				rv = Path.GetDirectoryName (Configuration.XamarinTVOSDll);
				break;
			case ApplePlatform.MacOSX:
				rv = Path.GetDirectoryName (assembly);
				break;
			case ApplePlatform.MacCatalyst:
				rv = Path.GetDirectoryName (Configuration.XamarinCatalystDll);
				break;
			default:
				throw new NotImplementedException (assembly);
			}

			return rv;
		}

		static IEnumerable<string> PlatformAssemblies {
			get {
				if (!Configuration.include_legacy_xamarin)
					yield break;

				if (Configuration.include_ios) {
					// we want to process 32/64 bits individually since their content can differ
					if (Configuration.iOSSupports32BitArchitectures)
						yield return Path.Combine (Configuration.MonoTouchRootDirectory, "lib", "32bits", "iOS", "Xamarin.iOS.dll");
					yield return Path.Combine (Configuration.MonoTouchRootDirectory, "lib", "64bits", "iOS", "Xamarin.iOS.dll");
				}

				if (Configuration.include_watchos) {
					// XamarinWatchOSDll is stripped of its IL
					yield return Path.Combine (Configuration.MonoTouchRootDirectory, "lib", "32bits", "watchOS", "Xamarin.WatchOS.dll");
				}

				if (Configuration.include_tvos) {
					// XamarinTVOSDll is stripped of it's IL
					yield return Path.Combine (Configuration.MonoTouchRootDirectory, "lib", "64bits", "tvOS", "Xamarin.TVOS.dll");
				}

				if (Configuration.include_mac) {
					yield return Configuration.XamarinMacMobileDll;
					yield return Configuration.XamarinMacFullDll;
				}
			}
		}

		static IList<AssemblyInfo>? platform_assembly_definitions;
		public static IEnumerable<AssemblyInfo> PlatformAssemblyDefinitions {
			get {
				if (platform_assembly_definitions is null) {
					platform_assembly_definitions = PlatformAssemblies
						.Select (v => new AssemblyInfo (v, GetAssembly (v, readSymbols: true), false))
						.ToArray ();
				}
				return platform_assembly_definitions;
			}
		}


		static IEnumerable<string> NetPlatformAssemblies => Configuration.GetRefLibraries ();

		static IList<AssemblyInfo>? net_platform_assembly_definitions;
		public static IEnumerable<AssemblyInfo> NetPlatformAssemblyDefinitions {
			get {
				if (net_platform_assembly_definitions is null) {
					net_platform_assembly_definitions = NetPlatformAssemblies
						.Select (v => new AssemblyInfo (v, GetAssembly (v, readSymbols: false), true))
						.ToArray ();
				}
				return net_platform_assembly_definitions;
			}
		}

		static IEnumerable<string> NetPlatformImplementationAssemblies => Configuration.GetBaseLibraryImplementations ();

		static IList<AssemblyInfo>? net_platform_assembly_implemnetation_assembly_definitions;
		public static IEnumerable<AssemblyInfo> NetPlatformImplementationAssemblyDefinitions {
			get {
				if (net_platform_assembly_implemnetation_assembly_definitions is null) {
					net_platform_assembly_implemnetation_assembly_definitions = NetPlatformImplementationAssemblies
						.Select (v => new AssemblyInfo (v, GetAssembly (v, readSymbols: true), true))
						.ToArray ();
				}
				return net_platform_assembly_implemnetation_assembly_definitions;
			}
		}

		public static IEnumerable<TestFixtureData> TaskAssemblies {
			get {
				if (Configuration.include_ios)
					yield return CreateTestFixtureDataFromPath (Path.Combine (Configuration.SdkRootXI, "lib", "msbuild", "iOS", "Xamarin.iOS.Tasks.dll"));
				if (Configuration.include_mac)
					yield return CreateTestFixtureDataFromPath (Path.Combine (Configuration.SdkRootXM, "lib", "msbuild", "Xamarin.Mac.Tasks.dll"));
			}
		}

		static TestFixtureData CreateTestFixtureDataFromPath (string path)
		{
			var rv = new TestFixtureData (path);
			rv.SetArgDisplayNames (Path.GetFileName (path));
			return rv;
		}
	}

	public static class CompatExtensions {
		// cecil-tests is not NET5 yet, this is required to foreach over a dictionary
		public static void Deconstruct<T1, T2> (this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
		{
			key = tuple.Key;
			value = tuple.Value;
		}
	}

	public class AssemblyInfo {
		public AssemblyDefinition Assembly;
		public string Path;
		public ApplePlatform Platform;

		public AssemblyInfo (string path, AssemblyDefinition assembly, bool isDotNet)
		{
			Assembly = assembly;
			Path = path;
			Platform = Configuration.GetPlatform (path, isDotNet);
		}

		public override string ToString ()
		{
			// The returned text will show up in VSMac's unit test pad
			return Path.Replace (Configuration.RootPath, string.Empty).TrimStart ('/');
		}
	}
}
