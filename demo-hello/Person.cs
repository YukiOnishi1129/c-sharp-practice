using System;

class Person(string name)
{
	public string Name { get; set; } = name;

	public void Introduce()
	{
		Console.WriteLine($"My name is {Name}.");
	}
}