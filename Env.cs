using System.Collections.Concurrent;

namespace EnvDotNet;

public class Env
{

    private static Env? instance;

    private readonly ConcurrentDictionary<string, string> dict;

    public static bool IsInitted => instance != null;

    public Env(string filename, bool global = true)
    {
        if (global)
            instance = this;

        dict = new ConcurrentDictionary<string, string>();

        // Load the file line by line
        string[] lines = File.ReadAllLines(filename);
        foreach (string line in lines)
        {
            // Split the line into key and value
            string[] parts = line.Split('=', 2);
            if (parts.Length == 2)
            {
                // Add the key and value to the dictionary
                dict[parts[0]] = parts[1];
            }
        }
    }

    public static void Init(string filename) => _ = new Env(filename);

    public static void Dispose()
    {
        instance = null;
    }

    public bool ContainsKey(string key) => dict.ContainsKey(key);

    public string this[string key] => dict[key];
    public string this[string key, string defaultValue] => dict.GetValueOrDefault(key, defaultValue);

    public static bool HasKey(string key) => instance?.dict.ContainsKey(key) ?? throw new EnvNotInittedException();

    public static string Get(string key) => instance?[key] ?? throw new EnvNotInittedException();

    public static string Get(string key, string defaultValue)
    {
        if (instance == null)
            throw new EnvNotInittedException();

        return instance.dict.GetValueOrDefault(key, defaultValue);
    }

    public static bool TryGet(string key, out string? value) => instance?.dict.TryGetValue(key, out value) ?? throw new EnvNotInittedException();
}
