using System;
using Xunit;
using HackerNewsProject;

namespace HackerNewsProjectTests
{
    public class HackerNewsTests
    {
        [Fact]
        public async void Given_GetXNumberOfTopHackerNewsPosts_When_NumberOfStoriesParamIsLessThanZero_Then_ReturnInfoMessage()
        {
            int numberOfPosts = -1;

            HackerNews test = new HackerNews();

            string result = await test.GetXNumberOfTopHackerNewsPosts(numberOfPosts);

            Assert.Equal("Number of posts specified is out of bounds. Please enter a value between 0 and 100", result);
        }

        [Fact]
        public async void Given_GetTopStoriesMethod_When_NumberOfStoriesGreaterThanOneHundred_Then_ReturnInfoMessage()
        {
            int numberOfPosts = 101;

            HackerNews test = new HackerNews();

            string result = await test.GetXNumberOfTopHackerNewsPosts(numberOfPosts);

            Assert.Equal("Number of posts specified is out of bounds. Please enter a value between 0 and 100", result);
        }

        [Fact]
        public async void Given_GetTopStoriesMethod_When_NumberOfStoriesBetween0and100_Then_ReturnExpectedJson()
        {
            int numberOfPosts = 1;

            HackerNews test = new HackerNews();

            string result = await test.GetXNumberOfTopHackerNewsPosts(numberOfPosts);

            Assert.NotEqual("Number of posts specified is out of bounds. Please enter a value between 0 and 100", result);
        }

        [Fact]
        public void Given__When__Then_()
        {
            
        }
    }
}
