class LineBlock : ICodeBlock
{
	readonly string line;
	int currentIndent = 0;
	readonly string newLine = "\n";

	public LineBlock(int currentIndent, string line)
	{
		this.line = line;
		this.currentIndent = currentIndent;
	}

	public void SetIndent(int indent)
	{
		currentIndent = indent;
	}

	public string Print()
	{
		return new string(' ', currentIndent) + line + newLine;
	}
}
