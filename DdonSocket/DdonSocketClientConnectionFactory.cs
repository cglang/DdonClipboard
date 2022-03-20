namespace DdonSocket
{
    public class DdonSocketClientConnectionFactory
    {
        private static readonly Dictionary<Guid, DdonSocketClient> Pairs = new();

        public static DdonSocketClient? GetDdonTcpClient(Guid clientId)
        {
            return Pairs.ContainsKey(clientId) ? Pairs[clientId] : null;
        }

        public static void Add(DdonSocketClient client)
        {
            Pairs.Add(client.ClientId, client);
        }

        public static void Remove(Guid clientId)
        {
            if (Pairs.ContainsKey(clientId))
                Pairs.Remove(clientId);
        }
    }
}
