using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace HackerNewsProject
{
    public class HackerNews
    {
        public async Task<string> GetXNumberOfTopHackerNewsPosts(int numberOfPosts){
            string JsonToReturn = "";

            var data = await HackerNewsAPI.GetTopHackerNewsStoryIds();

            List<int> storyIds = parseListOfStoryIdsFromJson(data, 100);

            return JsonToReturn;
        }

        static List<int> parseListOfStoryIdsFromJson(string json, int numberOfIdsToParse){
            JArray idsJsonArray = JArray.Parse(json);           

            //If there's less item ids than we actually want 
            //to see then we can only show that many items
            if(idsJsonArray.Count < numberOfIdsToParse){
                numberOfIdsToParse = idsJsonArray.Count;
            }

            List<int> itemIds = new List<int>();

            //counter to break out of for each when we've gotten as many 
            int counter = numberOfIdsToParse;

            //Go through the array of item Ids and add to list 
            foreach(var item in idsJsonArray)
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
                
                counter--;
                //Break out of loop if we've gotten the amount of Ids that we need
                if(counter <= 0){
                    break;
                }
            }

            return itemIds;
        }
    }
}