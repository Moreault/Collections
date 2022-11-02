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

### Breaking changes

#### 1.0.X -> 1.1.X

-Now uses Vector2<int> instead of Coordinates
-Coordinates still exist and now have implicit operators to give you time to move to Vector2

#### 1.1.X -> 1.2.X

-Column and Row counts are now based on actual index values rather than starting from zero (use FirstColumn, LastColumn, FirstRow and LastRow for boundaries instead)

![Inventory](https://github.com/Moreault/Collections/blob/master/inventory.png)
## Inventory

A linear-indexed collection of unique item entries with their quantity. 

But what does that mean in English?
Have you ever played an RPG? Think of Inventory<T> like a bag of items your party would carry. It can hold stacks of potions, swords or pieces of equipment you picked up along the way.

Could this be used outside of video games? 
Absolutely. If you need a collection that can list out unique item entries with their quantities, then this is for you. 

### Getting started

You can set the inventory's stack limit when you instantiate it or later on using the StackSize property.

```c#

//This Inventory will hold a maximum of 999 instances for every item
var inventory = new InventoryTable<Item>(999);

//You can, however, use the ToInventoryTable() extension if you really need the same items with a different stack size for some reason.
var newInventory = inventory.ToInventoryTable(1500);

//Or turn it into an InventoryList using the similar extension method
var inventoryList = inventory.ToInventoryList(1500);

//Attempting to add more items beyond that stack limit will result in an exception being thrown. 
newInventory.Add(excalibur, 1501);

//The same thing will happen if you attempt to remove more than it holds.
newInventory.Remove(potion, 3000);

//If you like “safe” methods that do not throw under questionable usage then you’ll be pleased to know that InventoryTable<T> and InventoryList<T> both have TryAdd() and TryRemove() methods

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

### StackSize : What the hell is this?

The stack size is the maximum quantity allowed per stack of item. In JRPGs of old such as Final Fantasy, this value was typically 99. 

A stack of item is basically an entry in the inventory system. With a limit of 99, you could only have 99 phoenix down per entry for instance.

As of version 1.1.0, the stack size is Int32.MaxValue by default (aka extremely high.) Prior to 1.1.0, this value was 99.

This value can be set either via constructor or the StackSize property directly.

#### What happens if I set a StackSize that is lower than my item quantities?

Those items are automatically discarded without a fuss. In other words, it does not throw exceptions. 

Shrinking down your inventory's quantities via StackSize is considered to be similar to the method Clear.

The CollectionChanged event will trigger if any items are discarded using this technique.

#### What if I want different stack sizes per item type?

There is currently no built-in way to do this but I am looking into it. Right now you would have to check it manually. 

I'm trying to come up with a smart way of doing this that doesn't complexify its usage. In other words; it might take a while!

### Which should I use? InventoryTable or InventoryList?

I expect InventoryList would work for most needs. However, both collections have important differences.

InventoryTable only allows one stack of each item. What that means is that you can only have one "X-Potion" stack in your inventory. Every time you add X-Potions to your inventory it would just add to that unique existing stack until it reaches maximum stack size. From there on it'll throw exceptions if you try to add more (or you can use TryAdd if you don't care about exceptions.)

InventoryList on the other hand can have as many stacks of the same item as you want. It'll let you carry multiple stacks of 99 X-Potions for instance. Adding more X-Potions to a full stack will add a new stack with the remainder. 

### Searching

In addition to the awesome LINQ extension methods, there are a few additional methods you can use to perform searches on your inventory object.

#### IndexesOf

This is a classic and should be straightforward enough. It returns all indexes of an item or predicate.

#### QuantityOf

Returns total items of this type in stock. Also includes an overload with a predicate.

#### Search

Returns all entries (item, quantity and index) that correspond to item or predicate. This is is basically a combination of the above methods that you may need if you need more detail.

This is similar to how it works in Final Fantasy XIV when you want to know if you have an item in your inventory (or in one of your retainers') and it tells you exactly in which tab to look. 

This method won't tell you tabs though since the Inventory classes only know about indexes. You're going to have to bridge the gap between indexes and tabs yourself if that's what you want to do.

##### Wait a second! You play critcally-acclaimed MMORPG Final Fantasy XIV?!

I sure do! Hit up Seiraniel Ratsbane the next time you're on Balmung.

##### How do you find time to work a full-time job, code, make a game AND play games??

I don't sleep. 

### Serialization
As of version 1.1.0, only JSON is supported. XML support is planned for later.

Both Newtonsoft and System.Text are supported for JSON.

Versions prior to 1.1.0 did not support serialization at all.

### Custom Inventory types
Both Inventory and InventoryList inherit from InventoryBase which you can also use to make your own custom inventory types.

I would recommend against inheriting from Inventory or InventoryList directly but I also don't like making classes "sealed" so you're also free to do so.

#### But how do I trigger change events on my inherited class?
There is a protected OnCollectionChanged method on the base class.

#### Do I HAVE to use collection change events? 
I would do it if only for code consistency but it's entirely up to you.

### Breaking changes

#### 1.0.X -> 1.1.X

Name changes
-Inventory<T> has been renamed InventoryTable<T>
-A new abstract class Inventory<T> has been added which should not be confused with the above
-InventoryEntry<T> has been renamed Entry<T>

The StackSize property has been changed
-It now has a public setter (it was read-only)
-Its default value has been changed from 99 to Int32.MaxValue;

Why these changes to StackSize?
To accomodate JSON and (eventually) XML serialization.

![ReadOnly](https://github.com/Moreault/Collections/blob/master/readonlylist.png)
## ReadOnly

Actual read-only collections with helper extension methods to make them less painful to use.

### Why??
Because, to the best of my knowledge, there exists no true read-only list-like collection in .NET that is available outside of Microsoft's internal libraries. The closest thing we have right out of the box is probably an array but even that is not truly read-only because you can still use the set accessor as you like.

The ReadOnlyList merely implements the official IReadOnlyList and not much more. It cannot be modified in any way. You cannot cast it to an array, IList<T>, List<T> or anything else.

Read-only collections are a super handy tools to protect your code against your ill-intentioned colleagues who see no harm in "returning" objects using a list that is passed to the method.

Have you ever seen this awful anti-pattern?

```c#
public void SomeMethod(Thing thing)
{
    var errors = new List<Error>();
    var isValid = ValidateThisThing(thing, errors)
}

//Not only do you "return" your errors in a list that is passed to it but you also need to instantiate it before you do! What CLEVERNESS!
public bool ValidateThisThing(Thing thing, List<Error> errors)
{
    bool singleReturnBoolBecauseWhyNotAtThisPoint;
    
    //Double negative conditions! My favorites!
    if (!string.IsNullOrWhiteSpace(thing.Name))
    {
        singleReturnBoolBecauseWhyNotAtThisPoint = true;
    }
    else
    {
        errors.Add(new Error("This thing has no name!"))
        singleReturnBoolBecauseWhyNotAtThisPoint = false;
    }

    //I mean why not?
    return singleReturnBoolBecauseWhyNotAtThisPoint;
}
```

Oh I have seen it and read-only collections are my answers to it. 

### Getting started

```c#
//Straightforward enough, right? You just fill it with anything you like
var things = new ReadOnlyList<Thing> { ... };

//Alternatively, you can also pass an existing collection to it
var readOnlyThings = new ReadOnlyList<Thing>(things);

//Or even better
var readOnlyThings = things.ToReadOnlyList();

//It also comes (like all other ToolBX collections for that matter) with overloaded equality operators for meaningful comparisons
var areEqual = things == readOnlyThings; //This returns true, provided that both collections contain the same items in the same order
```

For me, the most interesting part of this package are the following extension methods : 

```c#
//Creates a new ReadOnlyList with the new elements at the end
var newThings = things.With(new Thing("abc"), new Thing("def"));

//Removes the specified things
var newThings = things.Without(thing1, thing5, thing8);

//You can even remove items using a predicate lambda
var newThings = things.Without(x => x.Tag == "Something");
```

The best part of the above is that the extensions are on IReadOnlyList<T> and not on the concrete type so you can use them on all your existing IReadOnlyList<T> collections.