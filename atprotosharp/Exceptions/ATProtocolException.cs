namespace atprotosharp.Exceptions
{
    [Serializable]
    public class ATProtocolException : Exception
    {
        public ATProtocolException() { }

        public ATProtocolException(string message)
            : base(message) { }

        public ATProtocolException(string message, Exception inner)
            : base(message, inner) { }
    }
}
