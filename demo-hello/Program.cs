using System;

class Program
{
	static void Main(string[] args)
	{
		Console.Write("あなたの名前は？: ");
		string name = Console.ReadLine() ?? "";
		Console.WriteLine($"こんにちは、{name}さん！");
	}
}