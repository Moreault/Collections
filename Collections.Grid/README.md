[Grid](https://github.com/Moreault/Collections/blob/master/grid.png)
# Grid
An observable, dynamic two-dimensional array.

Which one should you use? T[,] or T[][]? What is the difference? How do you instantiate this again? Use Grid<T>!

Grid<T> is to T[,] what ObservableList<T> is to T[]. It's easy to use and, as a bonus, is observable as well!

Unlike 2D arrays, it does support negative indexes. In other words, the following is allowed:

```c#
grid[-13, 4] = "thingy";
```

## Getting started

### Instantiation

```c#
var grid = new Grid<string>();

var dictionary = new Dictionary<int, string> { ... };
var grid = new Grid<string>(dictionary);

var cells = new List<Cell<string>> { ... };
var grid = new Grid<string>(cells);

var array = new[] { ... }
var grid = new Grid<string>(array);
```

```c#
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

# OverlapGrid
An observable, dynamic two-dimensional array that can hold multiple values for the same position.

## What does that mean?
The `OverlapGrid<T>` has multiple entries per index. This means that `[4, 5]` for instance returns a `IReadOnlyList<T>` which can contain zero, one or multiple items. It's useful if you want to work with multiple layers without having to have an entire `Grid<T>` for each layer. Of course, having multiple `Grid<T>` per layer works in most cases.

In Rough Trigger, a `Grid<T>` is used per tilemap layer but an `OverlapGrid<T>` is used for the "more dynamic" `GameObject` placements such as enemies, doors, toilets, special zones, etc... Since `GameObjects` may overlap with one another during gameplay, it only makes sense that they should be placeable as such from the start as well.

I use it when layer precision is not as important and multiple elements could sit on top of each other without interference. For these reasons, `FloodFill` is not supported by this grid.

## Getting started
The `OverlapGrid<T>` is used in much the same way as its cousin, except that it returns a `IReadOnlyList<T>` instead of a single `T` object. 

# Conversions!

`Grid<T>` and `OverlapGrid<T>` have value-based equality overloads and can be compared with other equivalent types such as `Dictionary<int, T>`, `T[,]` and even `T[][]`!

And extension methods!

```c#
//You can also make a 2d array out of it if that's your thing
var array = grid.To2dArray();

//Or turn a 2d array into a grid
var grid = array.ToGrid();

//Yep, even jagged arrays if you like those! I'm not judging! (or am I?)
var jagged = grid.ToJaggedArray();

//And also turn it back into a grid!
grid = jagged.ToGrid();

//Or into an overlap grid!
var overlapGrid = grid.ToOverlapGrid();
```

As of version 3.0.0, `ToJaggedArray` and `To2dArray` are _explicitly_ unsupported if the `Grid<T>` has negative indexes. Before, it would throw an `ArgumentOutOfRangeException`.

# Columns, Rows & Count
There are `ColumnCount` and `RowCount` properties which return the total number of columns and rows respectively. Also `FirstColumn`, `LastColumn`, `FirstRow` and `LastRow` that behave as you would expect them to.

```c#
//Useful for iterating through the entire grid if foreach won't cut it
for (var x = grid.FirstColumn; x < grid.LastColumn; x++)
{
    for (var y = grid.FirstRow; y < grid.LastRow; y++)
    {
        ...
    }
}
```

There is also a `Count` property which returns the total number of cells that are actually in use. In other words, it doesn't take into account the number of rows and columns but rather the number of cells in which there is actually a value.

# Indexers
You can use the indexer just as you would a 2D array (`T[]`) or a `Dictionary<int, T>`. Note that only `Grid<T>`'s indexers have `set` accessors. `OverlapGrid<T>` only supports the `get` accessor.

```c#
var value = grid[3, 12];

//Can also be used to set some value. It doesn't require you to "activate" that cell beforehand
grid[51, 74] = "Some value";
```

There is even an overload which uses a `Vector2<int>` from ToolBX.Mathemancy.

```c#
var value = grid[new Vector2<int>(3, 12)];

grid[vector] = "Another value!";
```

# Boundaries
Limits are also defined by a `Boundaries<int>` property named `Boundaries`.

# FloodFill
It "floods" all cells adjascent to the one selected with the same value provided that they also have the same value to begin with. Think of it like a paint program's bucket tool because that's pretty much exactly what it is.

```c#
//Floods all color of a Grid<Color> with the color blue starting with 3,9
grid.FloodFill(3, 9, Colors.Blue);
```

It is also possible to use `Vector2<int>` instead of two `int` and it is also possible to provide a `Boundaries<int>` if you want to contain the flooding to a smaller area than the entire grid.

```c#
grid.FloodFill(vector, Colors.Purple, new Boundaries<int> { Top = 2, Left = 0, Right = 8, Down = 5 });
```

Note that `FloodFill` is not supported by `OverlapGrid<T>`.

# FloodClear
It works just like `FloodFill` except that it removes by flooding. It is also not supported by `OverlapGrid<T>` and accepts `Vector<int>` as well as `Boundaries<int>` as parameters.

```c#
grid.FloodClear(12, 14);
```

# Serialization

Both `Grid<T>` and `OverlapGrid<T>` can be serialized and deserialized using Newtonsoft.Json or System.Text.Json. The latter requires you to add the `WithGridConverters()` extension method to your `JsonSerializerOptions` object.

```c#
//Grid<T> with Newtonsoft.Json
var json = JsonConvert.SerializeObject(grid);
var grid = JsonConvert.DeserializeObject<Grid<string>>(json);

//Grid<T> with System.Text.Json
var json = JsonSerializer.Serialize(grid, new JsonSerializerOptions().WithGridConverters());
var grid = JsonSerializer.Deserialize<Grid<string>>(json, new JsonSerializerOptions().WithGridConverters());

//OverlapGrid<T> with Newtonsoft.Json
var json = JsonConvert.SerializeObject(overlapGrid);
var overlapGrid = JsonConvert.DeserializeObject<OverlapGrid<string>>(json);

//OverlapGrid<T> with System.Text.Json
var json = JsonSerializer.Serialize(overlapGrid, new JsonSerializerOptions().WithGridConverters());
var overlapGrid = JsonSerializer.Deserialize<OverlapGrid<string>>(json, new JsonSerializerOptions().WithGridConverters());
```

# Breaking changes

## 1.0.X -> 1.1.X

-Now uses Vector2<int> instead of Coordinates
-Coordinates still exist and now have implicit operators to give you time to move to Vector2

## 1.1.X -> 1.2.X

-Column and Row counts are now based on actual index values rather than starting from zero (use FirstColumn, LastColumn, FirstRow and LastRow for boundaries instead)