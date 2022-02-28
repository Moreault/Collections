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

![Grid](https://github.com/Moreault/Collections/blob/master/grid.png)
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

![Grid](https://github.com/Moreault/Collections/blob/master/inventory.png)
## Inventory

A linear-indexed collection of unique item entries with their quantity. 

But what does that mean in English?
Have you ever played an RPG? Think of Inventory<T> like a bag of items your party would carry. It can hold stacks of potions, swords or pieces of equipment you picked up along the way.

Could this be used outside of video games? 
Absolutely. If you need a collection that can list out unique item entries with their quantities, then this is for you. 

### Getting started

You set the inventory’s stack limit when you instantiate it and it cannot be modified afterwards.

```c#

//This Inventory will hold a maximum of 999 instances for every item
var inventory = new Inventory<Item>(999);

//You can, however, use the ToInventory() extension if you really need the same items with a different stack size for some reason.
var newInventory = inventory.ToInventory(1500);

//Attempting to add more items beyond that stack limit will result in an exception being thrown. 
newInventory.Add(excalibur, 1501);

//The same thing will happen if you attempt to remove more than it holds.
newInventory.Remove(potion, 3000);

//If you like “safe” methods that do not throw under questionable usage then you’ll be pleased to know that Inventory<T> has methods TryAdd() and TryRemove()

//This will cap out the amount of rubber ducks in the inventory to its allowed maximum quantity of 1500 regardless of how many you try to add
newInventory.TryAdd(rubberDuck, 30000);

//TryAdd and TryRemove both return a result object which can tell you how many items were successfully added and how many remain
var result = newInventory.TryAdd(rubberDuck, 30000);

//There are also overloads to Add, Remove and other methods which use a predicate

//The following will remove 45 quantity from all items that are named "Roger"
newInventory.Remove(x => x.Name == "Roger", 45)

//Like other collections in the larger namespace, it is also observable using the exact same syntax as the ObservableList
inventory.CollectionChanged += OnCollectionChanged;
```