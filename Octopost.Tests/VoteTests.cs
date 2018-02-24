namespace Octopost.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Services.Posts;
    using Octopost.Services.UoW;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class VoteTests : OctopostControllerTestBase
    {
        [TestMethod]
        public async Task CheckVote()
        {
            // Arrange
            var postCreationService = this.GetService<IPostCreationService>();
            var unitOfWorkFactory = this.GetService<IUnitOfWorkFactory>();
            var id = postCreationService.CreatePost(new CreatePostDto
            {
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                Latitude = 23,
                Longitude = 8
            });
            var up = $"/api/Posts/{id}/Votes?state=1";
            var neutral = $"/api/Posts/{id}/Votes?state=0";
            var down = $"/api/Posts/{id}/Votes?state=-1";

            // Act
            await this.Client.PostAsync(up, null);
            await this.Client.PostAsync(down, null);
            await this.Client.PostAsync(neutral, null);

            // Assert
            using (var unitOfWork = unitOfWorkFactory.CreateUnitOfWork())
            {
                var voteRepository = unitOfWork.CreateEntityRepository<Vote>();
                var votes = voteRepository.Query().OrderByDescending(x => x.State).ToList();
                Assert.AreEqual(votes.Count, 2);
                Assert.AreEqual(votes[0].State, VoteState.Up);
                Assert.AreEqual(votes[1].State, VoteState.Down);
            }
        }
    }
}
