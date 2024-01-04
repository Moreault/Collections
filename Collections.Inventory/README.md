![Inventory](https://github.com/Moreault/Collections/blob/master/inventory.png)
# Inventory

A linear-indexed collection of unique item entries with their quantity. 

But what does that mean in English?
Have you ever played an RPG? Think of Inventory<T> like a bag of items your party would carry. It can hold stacks of potions, swords or pieces of equipment you picked up along the way.

Could this be used outside of video games? 
Absolutely. If you need a collection that can list out unique item entries with their quantities, then this is for you. 

## Getting started

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

## StackSize : What the hell is this?

The stack size is the maximum quantity allowed per stack of item. In JRPGs of old such as Final Fantasy, this value was typically 99. 

A stack of item is basically an entry in the inventory system. With a limit of 99, you could only have 99 phoenix down per entry for instance.

As of version 1.1.0, the stack size is Int32.MaxValue by default (aka extremely high.) Prior to 1.1.0, this value was 99.

This value can be set either via constructor or the StackSize property directly.

### What happens if I set a StackSize that is lower than my item quantities?

Those items are automatically discarded without a fuss. In other words, it does not throw exceptions. 

Shrinking down your inventory's quantities via StackSize is considered to be similar to the method Clear.

The CollectionChanged event will trigger if any items are discarded using this technique.

### What if I want different stack sizes per item type?

There is currently no built-in way to do this but I am looking into it. Right now you would have to check it manually. 

I'm trying to come up with a smart way of doing this that doesn't complexify its usage. In other words; it might take a while!

## Which should I use? InventoryTable or InventoryList?

I expect InventoryList would work for most needs. However, both collections have important differences.

InventoryTable only allows one stack of each item. What that means is that you can only have one "X-Potion" stack in your inventory. Every time you add X-Potions to your inventory it would just add to that unique existing stack until it reaches maximum stack size. From there on it'll throw exceptions if you try to add more (or you can use TryAdd if you don't care about exceptions.)

InventoryList on the other hand can have as many stacks of the same item as you want. It'll let you carry multiple stacks of 99 X-Potions for instance. Adding more X-Potions to a full stack will add a new stack with the remainder. 

## Searching

In addition to the awesome LINQ extension methods, there are a few additional methods you can use to perform searches on your inventory object.

### IndexesOf

This is a classic and should be straightforward enough. It returns all indexes of an item or predicate.

### QuantityOf

Returns total items of this type in stock. Also includes an overload with a predicate.

### Search

Returns all entries (item, quantity and index) that correspond to item or predicate. This is is basically a combination of the above methods that you may need if you need more detail.

This is similar to how it works in Final Fantasy XIV when you want to know if you have an item in your inventory (or in one of your retainers') and it tells you exactly in which tab to look. 

This method won't tell you tabs though since the Inventory classes only know about indexes. You're going to have to bridge the gap between indexes and tabs yourself if that's what you want to do.

#### Wait a second! You play critcally-acclaimed MMORPG Final Fantasy XIV?!

I sure do! Hit up Seiraniel Ratsbane the next time you're on Balmung.

#### How do you find time to work a full-time job, code, make a game AND play games??

I don't sleep. 

## Serialization
As of version 1.1.0, only JSON is supported. XML support is planned for later.

Both Newtonsoft and System.Text are supported for JSON.

Versions prior to 1.1.0 did not support serialization at all.

## Custom Inventory types
Both Inventory and InventoryList inherit from InventoryBase which you can also use to make your own custom inventory types.

I would recommend against inheriting from Inventory or InventoryList directly but I also don't like making classes "sealed" so you're also free to do so.

### But how do I trigger change events on my inherited class?
There is a protected OnCollectionChanged method on the base class.

### Do I HAVE to use collection change events? 
I would do it if only for code consistency but it's entirely up to you.

## Breaking changes

### 1.0.X -> 1.1.X

Name changes
-Inventory<T> has been renamed InventoryTable<T>
-A new abstract class Inventory<T> has been added which should not be confused with the above
-InventoryEntry<T> has been renamed Entry<T>

The StackSize property has been changed
-It now has a public setter (it was read-only)
-Its default value has been changed from 99 to Int32.MaxValue;

Why these changes to StackSize?
To accomodate JSON and (eventually) XML serialization.