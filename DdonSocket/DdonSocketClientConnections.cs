namespace DdonSocket
{
    public class DdonSocketClientConnections
    {
        private static readonly object _lock = new();

        private readonly Dictionary<Guid, DdonSocketClient> Pairs = new();

        private static DdonSocketClientConnections? ddonSocketClientConnectionFactory;

        public static DdonSocketClientConnections GetDdonSocketClientConnectionFactory()
        {
            lock (_lock)
            {
                if (ddonSocketClientConnectionFactory == null)
                    ddonSocketClientConnectionFactory = new DdonSocketClientConnections();
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
