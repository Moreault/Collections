namespace Collections.Common.Tests;

[TestClass]
public class CollectionChangeEventArgsTests : RecordTester<CollectionChangeEventArgs<CollectionChangeEventArgsTests.Dummy>>
{
    public sealed record Dummy
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }
}