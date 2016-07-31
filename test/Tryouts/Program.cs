namespace Tryouts
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new TestClient().Test().Wait();
        }
    }
}