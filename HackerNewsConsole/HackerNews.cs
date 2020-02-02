using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HackerNewsProject
{
    public class HackerNews
    {
        public async Task<string> GetXNumberOfTopHackerNewsPosts(int numberOfPosts){
            string JsonToReturn = "";

            var data = await HackerNewsAPI.GetTopHackerNewsStoryIds();

            List<int> storyIds = parseListOfStoryIdsFromJson(data, 100);

            //foreach(int id in storyIds){
            //    HackerNewsAPI.GetHackerNewsStoriesById(id);
           // }

            return JsonToReturn;
        }

        static List<int> parseListOfStoryIdsFromJson(string json, int numberOfIdsToParse){
            JArray idsJsonArray = JArray.Parse(json);   

            var list = JsonConvert.DeserializeObject<List<String>>(json);

            //If there's less item ids than we actually want 
            //to see then we can only show that many items
            if(list.Count > numberOfIdsToParse){
                list.RemoveRange(numberOfIdsToParse, list.Count - numberOfIdsToParse);
            }

            List<int> itemIds = new List<int>();

            //Go through the array of item Ids and add to list 
            foreach(var item in list)
            {
                int number;
                string itemTemp = item.ToString();
                bool success = int.TryParse(itemTemp, out number);
                if(success){
                    itemIds.Add(number);
                } else {
                    //Throw an exception if the parsing goes wrong because JSON is probably wrong then
                    throw new System.ArgumentException("Something went wrong in Parsing the itemIds. Probably Json is malformed", json);
                }
            }

            return itemIds;
        }
    }
}