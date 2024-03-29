﻿namespace ToolBX.Collections.Inventory;

public sealed record Entry<T> : EntryBase<T>
{
    public Entry()
    {

    }

    public Entry(T item, int quantity = 1) : base(item, quantity)
    {

    }

    public override string ToString() => base.ToString();
}

//TODO 3.0.0 : Replace current Entry<T> with his one
//public record Entry<T>
//{
//    public T? Item { get; init; }

//    public int Quantity
//    {
//        get => _quantity;
//        init => _quantity = value <= 0 ? throw new ArgumentOutOfRangeException(nameof(value), value, Exceptions.QuantityMustBeAtLeastOne) : value;
//    }
//    private readonly int _quantity;

//    public Entry()
//    {

//    }

//    public Entry(T item, int quantity = 1)
//    {
//        Item = item;
//        Quantity = quantity;
//    }

//    public void Deconstruct(out T? item, out int quantity)
//    {
//        item = Item;
//        quantity = Quantity;
//    }

//    public override string ToString() => $"{(Item is null ? "NULL" : Item.ToString())} x{Quantity}";
//}