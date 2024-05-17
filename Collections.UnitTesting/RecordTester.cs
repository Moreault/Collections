namespace ToolBX.Collections.UnitTesting;

/// <summary>
/// Automatically covers test cases for records such as cloning and value equality.
/// </summary>
public abstract class RecordTester<T> : Tester where T : class
{
    [TestMethod]
    public void WhenUsingPrivateCloningConstructor_ThenCloneObject()
    {
        //Arrange
        var instance = Dummy.Create<T>();

        //This constructor for a sealed record is private but it's protected for a non-sealed record
        var constructor = typeof(T).GetSingleConstructor(x => (x.IsPrivate || x.IsProtected()) && x.IsInstance() && x.HasParameters<T>());

        //Act
        var result = (T)constructor.Invoke(new object?[] { instance });

        //Assert
        Assert.IsFalse(ReferenceEquals(instance, result));
    }

    [TestMethod]
    public void Ensure_ValueEquality() => Ensure.ValueEquality<T>(Dummy, JsonSerializerOptions);

    [TestMethod]
    public void Ensure_ConsistentHashCode() => Ensure.ValueHashCode<T>(Dummy, JsonSerializerOptions);
}