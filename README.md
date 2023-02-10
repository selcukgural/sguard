
The **SGuard** library is a collection of tools for validating values and throwing exceptions if they do not meet certain conditions. It is designed to help developers ensure that their code is working as intended and that input values are valid, which can help prevent errors and exceptions from occurring in production environments.

One of the key features of SGuard is its use of the callback pattern, which allows developers to specify custom error messages for the exceptions that are thrown if validation fails. This can help developers provide more helpful and specific error messages to their users.

In addition to the callback pattern, the SGuard library also makes use of the `DoesNotReturn` attribute, which is applied to certain methods in the `Throw` class. This attribute indicates that the method does not return a value, and is used to mark methods that throw exceptions as "does not return" in the C# language. **This can be helpful for static analysis tools and other code analysis tools that can use this information to ensure that code is working as intended.**

The `Is` class in the SGuard library provides a range of methods for validating values. Some of these methods include:

[Nuget](https://www.nuget.org/packages/SGuard/1.0.0)

`dotnet tool install --global SGuard --version 1.0.0`

# Usage samples

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/381c27d9f6c54fe1890f1919d4bd8b14)](https://app.codacy.com/gh/selcukgural/sguard?utm_source=github.com&utm_medium=referral&utm_content=selcukgural/sguard&utm_campaign=Badge_Grade_Settings)

## NullOrEmpty : `boolean`
This method is a generic extension method that checks whether a nullable value type is null or empty. It has two parameters:

* `T? value`: This is a nullable value type that will be checked for null or empty.
* `Action<CallbackOption>? option`: This is an optional parameter of type `Action<CallbackOption>` that represents a callback function that takes a `CallbackOption` object as an argument.

```c sharp
//NullOrEmpty
var testString1 = "hello";
var testString2 = "";

Console.WriteLine(Is.NullOrEmpty(testString1)); //False
Console.WriteLine(Is.NullOrEmpty(testString2)); //True


var testArray1 = new[] { 1, 2, 3 };
var testArray2 = Array.Empty<int>();

Console.WriteLine(Is.NullOrEmpty(testArray1)); //False
Console.WriteLine(Is.NullOrEmpty(testArray2)); //True
```
<br/>

* With global callback only triggered when it's `false`
```c sharp
// Set the global callback action
Is.SetCallback(() => Console.WriteLine("A null or empty check was performed."));

// Check if a string is null or empty
var testString1 = "hello";
var testString2 = "";

Console.WriteLine(Is.NullOrEmpty(testString1));
// Output: False
//         A null or empty check was performed.
Console.WriteLine(Is.NullOrEmpty(testString2));
// Output: True
```
<br/>

* With optional callback
```c sharp
// Check if a string is null or empty
var testString1 = "hello";
var testString2 = "";
                                                        // We can do what you want instead of printing it to the console.
Console.WriteLine(Is.NullOrEmpty(testString1, option => Console.WriteLine("A null or empty check was performed.")));
// A null or empty check was performed.
// Output: False
Console.WriteLine(Is.NullOrEmpty(testString2));
// Output: True
```

<br/>

* With conditional optional callback
```c sharp
const string testString = "";

// Output: This method will be called.
Is.NullOrEmpty(testString, option =>
{
    option.InvokeCallbackWhenNullOrEmpty = true;
    option.SetCallback(() => Console.Write("This method will be called"));
});

Is.NullOrEmpty(testString, option =>
{
    option.InvokeCallbackWhenNullOrEmpty = false;
    option.SetCallback(() => Console.Write("This method will NOT be called"));
});
```
<br/>

* With `Expression`
```c sharp
public class Car
{
    public string Brand { get; set; }
}

Car car = new();

 // True
Is.NullOrEmpty(car, e => e.Brand);
```
<br/>

## NullOrEmptyThrow : `void`
This method just like `NullOrEmpty` main difference is when value null or empty it's will be throw `IsNullOrEmptyException`

```c sharp
// Throwing IsNullOrEmptyException
var emptyString = "";
Is.NullOrEmptyThrow(emptyString); 

// Throwing IsNullOrEmptyException
Car car = new();
Is.NullOrEmptyThrow(car, e => e.Brand); 

// Throwing IsNullOrEmptyException
var emptyArray = Array.Empty<int>();
Is.NullOrEmptyThrow(emptyArray);

//traditional method
Car car = new();
if (string.IsNullOrEmpty(car.Brand))
{
    throw new ArgumentNullException(nameof(car));
}

// I can do that much simple like that
Is.NullOrEmptyThrow(car, e => e.Brand);

// Another sample with callback
Is.NullOrEmptyThrow(car, e=> e.Brand, _ => throw new DomainSpecificException("...."));
```
<br/>


-   `IsNull`: Validates that a nullable value is not null.
-   `IsBetween`: Validates that a value is within a certain range.
-   `IsGreaterThan`: Validates that a value is greater than another value.
-   `IsGreaterOrEqualThan`: Validates that a value is greater than or equal to another value.
-   `IsLessThan`: Validates that a value is less than another value.
-   `IsLessOrEqualThan`: Validates that a value is less than or equal to another value.

In addition to these methods, the SGuard library also provides the following utility methods:

-   `NullOrEmpty`: Determines whether a nullable value is null or empty.
-   `AllNull`: Determines whether all elements in a collection are null or empty.
-   `AnyNull`: Determines whether any element in a collection is null or empty.

Overall, the SGuard library can be a valuable tool for developers looking to improve the reliability and robustness of their code. It provides a simple and flexible way to validate values and ensure that they meet certain conditions, helping to prevent errors and exceptions from occurring in production environments. Its use of the callback pattern and the `DoesNotReturn` attribute further enhance its usefulness and make it a powerful tool for developers.
