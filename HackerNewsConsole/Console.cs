using System;
using System.Threading.Tasks;

namespace HackerNewsProject
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string errorResponse = "Please pass in arguments in the format --posts n. Where n is how many posts to print. A positive integer <= 100.";

            int position = Array.IndexOf(args, "--posts");

            //check if --posts is in arguments
            if(position > -1)
            {
                //Check that there's in integer for number of posts
                if(args.Length > 1) {
                    if(args[position + 1] != null
                    && int.TryParse(args[position + 1], out int result)){
                        //Make sure integer is within bounds
                        if(result > 0 && result < 101){
                            var generator = new HackerNews();

                            string jsonToReturn = await generator.GetXNumberOfTopHackerNewsPosts(result);

                            Console.WriteLine(jsonToReturn);
                        } else {
                            Console.WriteLine(errorResponse);
                        }                      
                    } else {
                        Console.WriteLine(errorResponse);
                    }  
                } else {
                    Console.WriteLine(errorResponse);
                }          
            } else {
                Console.WriteLine(errorResponse);
            }           
        }
    }
}