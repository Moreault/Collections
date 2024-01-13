# Collections
Modern and straightforward .NET Collections

![ObservableList](https://github.com/Moreault/Collections/blob/master/ObservableList.png)
## ObservableList
An observable, dynamic one-dimensional array.

Ever wish C#'s list would tell you if it has been modified in any way without having to clumsily wrap it like so

```c#
private readonly List<Thing> _things = new List<Thing>();

public void Add(Thing thing)
{
   _things.Add(thing);
   DoStuff();
}
```

This is where ObservableList shines the brightest. 

```c#
private readonly ObservableList<Thing> _things = new ObservableList<Thing>();

public void SomeInitializationMethodSomewhere()
{
  _things.CollectionChanged += OnThingsChanged;
}

//This gets called whenever the ObservableList is changed be it through Add, Insert, Remove, RemoveAt, Clear, etc...
private void OnThingsChanged(object sender, CollectionChangeEventArgs<Thing> args)
{
  foreach (var item in args.OldValues)
  {
    DoStuffWithOldValues();
  }
  
  foreach (var item in args.NewValues)
  {
    DoStuffWithNewValues();
  }
}
```

### Breaking changes

#### 1.0.X -> 1.1.X

-Now uses a List internally instead of an array so it could break in places but signatures remain largely unchanged so do not expect compile errrors
-IndexesOf overloads return IReadOnlyList<T> instead ObservableList<T>
-The DLL now comes packaged with dependencies to the OPEX library and uses its extensions for many of its methods