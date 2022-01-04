# Collections
Modern and straightforward .NET Collections

![Deck](https://github.com/Moreault/Collections/blob/master/deck.png)
## Deck
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

This is where Deck shines the brightest. 

```c#
private readonly Deck<Thing> _things = new Deck<Thing>();

public void SomeInitializationMethodSomewhere()
{
  _things.CollectionChanged += OnThingsChanged;
}

//This gets called whenever the Deck is changed be it through Add, Insert, Remove, RemoveAt, Clear, etc...
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
