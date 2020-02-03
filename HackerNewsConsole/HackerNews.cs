using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HackerNewsProject
{
    public class HackerNews
    {
        public async Task<string[]> GetXNumberOfTopHackerNewsPosts(int numberOfPosts){

            var data = await HackerNewsAPI.GetTopHackerNewsStoryIds();

            List<int> storyIds = parseListOfStoryIdsFromJson(data, numberOfPosts);

            string[] storyInfo = await GetStoriesBasedOnStoryIds(storyIds);

            return storyInfo;
        }

        static async Task<string[]> GetStoriesBasedOnStoryIds(List<int> storyIds){
            JObject objectToReturn = new JObject();

            int rank = 0;

            List<Task<string>> listOfTasks = new List<Task<string>>();

            //We want to be doing the async tasks in parallel here because doing it serially would be slow
            //https://medium.com/@t.masonbarneydev/iterating-asynchronously-how-to-use-async-await-with-foreach-in-c-d7e6d21f89fa
            foreach(int id in storyIds){
                rank++;
                listOfTasks.Add(GetStoryBasedOnStoryIdAndRank(id, rank));
            }

            string[] storyItemObjects = await Task.WhenAll<string>(listOfTasks);

            return storyItemObjects;
        }

        static async Task<string> GetStoryBasedOnStoryIdAndRank(int storyId, int rank){

            //Get the story information
            string json = await HackerNewsAPI.GetHackerNewsStoriesById(storyId);           

            JObject storyInfo = (JObject)JsonConvert.DeserializeObject(json);

            StoryInfo hnInfo = new StoryInfo();
            hnInfo.Title = storyInfo.GetValue("title").ToString();
            hnInfo.Uri = storyInfo.GetValue("url").ToString();
            hnInfo.Author = storyInfo.GetValue("by").ToString();
            hnInfo.Points = storyInfo.GetValue("score").ToString();
            hnInfo.comments = storyInfo.GetValue("descendants").ToString();
            hnInfo.rank = rank;

            // Convert DtoryInfo object to JOSN string format   
            string jsonData = JsonConvert.SerializeObject(hnInfo, Formatting.Indented); 

            return jsonData;
        }

        public class StoryInfo{
            public string Title { get; set; }
            public string Uri { get; set; }
            public string Author { get; set; }
            public string Points { get; set; }
            public string comments { get; set; }
            public int rank { get; set; }
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