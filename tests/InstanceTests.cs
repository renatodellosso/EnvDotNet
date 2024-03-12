using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnvDotNet.tests;

[TestClass]
public class InstanceTests
{

    private readonly Env env;

    public InstanceTests()
    {
        // Dispose the global environment if it exists
        try { Env.Dispose(); } catch (EnvNotInittedException) { }

        // Generate a temporary file
        string envText = "key1=value";
        string filename = Path.GetTempFileName();
        File.WriteAllText(filename, envText);

        // Initialize the environment
        env = new Env(filename);

        File.Delete(filename);
    }

    [TestMethod]
    public void Get()
    {
        // Test the Get method
        Assert.AreEqual("value", env["key1"]);
    }

    [TestMethod]
    public void HasKey()
    {
        // Test the ContainsKey method
        Assert.IsTrue(env.ContainsKey("key1"));
        Assert.IsFalse(env.ContainsKey("key2"));
    }

    [TestMethod]
    public void GetDefault()
    {
        // Test the GetDefault method
        Assert.AreEqual("value", env["key1"]);
        Assert.AreEqual("not found", env["key2", "not found"]);
    }

    [TestMethod]
    public void HasKeyGlobal()
    {
        Assert.ThrowsException<EnvNotInittedException>(() => Env.ContainsKey("key1"));
    }

    [TestMethod]
    public void GetGlobal()
    {
        Assert.ThrowsException<EnvNotInittedException>(() => Env.Get("key1"));
    }

    [TestMethod]
    public void GetDefaultGlobal()
    {
        Assert.ThrowsException<EnvNotInittedException>(() => Env.Get("key1", "not found"));
    }

    [TestMethod]
    public void TryGetGlobal()
    {
        Assert.ThrowsException<EnvNotInittedException>(() => Env.TryGet("key1", out _));
    }
}