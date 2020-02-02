using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace HackerNewsProject
{
    public class HackerNewsAPI
    {       
        public string HelloWorld()
        {
            return "Hello World!";
        }
        public static async Task<string> GetTopHackerNewsStories()
        {
            //string url = "https://hacker-news.firebaseio.com/v0/topstories.json";
            //string urlParameters = "?print=pretty/";

            var httpClient = HttpClientFactory.Create();

             string data = "";

             try{
                 HttpResponseMessage httpResponseMessage = await httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/topstories.json");
                 httpResponseMessage.EnsureSuccessStatusCode();
                 var content = httpResponseMessage.Content;
                 data = await content.ReadAsStringAsync();
                 System.Console.WriteLine("Got into here");
             }
             catch(HttpRequestException e)
             {
                 System.Console.WriteLine("\nException Caught!");	
                 System.Console.WriteLine("Message :{0} ",e.Message);
             }

             return data;

        }
    }
}