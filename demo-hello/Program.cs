using System;

class Program
{
	static void Main(string[] args)
	{
		Person p1 = new("Alice");
		p1.Introduce();
		Person p2 = new("Bob");
		p2.Introduce();
	}
}