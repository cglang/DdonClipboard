namespace DdonSocket
{
    public class DdonSocketClientConnectionFactory
    {
        private static readonly object _lock = new();

        private readonly Dictionary<Guid, DdonSocketClient> Pairs = new();

        private static DdonSocketClientConnectionFactory? ddonSocketClientConnectionFactory;

        public static DdonSocketClientConnectionFactory GetDdonSocketClientConnectionFactory()
        {
            lock (_lock)
            {
                if (ddonSocketClientConnectionFactory == null)
                    ddonSocketClientConnectionFactory = new DdonSocketClientConnectionFactory();
            }
            return ddonSocketClientConnectionFactory;
        }

        public DdonSocketClient? GetClient(Guid clientId)
        {
            return Pairs.ContainsKey(clientId) ? Pairs[clientId] : null;
        }

        public void Add(DdonSocketClient client)
        {
            Pairs.Add(client.ClientId, client);
        }

        public void Remove(Guid clientId)
        {
            if (Pairs.ContainsKey(clientId))
                Pairs.Remove(clientId);
        }
    }
}
