namespace Collections.Grid.Tests;

[TestClass]
public sealed class GridChangedEventArgsTests : RecordTester<GridChangedEventArgs<Garbage>>
{
    protected override void InitializeTest()
    {
        base.InitializeTest();
        Dummy.WithCollectionCustomizations();
    }
}