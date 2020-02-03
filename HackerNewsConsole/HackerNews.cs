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

            var data = await HackerNewsAPI.GetTopHackerNewsStoryIds();

            List<int> storyIds = parseListOfStoryIdsFromJson(data, numberOfPosts);

            string storyInfo = await GetStoriesBasedOnStoryIds(storyIds);

            return storyInfo;
        }

        static async Task<string> GetStoriesBasedOnStoryIds(List<int> storyIds){
            JObject objectToReturn = new JObject();

            int rank = 0;

            List<Task<StoryInfo>> listOfTasks = new List<Task<StoryInfo>>();

            //We want to be doing the async tasks in parallel here because doing it serially would be slow
            //https://medium.com/@t.masonbarneydev/iterating-asynchronously-how-to-use-async-await-with-foreach-in-c-d7e6d21f89fa
            foreach(int id in storyIds){
                rank++;
                listOfTasks.Add(GetStoryBasedOnStoryIdAndRank(id, rank));
            }

            StoryInfo[] storyItemObjects = await Task.WhenAll<StoryInfo>(listOfTasks);

            string jsonData = JsonConvert.SerializeObject(storyItemObjects, Formatting.Indented); 

            return jsonData;
        }

        static async Task<StoryInfo> GetStoryBasedOnStoryIdAndRank(int storyId, int rank){

            //Get the story information
            string json = await HackerNewsAPI.GetHackerNewsStoriesById(storyId);           

            JObject storyInfo = (JObject)JsonConvert.DeserializeObject(json);

            //Title
            StoryInfo hnInfo = new StoryInfo();
            if(storyInfo.GetValue("title") != null){
                string title = storyInfo.GetValue("title").ToString();

                //Check if title is non-empty string
                if(!String.IsNullOrEmpty(title)){
                    hnInfo.Title = title;
                } else{
                    hnInfo.Title = "No Title Available";
                }                
            }

            //Uri        
            if(storyInfo.GetValue("url") != null){
                hnInfo.Uri = storyInfo.GetValue("url").ToString();

                //Check if URL is valid
                Uri uriResult;
                bool result = Uri.TryCreate(hnInfo.Uri, UriKind.Absolute, out uriResult) 
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                if(!result){
                    hnInfo.Uri = hnInfo.Uri + " (Invalid Uri)";
                }

            }

            //Author
            if(storyInfo.GetValue("by") != null){
                string author = storyInfo.GetValue("by").ToString();

                //Check if Author is non-empty string
                if(!String.IsNullOrEmpty(author)){
                    hnInfo.Author = author;
                } else {
                    hnInfo.Author = "No Author Available";
                }
            }

            //Points
            if(storyInfo.GetValue("score") != null){
                string score = storyInfo.GetValue("score").ToString();

                //Check if greater or equal to zero
                bool isInt = int.TryParse(score, out int intScore);
                if(isInt && intScore >= 0){
                    hnInfo.Points = score;
                } else {
                    hnInfo.Points = "No score available";
                }
                
            }

            //Comments
            if(storyInfo.GetValue("descendants") != null){
                string comments = storyInfo.GetValue("descendants").ToString();

                //Check if greater or equal to zero
                bool isInt = int.TryParse(comments, out int intComments);
                if(isInt && intComments >= 0){
                    hnInfo.comments = comments;
                } else {
                    hnInfo.comments = "No comment info available";
                }
            }
            
            //Rank
            //Check if greater or equal to zero
            if(rank >= 0){
                hnInfo.rank = rank.ToString();
            } else {
                hnInfo.rank = "No Rank Info Available";
            }
            

            // Convert DtoryInfo object to JOSN string format   
            //string jsonData = JsonConvert.SerializeObject(hnInfo, Formatting.Indented); 

            return hnInfo;
        }

        public class StoryInfo{
            public string Title { get; set; }
            public string Uri { get; set; }
            public string Author { get; set; }
            public string Points { get; set; }
            public string comments { get; set; }
            public string rank { get; set; }
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