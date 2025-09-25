using System;

class Program
{
	static void Main(string[] args)
	{
		Random rand = new();
		int answer = rand.Next(1, 11);

		Console.WriteLine("1から10までの数字を当ててください。");

		while (true)
		{
			Console.Write("あなたの予想は？：");
			string? input = Console.ReadLine();
			if (input == null)
			{
				Console.WriteLine("入力がありません。もう一度入力してください。");
				continue;
			}
			int guess = int.Parse(input);

			if (guess == answer)
			{
				Console.WriteLine("正解です！");
				break;
			}
			else if (guess < answer)
			{
				Console.WriteLine("もっと大きい数字です。");
			}
			else
			{
				Console.WriteLine("もっと小さい数字です。");
			}
		}
	}
}