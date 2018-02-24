namespace Octopost.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Services.Posts;
    using Octopost.Services.UoW;
    using Octopost.Services.Votes;
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    [TestClass]
    public class FilterTests : OctopostControllerTestBase
    {
        [TestMethod]
        public async Task PostsByTags()
        {
            // Arrange
            const string WrittenWorkTag = "WrittenWork";
            const string NaturalPlaceTag = "NaturalPlace";
            const string CompanyTag = "Company";
            using (var unitOfWork = this.GetService<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var postRepository = unitOfWork.CreateEntityRepository<Post>();
                var locationNameRepository = unitOfWork.CreateEntityRepository<LocationName>();
                var locationName = new LocationName { Latitude = 43, Longitude = 8, Name = "Switzerland" };
                locationNameRepository.Create(locationName);
                unitOfWork.Save();
                for (var i = 0; i < 10; i++)
                {
                    postRepository.Create(new Post { Topic = WrittenWorkTag, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Latitude = 43, Longitude = 8, LocationNameId = locationName.Id });
                    postRepository.Create(new Post { Topic = NaturalPlaceTag, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Latitude = 43, Longitude = 8, LocationNameId = locationName.Id });
                    postRepository.Create(new Post { Topic = CompanyTag, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Latitude = 43, Longitude = 8, LocationNameId = locationName.Id });
                }

                unitOfWork.Save();
            }

            const string Url = "/api/Posts/Tags?page=0&pageSize=100";
            var writtenWork = string.Concat(Url, "&tags=", WrittenWorkTag);
            var naturalPlace = string.Concat(Url, "&tags=", NaturalPlaceTag);
            var both = string.Concat(Url, "&tags=", WrittenWorkTag, ",", NaturalPlaceTag);
            var invalid = string.Concat(Url, "&tags=Invalid");

            // Act
            var response = await this.Client.GetAsync(writtenWork);
            var content = await response.Content.ReadAsStringAsync();
            var writtenWorkPosts = JsonConvert.DeserializeObject<PostDto[]>(content);

            response = await this.Client.GetAsync(naturalPlace);
            content = await response.Content.ReadAsStringAsync();
            var naturalPlacePosts = JsonConvert.DeserializeObject<PostDto[]>(content);

            response = await this.Client.GetAsync(both);
            content = await response.Content.ReadAsStringAsync();
            var bothPosts = JsonConvert.DeserializeObject<PostDto[]>(content);

            response = await this.Client.GetAsync(invalid);
            content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.IsTrue(writtenWorkPosts.All(x => x.Topic == WrittenWorkTag));
            Assert.IsTrue(naturalPlacePosts.All(x => x.Topic == NaturalPlaceTag));
            Assert.IsTrue(writtenWorkPosts.All(x => x.Topic == WrittenWorkTag));
            Assert.IsTrue(bothPosts.All(x => x.Topic == WrittenWorkTag || x.Topic == NaturalPlaceTag));
            Assert.IsTrue(content.Contains("INVALID_TAG_ID"));
        }

        [TestMethod]
        public async Task PostsByVotes()
        {
            // Arrange
            const string Url = "/api/Posts/Votes?page=0&pageSize=100";
            var postCreationService = this.GetService<IPostCreationService>();
            var postVoteService = this.GetService<IVoteService>();
            var firstId = postCreationService.CreatePost(new CreatePostDto { Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.", Latitude = 43, Longitude = 8 });
            var secondId = postCreationService.CreatePost(new CreatePostDto { Text = "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.", Latitude = 43, Longitude = 8 });
            postVoteService.Vote(firstId, VoteState.Up);
            postVoteService.Vote(firstId, VoteState.Down);
            postVoteService.Vote(firstId, VoteState.Up);
            postVoteService.Vote(secondId, VoteState.Down);
            postVoteService.Vote(secondId, VoteState.Down);
            postVoteService.Vote(secondId, VoteState.Down);

            // Act
            var response = await this.Client.GetAsync(Url);
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<PostDto[]>(content);

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(posts.Length, 2);
            Assert.AreEqual(posts[0].Id, firstId);
            Assert.AreEqual(posts[1].Id, secondId);
            Assert.AreEqual(posts[0].VoteCount, 1);
            Assert.AreEqual(posts[1].VoteCount, -3);
        }

        [TestMethod]
        public async Task PostsByNewest()
        {
            // Arrange
            const string Url = "/api/Posts/Newest?page=0&pageSize=100";
            var postCreationService = this.GetService<IPostCreationService>();
            var firstId = postCreationService.CreatePost(new CreatePostDto { Latitude = 43, Longitude = 8, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
            var secondId = postCreationService.CreatePost(new CreatePostDto { Latitude = 43, Longitude = 8, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });

            // Act
            var response = await this.Client.GetAsync(Url);
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<PostDto[]>(content);

            // Assert
            Assert.IsTrue(posts[0].Created > posts[1].Created);
        }

        [TestMethod]
        public async Task PostsByDate()
        {
            // Arrange
            var fromStr = new DateTime(2018, 01, 01).ToString("yyyy-MM-dd");
            var toStr = new DateTime(2019, 01, 01).ToString("yyyy-MM-dd");
            var url = $"/api/Posts/Date?from={fromStr}&to={toStr}&page=0&pageSize=100";
            var postCreationService = this.GetService<IPostCreationService>();
            var firstId = postCreationService.CreatePost(new CreatePostDto { Latitude = 43, Longitude = 8, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
            var secondId = postCreationService.CreatePost(new CreatePostDto { Latitude = 43, Longitude = 8, Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua." });
            var unitOfWorkFactory = this.GetService<IUnitOfWorkFactory>();
            using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
            {
                var postRepository = unitOfWork.CreateEntityRepository<Post>();
                var post = postRepository.FindById(firstId);
                post.Created = new DateTime(2018, 12, 21);
                postRepository.Update(post);
                var secondPost = postRepository.FindById(secondId);
                secondPost.Created = new DateTime(2017, 01, 01);
                postRepository.Update(secondPost);
                unitOfWork.Save();
            }

            // Act
            var response = await this.Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<PostDto[]>(content);

            // Assert
            Assert.AreEqual(1, posts.Count());
            Assert.AreEqual(posts.Single().Id, firstId);
        }

        [TestMethod]
        public async Task PostsByQuery()
        {
            // Arrange
            const string Query = "Test";
            var url = $"/api/Posts/Query?query={Query}&page=0&pageSize=100";
            var postCreationService = this.GetService<IPostCreationService>();
            var voteService = this.GetService<IVoteService>();
            var firstId = postCreationService.CreatePost(new CreatePostDto { Latitude = 43, Longitude = 8, Text = "Test Test Test Test Test Test Test" });
            var secondId = postCreationService.CreatePost(new CreatePostDto { Latitude = 43, Longitude = 8, Text = "Another Test Another Test Another Test Another Test Another Test" });
            var thirdId = postCreationService.CreatePost(new CreatePostDto { Latitude = 43, Longitude = 8, Text = "not queried not queried not queried not queried not queried" });
            voteService.Vote(firstId, VoteState.Up);

            // Act
            var response = await this.Client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var posts = JsonConvert.DeserializeObject<PostDto[]>(content);

            // Assert
            Assert.AreEqual(2, posts.Count());
            Assert.AreEqual(posts[0].Id, firstId);
            Assert.AreEqual(posts[1].Id, secondId);
        }
    }
}
