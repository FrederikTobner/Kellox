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
for (var i = 0; i < 60; i = i + 1) 
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
for (var j = 0; j < 10; j = j + 1) 
{
	print fibo();
}
```
Count down from 10 to zero using recursion
```
fun Countdown(x)
{
	if(x > -1)
	{
		print x;
		Countdown(x = x - 1);
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
