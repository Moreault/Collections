namespace Collections.Common.Tests;

[TestClass]
public class CollectionChangeEventArgsTests : ToolBX.Collections.UnitTesting.RecordTester<CollectionChangeEventArgs<CollectionChangeEventArgsTests.Garbage>>
{
    public sealed record Garbage
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}