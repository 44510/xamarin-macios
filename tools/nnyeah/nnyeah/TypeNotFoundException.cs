using System;
namespace Microsoft.MaciOS.Nnyeah {
	public class TypeNotFoundException : Exception {
		public TypeNotFoundException (string typeName)
			: base ($"The type {typeName} was not found.")
		{
			TypeName = typeName;
		}

		public string TypeName { get; init; }
	}
}
