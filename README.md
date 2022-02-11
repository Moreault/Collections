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

## Grid
An observable, dynamic two-dimensional array.

Which one should you use? T[,] or T[][]? What is the difference? How do you instantiate this again? Use Grid<T>!

Grid<T> is to T[,] what ObservableList<T> is to T[]. It's easy to use and, as a bonus, is observable as well!

Unlike 2D arrays, it does support negative indexes. In other words, the following is allowed:

```c#
grid[-13, 4] = "thingy";
```

### Getting started

```c#
//Instantiation
var grid = new Grid<string>();

//You don't need to specify the grid's boundaries as it'll "expand" automatically
grid[20, 45] = "Something";

//Deconstructors are provided for the cell
foreach (var ((x, y), value) in grid)
{
    ...
}

//You can also just iterate through it like this if you're old school and systematically hate syntaxic sugar that came out after 201X
foreach (var cell in grid)
{
    if (cell.Index.X > 0)
    {
        ...
    }
}

//You can also listen for changes
grid.CollectionChanged += OnGridChanged;

//This will flood fill the grid starting with the index that was passed and will automatically stop at the grid's current boundaries
grid.FloodFill(4, 5, "ToolBX!");

//Or you can specify boundaries manually if you want to go over (or under) its limits
grid.FloodFill(4, 5, "ToolBX!", new Boundaries<int> { Top = -10, Left = -5, Bottom = 40, Right = 80 });

//You can even clear all of that up
grid.FloodClear(10, 5);
```

It even has equality overloads so that you're not merely comparing references whenever you test for equality. Not only that but it also has overloads for most similar types such as dictionaries, 2d arrays and even jagged arrays.

And extension methods!

```c#
//You can also make a 2d array out of it if that's your thing
var array = grid.To2dArray();

//Or turn a 2d array into a grid
var grid = array.ToGrid();

//Yep, even jagged arrays if you like those! I'm not judging! (I am)
var jagged = grid.ToJaggedArray();

//And also turn it back into a grid!
grid = jagged.ToGrid();
```