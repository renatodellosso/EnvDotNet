using System.Collections.Concurrent;

namespace EnvDotNet;

public class Env
{

    private static Env? instance;

    /// <summary>
    /// The dictionary containing the environment variables. Key: name of the variable, Value: value of the variable.
    /// </summary>
    private readonly ConcurrentDictionary<string, string> dict;

    /// <summary>
    /// Returns whether the environment has been <b>globally</b> initialized. Non-global environments will not cause this property to return true.
    /// </summary>
    public static bool IsInitted => instance != null;

    /// <summary>
    /// Creates local environment. Local environments are not accessible through the static methods.
    /// </summary>
    /// <param name="filename">The name of the environment file or the path to it, if it is not in the current directory. Defaults to ".env".</param>
    public Env(string filename = ".env") : this(filename, false) { }

    /// <summary>
    /// Creates a new environment from the given file.
    /// </summary>
    /// <param name="filename">The name of the environment file or the path to it, if it is not in the current directory. Defaults to ".env".</param>
    /// <param name="global">Whether to set <see cref="instance"/> to the new environment. If true, static methods will be available.</param>
    private Env(string filename, bool global)
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

    /// <summary>
    /// Creates a global environment from the given file. Static methods will be available.
    /// </summary>
    /// <param name="filename">The name of the env file or the path to it, if it is not in the current directory. Defaults to ".env".</param>
    public static void Init(string filename = ".env") => _ = new Env(filename, true);

    /// <summary>
    /// Disposes the global environment. Static methods will no longer be available.
    /// </summary>
    public static void Dispose()
    {
        instance = null;
    }

    /// <returns>Whether the environment contains the given <see cref="key"/>.</returns>
    public bool ContainsKey(string key) => dict.ContainsKey(key);

    /// <summary>
    /// Retrieves an environment variable.
    /// </summary>
    /// <param name="key">The name of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public string this[string key] => dict[key];
    /// <summary>
    /// Retrieves an environment variable, but returns a default value if the variable is not found.
    /// </summary>
    /// <param name="key">The name of the environment variable.</param>
    /// <param name="defaultValue">The value to return if the <see cref="key"/> is not found.</param>
    /// <returns>The value of the environment variable if the key is present, or the <see cref="defaultValue"/> if the key is not present.</returns>
    public string this[string key, string defaultValue] => dict.GetValueOrDefault(key, defaultValue);

    /// <returns>Whether the environment contains the given <see cref="key"/>.</returns>
    /// <exception cref="EnvNotInittedException"></exception>
    public static bool HasKey(string key) => instance?.dict.ContainsKey(key) ?? throw new EnvNotInittedException();

    /// <summary>
    /// Retrieves an environment variable.
    /// </summary>
    /// <param name="key">The name of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    /// <exception cref="EnvNotInittedException"></exception>
    public static string Get(string key) => instance?[key] ?? throw new EnvNotInittedException();

    /// <summary>
    /// Retrieves an environment variable, but returns a default value if the variable is not found.
    /// </summary>
    /// <param name="key">The name of the environment variable.</param>
    /// <param name="defaultValue">The value to return if the <see cref="key"/> is not found.</param>
    /// <returns>The value of the environment variable if the key is present, or the <see cref="defaultValue"/> if the key is not present.</returns>
    /// <exception cref="EnvNotInittedException"></exception>
    public static string Get(string key, string defaultValue)
    {
        if (instance == null)
            throw new EnvNotInittedException();

        return instance.dict.GetValueOrDefault(key, defaultValue);
    }

    /// <summary>
    /// Attempts to retrieve an environment variable.
    /// </summary>
    /// <param name="key">The name of the environment variable.</param>
    /// <param name="value">The variable to write the value to.</param>
    /// <returns>Whether the key was successfully retrieved.</returns>
    /// <exception cref="EnvNotInittedException"></exception>
    public static bool TryGet(string key, out string? value) => instance?.dict.TryGetValue(key, out value) ?? throw new EnvNotInittedException();
}
