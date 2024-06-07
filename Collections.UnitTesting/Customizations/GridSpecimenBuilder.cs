﻿namespace ToolBX.Collections.UnitTesting.Customizations;

public sealed class GridCustomization : GridCustomizationBase
{
    protected override IEnumerable<Type> Types => [typeof(Grid<>)];

    protected override object Convert<T>(IEnumerable<Cell<T>> source) => source.ToGrid();
}