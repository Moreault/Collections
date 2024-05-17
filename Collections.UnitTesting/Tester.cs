namespace ToolBX.Collections.UnitTesting;

public abstract class Tester
{
    protected Dummy Dummy { get; private set; } = null!;

    protected Ensure Ensure { get; private set; } = null!;

    protected JsonSerializerOptions JsonSerializerOptions => _jsonSerializerOptions.Value;
    private Lazy<JsonSerializerOptions> _jsonSerializerOptions = null!;

    [TestInitialize]
    public void TestInitializeBase()
    {
        Dummy = new Dummy();
        Ensure = new Ensure();
        _jsonSerializerOptions = new(() => new JsonSerializerOptions());
        //TODO This should be in testinitialize even after we start using WJS.TestTools
        Dummy.WithCollectionCustomizations();
        InitializeTest();
    }

    protected virtual void InitializeTest()
    {

    }

    //Named as such to avoid unintentional shadowing
    [TestCleanup]
    public void TestCleanupOnBaseClass()
    {
        CleanupTest();
    }

    /// <summary>
    /// Runs after each test.
    /// </summary>
    protected virtual void CleanupTest()
    {

    }

    protected TValue? GetFieldValue<TInstance, TValue>(TInstance instance, string fieldName)
    {
        var fieldInfo = typeof(TInstance).GetSingleField(fieldName);
        return (TValue?)fieldInfo.GetValue(instance);
    }
}

public abstract class Tester<T> : Tester where T : class
{
    private readonly IDictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();

    /// <summary>
    /// Parameters that were used to instantiate <see cref="Instance"/>.
    /// </summary>
    protected IReadOnlyList<object> ConstructorParameters => _constructorParameters;
    private readonly List<object> _constructorParameters = new();

    private readonly List<object> _overridenConstructorParameters = new();

    /// <summary>
    /// Instance of the class that is being tested.
    /// </summary>
    protected T Instance => _instance.Value;
    private Lazy<T> _instance = null!;

    protected Tester()
    {
        ResetInstance();
    }

    private void ResetInstance()
    {
        _instance = new Lazy<T>(() =>
        {
            var instance = InstanceProvider.Create<T>(new DummyWrapper(Dummy), _overridenConstructorParameters, (IReadOnlyDictionary<Type, Mock>)_mocks);

            foreach (var mock in instance.Mocks)
            {
                if (_mocks.ContainsKey(mock.Key)) continue;
                _mocks[mock.Key] = mock.Value;
            }

            return instance.Value;
        });
    }

    protected override void CleanupTest()
    {
        base.CleanupTest();
        _mocks.Clear();
        _constructorParameters.Clear();
        _overridenConstructorParameters.Clear();
        ResetInstance();
    }

    protected Mock<TMock> GetMock<TMock>() where TMock : class
    {
        if (!_mocks.ContainsKey(typeof(TMock)))
            AddMock(typeof(TMock));
        return (Mock<TMock>)_mocks[typeof(TMock)];
    }

    private void AddMock(Type type)
    {
        var typeArgs = new[] { type };
        var mockType = typeof(Mock<>);
        var constructed = mockType.MakeGenericType(typeArgs);
        _mocks[type] = (Activator.CreateInstance(constructed) as Mock)!;
    }

    /// <summary>
    /// Returns field value by name on <see cref="Instance"/>.
    /// </summary>
    protected TValue? GetFieldValue<TValue>(string fieldName) => GetFieldValue<T, TValue>(Instance, fieldName);
}
