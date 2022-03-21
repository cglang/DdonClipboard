namespace DdonSocket
{
    public class DdonSocketClientConnections
    {
        private static readonly object _lock = new();

        private readonly Dictionary<Guid, DdonSocketClient> Pairs = new();

        private static DdonSocketClientConnections? ddonSocketClientConnection;

        public static DdonSocketClientConnections GetDdonSocketClientConnection()
        {
            if (ddonSocketClientConnection != null) return ddonSocketClientConnection;

            lock (_lock) ddonSocketClientConnection ??= new DdonSocketClientConnections();

            return ddonSocketClientConnection;
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
