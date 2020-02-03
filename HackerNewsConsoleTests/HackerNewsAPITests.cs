using System;
using System.IO;
using Xunit;
using HackerNewsProject;

namespace HackerNewsProjectTests
{
    public class HackerNewsAPITests
    {
        [Fact]
        public async void Given_RequestForTopHackerNewsStories_When_RequestIsMade_Then_JsonArrayOfTopStoriesReturned()
        {
            var data = await HackerNewsAPI.GetTopHackerNewsStoryIds();

            Assert.NotEmpty(data);
        }

        [Fact]
        public async void Given_RequestForStoryItem_When_RequestIsMade_Then_StoryItemJsonReturned()
        {
            var data = await HackerNewsAPI.GetHackerNewsStoriesById(8863);

            String expected = "";

            using (StreamReader sr = new StreamReader(@"..\..\..\..\HackerNewsConsoleTests/Resources/id8863.txt"))
            {
            // Read the stream to a string, and write the string to the console.
                expected = sr.ReadToEnd();
            }

            Assert.Equal(expected, data.ToString());
        }

        [Fact]
        public async void Given_IncorrectIdParameter_When_RequestIsMade_Then_EmptyJsonShouldBeReturned()
        {
            var data = await HackerNewsAPI.GetHackerNewsStoriesById(-1);

            String expected = "";

            Assert.Equal(expected, data.ToString());
        }
    }
}
