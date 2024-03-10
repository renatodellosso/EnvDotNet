using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnvDotNet.tests;

[TestClass]
public class GlobalTests
{

    // TestInitialize runs before each test
    [TestInitialize]
    public void InitEnv()
    {
        // Generate a temporary file
        string envText = "key1=value1\nkey2=value2",
            filename = Path.GetTempFileName();
        File.WriteAllText(filename, envText);

        // Initialize the environment
        Env.Init(filename);

        File.Delete(filename);
    }

    [TestCleanup]
    public void DisposeEnv()
    {
        if (Env.IsInitted)
            Env.Dispose();
    }

    [TestMethod]
    public void Init()
    {
        // Test the Init method
        Assert.IsTrue(Env.IsInitted);
    }

    [TestMethod]
    public void Get()
    {
        // Test the Get method
        Assert.AreEqual("value1", Env.Get("key1"));
    }

    [TestMethod]
    public void HasKey()
    {
        // Test the ContainsKey method
        Assert.IsTrue(Env.HasKey("key1"));
        Assert.IsFalse(Env.HasKey("key3"));
    }

    [TestMethod]
    public void GetDefault()
    {
        // Test the GetDefault method
        Assert.AreEqual("value1", Env.Get("key1", "not found"));
        Assert.AreEqual("not found", Env.Get("key3", "not found"));
    }

    [TestMethod]
    public void TryGet()
    {
        // Test the TryGet method
        Assert.IsTrue(Env.TryGet("key1", out string? value));
        Assert.AreEqual("value1", value);

        Assert.IsFalse(Env.TryGet("key3", out value));
        Assert.IsNull(value);
    }

    [TestMethod]
    public void Dispose()
    {
        Env.Dispose();
        Assert.ThrowsException<EnvNotInittedException>(() => Env.Get("key1"));
    }

}
