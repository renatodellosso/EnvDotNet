using System.Collections.Concurrent;

namespace EnvDotNet
{
    public class Env : IDisposable
    {

        private static Env? instance;

        private ConcurrentDictionary<string, string> env;

        public Env(string filename, bool global = true)
        {
            if (global)
            {
                instance?.Dispose();
                instance = this;
            }

            env = new ConcurrentDictionary<string, string>();

            // Load the file line by line
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                // Split the line into key and value
                string[] parts = line.Split('=', 2);
                if (parts.Length == 2)
                {
                    // Add the key and value to the dictionary
                    env[parts[0]] = parts[1];
                }
            }
        }

        public void Dispose()
        {
            instance = null;
        }

        public bool ContainsKey(string key) => env.ContainsKey(key);

        public string this[string key] => env[key];

        public static bool HasKey(string key) => instance?.env.ContainsKey(key) ?? throw new EnvNotInittedException();

        public static string Get(string key) => instance?[key] ?? throw new EnvNotInittedException();

        public static string Get(string key, string defaultValue)
        {
            if (instance == null)
                throw new EnvNotInittedException();

            return instance.env.GetValueOrDefault(key, defaultValue);
        }

        public static bool TryGet(string key, out string? value) => instance?.env.TryGetValue(key, out value) ?? throw new EnvNotInittedException();
    }
}
