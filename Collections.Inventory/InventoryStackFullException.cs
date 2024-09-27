namespace ToolBX.Collections.Inventory;

public class InventoryStackFullException : Exception
{
    public InventoryStackFullException(int maximum) : base(string.Format(Exceptions.CannotAddItemBecauseMaximumIsReached, maximum))
    {

    }

    public InventoryStackFullException(int amount, int maximum) : base(string.Format(Exceptions.CannotAddItemBecauseQuantityIsGreaterThanMaximum, amount, maximum))
    {

    }
}