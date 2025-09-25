using System;

class Program
{
	static void Main(string[] args)
	{
		Console.Write("1つ目の数を入力");
		double a = double.Parse(Console.ReadLine());

		Console.Write("2つ目の数を入力");
		double b = double.Parse(Console.ReadLine());

		Console.Write("演算子を入力(+,-,*,/)");
		string op = Console.ReadLine();

		double result = 0;

		if (op == "+")
		{
			result = a + b;
		}
		else if (op == "-")
		{
			result = a - b;
		}
		else if (op == "*")
		{
			result = a * b;
		}
		else if (op == "/")
		{
			result = a / b;
		}
		else
		{
			Console.WriteLine("不正な演算子です");
			return;
		}
		Console.WriteLine($"結果は {result} です");
	}
}