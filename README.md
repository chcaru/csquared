# csquared
CSquared is an interpreted programming language implemented completely in C#.

-- Variables --

	Example variable declaration:

		var myVar = 0;

	All variable declarations must be preceeded with "var."

	Identifiers must be ALL letters (no numbers ect...)!

	Assignment:

		myVar = "a string";
		myVar = 5.4;
		myVar = true;
		myVar = false;

	Intergers, Reals, Strings, Booleans, Objects, Arrays, Lambdas, and Lambda Calls are supported for declaration/assignment.

	*Strings are enumerable.

	*Reals must be "full," and cannot be partial (.0 or 0. is not valid, 0.0 is)

	*Objects, Arrays, Lambdas, and Lambda Calls are detailed in the following.


-- Lambdas --

	Lambda expressions are of the form:

		(p1, p2, ... , pn) => { s1; s2; ... sn; }
	OR
		(p1, p2, ... , pn) => e

	where p is a parameter name, s is a body statement, and e is an expression.

	For example:

		(x, y) => x + y  

		(x, y) => { return x + y; }

	are a valid lambda expressions. If the lambda has no body, the expression's value is implicitly returned.
	If the lambda has a body, then there must be an explicit return, otherwise null is returned by default.

	Lambda expressions can be used like functions when assigned to a variable.

		var sum = (x, y) => {
			return x + y;	
		};

	OR

		var sum = (x, y) => x + y;

	Calling a lambda expression is of the form:

	var a = 5;

	var z = sum(a, 4); //z would then be 9

	sum(a, 4); // is valid aswell


-- Objects --

	Objects are of the form:

		{
			varDec1;
			varDec2;
			...
			varDecn;
		}

	For example, to assign an Object to a variable:

		var person = {
			var name = "chris caruso";	
			var age = 21;
			var toString = () => name + ": " + age;
		};

	Members of the object can be accessed as in the following example:

		var myName = person.name;
		var myAge = person.age;
		var me = person.toString();
		var doubleAge = person.age * 2;

	Assigning a member of an object is as follows:

		person["name"] = "christopher a caruso";

	*Assignment of an object's members is limited to the object's immediate members, not nested :<

	Objects are enumerable.


-- Arrays --

	Arrays are of the form:

	1.
		[e1, e2, ... , em](n)

	OR

	2.
		[e1, e2, ... , en]

	Where n and m are integers (m <= n) and e is an expression.

	An Array's size can either be implicitly or explicity defined.
	Example 1 explicity defines a size of n with the pre-exisiting values 0 .. m
	Example 2 implicity define a size of n as denoted by the number of existing expressions.

	Example of assigning an Array to a variable:

		var arrayA = [1, 2, "sd", {}];

	OR

		var arrayB = [1, 2, "sd", {}](10);

	arrayA has a size of 4, while arrayB has a size of 10.

	Unitialized values in an explicitly sized array are defaulted to null.

	Indexing/Assigning an Array:

		var a = arrayB[0];

		arrayB[7] = "e";

	Arrays are enumerable.


-- Iteration --


	-- Foreach statement --

		A foreach statement is used to iterate through an enumerable type.

		Enumerable types include Strings, Arrays, and Objects.

		The form of a foreach statement is as follows:

			foreach (id in enumerable){
				s1;
				s2;
				...
				sn;
			}

		where id is an identifier (no declaration required, no var required) and enumerable is any enumerable type.
		s is a statement.

		The statements in the body will be executed for each enumerated expression in the enumerable, given to the 
		variable with the name id.

		For example:

			var array = ["a", "b", "c"];

			foreach (item in array){
				println(item);
			}

		would print the value of each item in the array. 

		Items in an object are anonymously enumerated.

		Strings of size 1 are to be expected from enumeration of a String. (each char in the string)


	-- While statement --

		A while statement acts like a normal while statement would in C.

		It is of the form:

			while (be1) {
				s1;
				s2;
				...
				sn;
			}

		where be is a boolean expression or an expression which can evaluate to a boolean value and s is a statement.

		The body statements will be executed for each iteration while the boolean expression is true.

		Everything except statements can evaluate to a boolean. Anything which is Null is false. If the expression is boolean, it assumes its value, otherwise, since it is not null, it is true.

			EXP.:  VALUE
			------------
			null:  false
			true:  true
			false: false
			else:  true

		This will also apply to if statements later.


-- Conditionals --

	The if statement is the only form of condition control, other than a while statement.

	The work the same as they do in C. Except they must have a block.

	The if statement is of the following form:

		if (be1){
			s1;
			s2;
			...
			sn;
		}

	OR

		if (be1){
			s1;
			s2;
			...
			sn;
		}
		else {
			s1;
			s2;
			...
			sm;
		}

	where be is a boolean expression and follows the same rules as detailed in the While statement section. 

-- Operators --

	Operators include:
		+
		-
		/
		*
		==
		!=
		|
		&
		<
		<=
		>
		>=
		!
		<-
		->

	All operators work as normal. 

	<- operator:

		This operator can be used on an object to add an item to the end of an object anonymously 

			var obj = { };

			obj <- 3;
			obj <- 4;

		the following operations can be conceptualized as:

			// obj = { 3; 4; }; 

		OR

		can be used to remove the first item of an object:

			//cont from prev. example

			var b;

			b <- obj;

			//b is 3

		*Items added to an object anonymously can be access as above or during enumeration of a foreach statement.

	-> operator:

		This operator can be used on an object to remove the last item of the object.

			var obj = { };

			obj <- 3;
			obj <- 4;

			var b;

			obj -> b;

			//b is 4


-- Built in functions --
	
	-- Type checks --

		isBool(e);
		isInt(e);
		isReal(e);
		isObject(e);
		isArray(e);
		isLambda(e);

	-- Casts --

		toInt(e);
		toReal(e);
		toString(e);
		toBool(e);

	-- Other --

		println(s);
		print(s);
		input();
		count(e); // returns the number of items in an enumerable
		empty(e); // returns if there are any items in an enumerable
		inject(s); // takes a string, evaluates it as code in the current line of execution


-- CONCURRENCY! --

	-- Parallelization --

		Parallelization is performed via use of a foreach statement.

		With the parallel modifier placed before a foreach statement, the foreach statement will execute each body block corresponding to an enumerated value in parallel. Note, this means the order is indeterminate.

		Foreach example:

			var array = ["a", "b", "c", "d"];

			parallel foreach (item in array){
				println(item);
			}

		could result in printing the values in array in any order.

	-- Mutual exclusion --

		Mutex like behavior can be exhibited through use of the lock statement.

		For example: 

			var a = 1;

			lock(a){
				a = a + 1;
			}

		Will only allow access to the variable a inside the block.

		Access to a other than inside the block statements will result in blocking the calling thread.

	-- Asynchronous execution --

		Expressions can be execude currently in the background using the async modifier.

		async placed before any expression will start execution of the expression in a background thread and return control to the calling thread immediately. Though, the expression's value is not returned, an asynx expression in a busy state is. The value of the expession can be access by trying to evaluate the async expression AFTER it has returned the first time. If the async expression is still running, and is busy, the accessing thread will be blocked until the async expression has finished, and will return the evaluated value and return control to the accessing thread. 

		For example:

			var c = 0;

			var func = () => {
				
				while (c < 3000000){
					c = c + 1;
				}
				return c;
			};

			var count = async func();

			println("func is executing in the background...");

			//Busy time
			var i = 0;
			while (i < 100000){
				i = i + 1;
			}

			println("current value of c is " + c);

			println("final value is " + count); // blocks this thread and waits for the async func call to finish
