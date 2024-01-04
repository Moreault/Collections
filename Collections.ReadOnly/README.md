![ReadOnly](https://github.com/Moreault/Collections/blob/master/readonlylist.png)

A read-only collection of elements that can be accessed via indexer.

# ReadOnlyList vs ImmutableList

.NET already has excellent read-only (or immutable) collections in the `System.Collections.Immutable` namespace. Here are the pros and cons of both.

* ReadOnlyList<T> uses value equality for both its `Equals` and `GetHashCode` methods including all its equality operators
* ImmutableList<T> uses reference equality for its `Equals` and `GetHashCode` methods
* Both are immutable
* Both are thread-safe
* ImmutableList<T> can be serialized using System.Text.Json by default
* ReadOnlyList<T> can only be serialized using System.Text.Json by using the `ReadOnlyListJsonConverter` class
* Both are serializable using Newtonsoft.Json without requiring custom converters

# Why use this?
The following example is a common (bad) pattern that I have seen over the years. Because the method returns a boolean, the developer has to pass a list of errors to it rather than returning a result object that contain both the boolean and the list of errors. Objects passed to methods should _never_ be modified by the method itself. This is a bad practice that leads to hard to maintain code. Not only that, but expecting the caller to pass a list of errors to the method is also a bad practice. The method should be responsible for creating the list of errors and returning it to the caller.

```c#
public void SomeMethod(Thing thing)
{
    var errors = new List<Error>();
    var isValid = ValidateThisThing(thing, errors)
}

public bool ValidateThisThing(Thing thing, List<Error> errors)
{
    if (string.IsNullOrWhiteSpace(thing.Name))
    {
        return true;
    }
    errors.Add(new Error("This thing has no name!"));
    return false;
}
```

So you see, enforcing the use of immutable (or read-only) collections prevents developers from making bad decisions.

# Getting started

```c#
//Straightforward enough, right? You just fill it with anything you like
var things = new ReadOnlyList<Thing>(new Thing(...), new Thing(...), new Thing(...));

//Alternatively, you can also pass an existing collection to it
var readOnlyThings = new ReadOnlyList<Thing>(things);

//Or even better
var readOnlyThings = things.ToReadOnlyList();

//It also comes (like all other ToolBX collections for that matter) with overloaded equality operators for meaningful comparisons
var areEqual = things == readOnlyThings; //This returns true, provided that both collections contain the same items in the same order
```

It also comes with a bunch of useful extension methods that make working with read-only collections a breeze. The following examples all return a new `IReadOnlyList<T>`. The original collection is never modified.

```c#
//Creates a new ReadOnlyList with the new elements at the end
var newThings = things.With(new Thing("abc"), new Thing("def"));

//Removes the specified things
var newThings = things.Without(thing1, thing5, thing8);

//You can even remove items using a predicate lambda
var newThings = things.Without(x => x.Tag == "Something");
```

# System.Text.Json
By default, `ReadOnlyList<T>` does not work with System.Text.Json but you can make it work by using the `ReadOnlyListJsonConverterFactory` class. Here is an example of how to use it.

```c#

//First, you need to create an instance of JsonSerializerOptions
var options = new JsonSerializerOptions();

//Then, you need to add the converter to the options
options.Converters.Add(new ReadOnlyListJsonConverterFactory());

//Alternatively, you can use this extension method which does the same thing
options.WithReadOnlyConverters();

//Finally, you can serialize and deserialize your read-only collections
var json = JsonSerializer.Serialize(things, options);
var newThings = JsonSerializer.Deserialize<ReadOnlyList<Thing>>(json, options);
```