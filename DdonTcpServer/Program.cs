using DdonSocket;

var host = DdonSocketServer.CreateServer("127.0.0.1", 8888);

host.StringContentHandler(async (head, data) =>
    {
        if (head.Opcode.Equals(DdonSocketOpcode.Authentication))
        {
            // 客户端要获取在线数量
        }
        else if (head.Opcode.Equals(DdonSocketOpcode.Repost))
        {
            // 客户端要转发文本
        }
        var client = host.GetDdonTcpClient(head.SendClientId);
        Console.WriteLine($"客户端:{head} 数据:{data}");
        if (client is not null)
            await client.SendStringAsync(data);
    })
    .FileByteHandler(async (head, fileBytes) =>
    {
        await Task.CompletedTask;
    })
    .StreamHandler(async (head, stream) =>
    {
        await Task.CompletedTask;
    });
host.Start();


