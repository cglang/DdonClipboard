namespace DdonSocket
{
    public class Test
    {
        private readonly Data data;

        public Test(Data data)
        {
            this.data = data;
        }

        public static Data Method(Data data)
        {
            Console.WriteLine("调用成功！参数.Name:" + data.Name);
            data.Age++;
            return data;
        }

        public async Task<Data> MethodAsync(Data data)
        {
            await Task.CompletedTask;
            await Task.Delay(500);
            Console.WriteLine("调用成功！参数.Name:" + data.Name);
            this.data.Age++;
            return this.data;
        }
    }

    public class Data
    {
        public int Age { get; set; }

        public string? Name { get; set; }
    }
}
