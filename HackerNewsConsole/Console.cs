using System;
using System.Threading.Tasks;

namespace HackerNewsProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int position = Array.IndexOf(args, "--posts");

            //check if --posts is in arguments
            if(position > -1)
            {
                //Check that there's in integer for number of posts
                if(args[position + 1] != null 
                && int.TryParse(args[position + 1], out int result)){
                    //
                    if(result > 0 && result < 101){
                        var generator = new HackerNews();

                        string jsonToReturn = await generator.GetXNumberOfTopHackerNewsPosts(result);

                        Console.WriteLine(jsonToReturn);
                    }                    
                }                
            } else {
                Console.WriteLine("Please pass in arguments in the format --posts <number of posts you want to see>");
            }

           
        }
    }
}