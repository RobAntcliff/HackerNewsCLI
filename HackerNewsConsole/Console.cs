using System;
using System.Threading.Tasks;

namespace HackerNewsProject
{
    class Console
    {
        static async Task Main(string[] args)
        {
            var generator = new HackerNewsAPI();

            var data = await HackerNewsAPI.GetTopHackerNewsStories();

            System.Console.WriteLine(data);  

            string toPrint = generator.HelloWorld();

            System.Console.WriteLine(toPrint); 

            System.Console.WriteLine("Hello World2!");         
        }
    }
}