# Interpreter
Interpreter based on the book [Crafting interpreters](https://craftinginterpreters.com/contents.html)

## Example programs
Print all Fibonacci numbers that are less than 1000
```
fun printFibonacci(upperBound){
	var x = 0;
	var temp = 0;
	for(var b = 1; x < upperBound;b = temp + x)
	{
		print x;
		temp = x;
		x = b;
	}
}

printFibonacci(1000);
```
  
