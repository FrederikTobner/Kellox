# Example programs
Hello World:
```
print "Hello World";
```
Greet the user after typing in the name:
```
print "Please type in your name:";
var name = read();
print "Hello " + name;
```
Prints the current time:
```
for (var i = 0; i < 60; i++) 
{
	print clock();
	wait(1);
	clear();
}
```
Creates a counter:
```
fun makeCounter() 
{
  	var i = 0;
  	fun count() 
	{
    		return i = i + 1;
  	}
  	return count;
}

var counter = makeCounter();
print counter(); // "1".
print counter(); // "2"
```
Print the first 10 Fibonacci Numbers:
```
fun fibonacci()
{
	var x = 0;
	var temp = 0;
	var b = 1;
	fun number()
	{
		temp = x;
		x = b;
		b = temp + x;
		return temp;
	}
	return number;
}

var fibo = fibonacci();
for (var j = 0; j < 10; j++) 
{
	print fibo();
}
```
Count down from 10 to zero using recursion
```
fun Countdown(x)
{
	if(x >= 0)
	{
		print x;
		wait(1);
		Countdown(x--);
	}
}
Countdown(10);
```
Class representing a Rectangle:
```
class Rectangle
{
	init(x, y)
	{
		this.x = x;
		this.y = y;
	}
	area()
	{
		return this.x * this.y * 0.5;
	}
}  
```
Tasty oop breakfast
```
class Doughnut 
{
	init(fryingColor)
	{
		this.fryingColor = fryingColor;
	}
	cook() 
	{
		println "Fry until " + this.fryingColor;
	}
}

class FilledDougnut < Doughnut 
{
	init(filling, fryingColor)
	{
		super.init(fryingColor);
		this.filling = filling;		
	}
	cook()
	{
		super.cook();
		println "With " + this.filling;
	}
}

class FilledDougnutWithChocolateCoat < FilledDougnut
{
	init(chocolateColor, filling, fryingColor)
	{
		super.init(filling, fryingColor);
		this.chocolateColor = chocolateColor;		
	}
	cook()
	{
		super.cook();
		println "And a " + this.chocolateColor + " chocolate coat";
	}
}

FilledDougnutWithChocolateCoat("dark", "custard", "golden brown").cook();
```
