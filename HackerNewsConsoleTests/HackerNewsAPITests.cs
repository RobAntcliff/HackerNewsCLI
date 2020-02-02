using System;
using Xunit;
using HackerNewsProject;

namespace HackerNewsProjectTests
{
    public class HackerNewsAPITests
    {
        [Fact]
        public void Test1()
        {
            var hnAPI = new HackerNewsAPI();
            string expected = "Hello World!";
            string actual = hnAPI.HelloWorld();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Given_RequestForTopHackerNewsStories_When_RequestIsMade_Then_JsonArrayOfTopStoriesReturned()
        {
            var data = HackerNewsAPI.GetTopHackerNewsStories();
        }

        [Fact]
        public void Given_MalformedTopHackerNewsStoryRequest_When_RequestIsMade_Then_EmptyJsonShouldBeReturned()
        {
            
        }

        [Fact]
        public void Given_RequestForStoryItem_When_RequestIsMade_Then_StoryItemJsonReturned()
        {
            
        }

        [Fact]
        public void Given_MalformedStoryItemRequest_When_RequestIsMade_Then_EmptyJsonShouldBeReturned()
        {
            
        }
    }
}
