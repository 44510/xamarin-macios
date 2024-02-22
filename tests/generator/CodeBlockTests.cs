using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace bgen_tests {
	public class CodeBlockTests {
		string PerformWriting (ICodeBlock codeBlock)
		{
			MemoryStream memoryStream = new ();
			StreamWriter writer = new StreamWriter (memoryStream);
			codeBlock.Print (writer);
			writer.Flush ();
			memoryStream.Position = 0;
			using StreamReader reader = new StreamReader (memoryStream);
			return reader.ReadToEnd ();
		}

		[Test]
		public void LineBlockTest ()
		{
			string inputText1 = "using System;";
			string inputText2 = "using Network;";
			string expectedText = "{\n    using System;\n    using Network;\n}\n";
			CodeBlock codeBlock = new ();
			codeBlock.AddLine (inputText1);
			codeBlock.AddLine (inputText2);
			string output = PerformWriting (codeBlock);
			Assert.AreEqual (expectedText, output);
		}

		[Test]
		public void MethodBlockAddLinesTest ()
		{
			string inputText1 = "int fooCount = 1;";
			string inputText2 = "string[] fooNames = new [] {\"foo\"};";
			string methodName = "public void Foobinate";
			string [] methodArguments = new [] { "int count", "bool isFoo" };
			string expectedText =
				"public void Foobinate(int count, bool isFoo)\n{\n    int fooCount = 1;\n    string[] fooNames = new [] {\"foo\"};\n}\n";
			MethodBlock methodBlock = new (methodName, methodArguments);
			methodBlock.AddLine (inputText1);
			methodBlock.AddLine (inputText2);
			string output = PerformWriting (methodBlock);
			Assert.AreEqual (expectedText, output);
		}

		[Test]
		public void MethodBlockTest ()
		{
			string methodName = "public void Foobinate";
			string [] methodArguments = new [] { "int count", "bool isFoo" };
			string expectedText =
				"public void Foobinate(int count, bool isFoo)\n{\n    int fooCount = 1;\n    string[] fooNames = new [] {\"foo\"};\n}\n";
			List<ICodeBlock> blocks = new List<ICodeBlock> () { new LineBlock ("int fooCount = 1;"), new LineBlock ("string[] fooNames = new [] {\"foo\"};") };
			MethodBlock methodBlock = new (methodName, methodArguments, blocks);
			string output = PerformWriting (methodBlock);
			Assert.AreEqual (expectedText, output);
		}

		[Test]
		public void CodeBlockAsClassTest ()
		{
			string headerText = "public class FooClass";
			string variableText = "public string FooText {get;set;}";
			string expectedText = "public class FooClass\n{\n    public string FooText {get;set;}\n}\n";
			CodeBlock codeBlock = new (headerText);
			codeBlock.AddLine (variableText);
			string output = PerformWriting (codeBlock);
			Assert.AreEqual (expectedText, output);
		}

		[Test]
		public void IfBlockTest ()
		{
			string ifConditionText = "fooCount == 5";
			string line1 = "Console.WriteLine(fooCount);";
			string expectedText = "if (fooCount == 5)\n{\n    Console.WriteLine(fooCount);\n}\n";
			List<ICodeBlock> blocks = new ();
			blocks.Add (new LineBlock (line1));
			IfBlock ifBlock = new (ifConditionText, blocks);
			string output = PerformWriting (ifBlock);
			Assert.AreEqual (expectedText, output);
		}

		[Test]
		public void IfElseBlockTest ()
		{
			string ifConditionText = "fooCount == 5";
			string line1 = "Console.WriteLine(fooCount);";
			string line2 = "Console.WriteLine(booCount);";
			string expectedText = "if (fooCount == 5)\n{\n    Console.WriteLine(fooCount);\n}\nelse\n{\n    Console.WriteLine(booCount);\n}\n";
			List<ICodeBlock> blocks = new ();
			List<ICodeBlock> elseBlocks = new List<ICodeBlock> () { new LineBlock (line2) };
			blocks.Add (new LineBlock (line1));
			IfBlock ifBlock = new (ifConditionText, blocks);
			ifBlock.AddElse (elseBlocks);
			string output = PerformWriting (ifBlock);
			Assert.AreEqual (expectedText, output);
		}

		[Test]
		public void IfElseIfElseBlockTest ()
		{
			string ifConditionText = "fooCount == 5";
			string elseIfConditionText = "fooCount == 10";
			string line1 = "Console.WriteLine(fooCount);";
			string line2 = "Console.WriteLine(booCount);";
			string line3 = "Console.WriteLine(zooCount);";
			string expectedText = "if (fooCount == 5)\n{\n    Console.WriteLine(fooCount);\n}\nelse if (fooCount == 10)\n{\n    Console.WriteLine(booCount);\n}\nelse\n{\n    Console.WriteLine(zooCount);\n}\n";
			List<ICodeBlock> blocks = new ();
			List<ICodeBlock> elseIfBlocks = new List<ICodeBlock> () { new LineBlock (line2) };
			List<ICodeBlock> elseBlocks = new List<ICodeBlock> () { new LineBlock (line3) };
			blocks.Add (new LineBlock (line1));
			IfBlock ifBlock = new (ifConditionText, blocks);
			ifBlock.AddElseIf (elseIfConditionText, elseIfBlocks);
			ifBlock.AddElse (elseBlocks);
			string output = PerformWriting (ifBlock);
			Assert.AreEqual (expectedText, output);
		}

		[Test]
		public void ComprehensiveTest ()
		{
			string usingText = "using Network;";
			string namespaceText = "public namespace FooSpace";
			string headerText = "public class FooClass";
			string variableText = "public string FooText {get;set;}";
			string methodName = "public void Foobinate";
			string [] methodArguments = new [] { "int count", "bool isFoo" };
			string expectedText = "using Network;\n\npublic namespace FooSpace\n{\n    " +
								  "public class FooClass\n    {\n        public string FooText {get;set;}\n        " +
								  "public void Foobinate(int count, bool isFoo)\n        {\n        }\n    }\n}\n";

			BlockContainer blockContainer = new ();
			blockContainer.AddLine (usingText);
			blockContainer.AddLine ("");

			CodeBlock namespaceBlock = new CodeBlock (namespaceText);
			CodeBlock classBlock = new CodeBlock (headerText);
			MethodBlock methodBlock = new MethodBlock (methodName, methodArguments);
			classBlock.AddLine (variableText);
			classBlock.AddBlock (methodBlock);
			namespaceBlock.AddBlock (classBlock);
			blockContainer.AddBlock (namespaceBlock);

			string output = PerformWriting (blockContainer);
			Assert.AreEqual (expectedText, output);
		}
	}
}
