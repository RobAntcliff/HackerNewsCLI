using System;
using System.Threading.Tasks;

namespace HackerNewsProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var generator = new HackerNews();

            var jsonToReturn = await generator.GetXNumberOfTopHackerNewsPosts(10);
        }
    }
}