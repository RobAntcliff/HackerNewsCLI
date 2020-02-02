using System;
using System.Threading.Tasks;

namespace HackerNewsProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var generator = new HackerNewsAPI();

            var data = await HackerNewsAPI.GetTopHackerNewsStories();      
        }
    }
}