# Interpreter
Interpreter based on the book [Crafting interpreters](https://craftinginterpreters.com/contents.html)
## Native functions
### clear
Clears the console e.g.
```
clear();
```
### clock
Displays the current time (right now only seconds) e.g.
```
var time = clock();
```
### read
Reads input from the user until he presses enter e.g.
```
var input = read();
```
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
print "Hello " + name;
```
Print the current time (seconds)
```
for (var i = 0; i < 60; i = i + 1) {
	print clock();
	wait(1);
	clear();
	print clock();
}
```
Creates a counter
```
fun makeCounter() {
  	var i = 0;
  	fun count() {
    		return i = i + 1;
  	}
  	return count;
}

var counter = makeCounter();
print counter(); // "1".
print counter(); // "2"
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
  
