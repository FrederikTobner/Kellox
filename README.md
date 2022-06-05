# Interpreter
Interpreter based on the book [Crafting interpreters](https://craftinginterpreters.com/contents.html)
## Native functions
### clear
Clears the console
### clock
Displays the current time (right now only seconds)
### read
Reads input from the user until he presses enter
### wait
Suspends the program for x seconds e.g. 
```
wait(5.25);
```
## Example programs
Hello World
```
print "Hello World";
```
Greet the user after typing in the name
```
print "Please type in your name:";
var name = read();
print "Hallo " + name;
```
Print the first 10 Fibonacci Numbers
```
fun fibonacci(){
	var x = 0;
	var temp = 0;
	var b = 1;
	fun number(){
		temp = x;
		x = b;
		b = temp + x;
		return temp;
	}
	return number;
}

var fibo = fibonacci();
for (var j = 0; j < 10; j = j + 1) {
	print fibo();
}
```
  
