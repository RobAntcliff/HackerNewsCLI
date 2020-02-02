using System;
using Xunit;
using HackerNewsProject;

namespace HackerNewsProjectTests
{
    public class HackerNewsTests
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
        public void Given_GetTopStoriesMethod_When_NumberOfStoriesParamIsZero_Then_ReturnInfoMessage()
        {
            
        }

         [Fact]
        public void Given_GetTopStoriesMethod_When_NumberOfStoriesGreaterThanOneHundred_Then_ReturnInfoMessage()
        {
            
        }

         [Fact]
        public void Given_GetTopStoriesMethod_When_NumberOfStoriesBetween0and100_Then_ReturnExpectedJson()
        {
            
        }

         [Fact]
        public void Given__When__Then_()
        {
            
        }
    }
}
