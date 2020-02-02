using System;
using System.Threading.Tasks;

namespace HackerNewsProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var generator = new HackerNews();

            string[] jsonToReturn = await generator.GetXNumberOfTopHackerNewsPosts(20);

            Array.ForEach(jsonToReturn, Console.WriteLine);
        }
    }
}