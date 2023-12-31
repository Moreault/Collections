namespace Collections.Grid.Tests;

[TestClass]
public sealed class GridChangedEventArgsTests : RecordTester<GridChangedEventArgs<Dummy>>
{
    [TestMethod]
    public void Always_EnsureConsistentHashCode() => Ensure.ConsistentHashCode<GridChangedEventArgs<Dummy>>(Fixture);
}