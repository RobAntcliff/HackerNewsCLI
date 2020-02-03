using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HackerNewsProject
{
    public class HackerNews
    {
        /// <summary>Get the top posts from hackernews.com in json string format</summary>
        /// <param name="numberOfPosts">Number of posts you want to get (0 > posts < 101)</param>
        /// <returns>a string containing the number of posts specified in a json format</returns>
        public async Task<string> GetXNumberOfTopHackerNewsPosts(int numberOfPosts){

            string storyInfo = "";

            if(numberOfPosts > 0 && numberOfPosts < 101){
                var data = await HackerNewsAPI.GetTopHackerNewsStoryIds();

                List<int> storyIds = parseListOfStoryIdsFromJson(data, numberOfPosts);

                storyInfo = await GetStoriesBasedOnStoryIds(storyIds);
            } else {
                storyInfo = "Number of posts specified is out of bounds. Please enter a value between 0 and 100";
            }         

            return storyInfo;
        }

        /// <summary>Get posts from hn based on a list of post Ids</summary>
        /// <param name="storyIds">List of post Ids</param>
        /// <returns>a string containing the posts specified in a json format</returns>
        static async Task<string> GetStoriesBasedOnStoryIds(List<int> storyIds){
            JObject objectToReturn = new JObject();

            //Will use this variable to add the rank to the story objects
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

        /// <summary>Get a single story from hacker news based on the id</summary>
        /// <param name="storyIds">hacker news post Id</param>
        /// <param name="rank">the rank at which the story is on hacker news</param>
        /// <returns>A StoryInfo object with fields containing info about the story</returns>
        static async Task<StoryInfo> GetStoryBasedOnStoryIdAndRank(int storyId, int rank){

            //Get the story information
            string json = await HackerNewsAPI.GetHackerNewsStoriesById(storyId);           

            JObject storyInfo = (JObject)JsonConvert.DeserializeObject(json);

            //Title
            StoryInfo hnInfo = new StoryInfo();
            if(storyInfo.GetValue("title") != null){
                string title = storyInfo.GetValue("title").ToString();

                //Check if title is non-empty string
                if(!String.IsNullOrEmpty(title) && title.Length < 257){
                    hnInfo.Title = title;
                } else if(title.Length > 256){
                    hnInfo.Title = "Title too long";
                } else {
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
                if(!String.IsNullOrEmpty(author) && author.Length < 257){
                    hnInfo.Author = author;
                } else if (author.Length > 257) {
                    hnInfo.Author = "Author name too long";
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

        /// <summary>Object representing a hacker news story</summary>
        public class StoryInfo{
            public string Title { get; set; }
            public string Uri { get; set; }
            public string Author { get; set; }
            public string Points { get; set; }
            public string comments { get; set; }
            public string rank { get; set; }
        }

        /// <summary>Makes a list of Hacker News story Id the length of however many the user has specified</summary>
        /// <param name="json">json list of hacker news post Ids</param>
        /// <param name="numberOfIdsToParse">the number of stories that the user has specified for the program to return</param>
        /// <returns>A list of rank order hacker news story ids with the same number that the user requested</returns>
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