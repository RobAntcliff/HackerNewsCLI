using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HackerNewsProject
{
    public class HackerNewsAPI
    {
        /// <summary>Gets a json list of the top hacker news story Ids</summary>
        /// <returns>a json string with the list of Ids in order</returns>
        public static async Task<string> GetTopHackerNewsStoryIds()
        {
            var httpClient = HttpClientFactory.Create();

             string data = "";

             try{
                 HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/topstories.json");
                 httpResponseMessage.EnsureSuccessStatusCode();
                 var content = httpResponseMessage.Content;
                 data = await content.ReadAsStringAsync();
             }
             catch(HttpRequestException e)
             {
                 System.Console.WriteLine("\nException Caught!");	
                 System.Console.WriteLine("Message :{0} ",e.Message);
             }

             return data;
        }

        /// <summary>Gets one specific story from Hacker News</summary>
        /// <param name="storyId">id of the story we want to get</param>
        /// <returns>string containing the json information for the story</returns>
        public static async Task<string> GetHackerNewsStoriesById(int storyId)
        {
            var httpClient = HttpClientFactory.Create();

             string data = "";

             if(storyId >= 0)
             {
                try{
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/item/"
                    + storyId.ToString()
                    + ".json");
                    httpResponseMessage.EnsureSuccessStatusCode();
                    var content = httpResponseMessage.Content;
                    data = await content.ReadAsStringAsync();
                }
                catch(HttpRequestException e)
                {
                    System.Console.WriteLine("\nException Caught!");	
                    System.Console.WriteLine("Message :{0} ",e.Message);
                }
             }             

             return data;
        }
    }
}