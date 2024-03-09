namespace EnvDotNet
{
    public class EnvNotInittedException : Exception
    {

        public EnvNotInittedException() : base("Env not initialized")
        {
        }

        public EnvNotInittedException(string message) : base(message)
        {
        }

        public EnvNotInittedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
