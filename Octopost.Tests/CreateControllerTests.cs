namespace Octopost.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Octopost.Model.ApiResponse.HTTP201;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.UoW;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class CreateControllerTests : OctopostControllerTestBase
    {
        [TestMethod]
        public async Task CreatePost()
        {
            // Arrange
            const string Url = "/api/Posts";
            var postRepository = this.GetService<IUnitOfWorkFactory>().CreateUnitOfWork().CreateEntityRepository<Post>();
            var postDto = new CreatePostDto
            {
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                Latitude = 43,
                Longitude = 8
            };

            // Act
            var response = await this.Client.PostAsync(Url, new StringContent(JsonConvert.SerializeObject(postDto), Encoding.UTF8, "application/json"));
            var result = await response.Content.ReadAsStringAsync();
            var createdMessage = JsonConvert.DeserializeObject<CreatedApiResult>(result);
            var post = postRepository.FindById(createdMessage.CreatedId);

            // Assert
            Assert.IsTrue(createdMessage.Success);
            Assert.AreEqual(createdMessage.Entity, OctopostEntityName.Post);
            Assert.AreEqual(post.Text, postDto.Text);
        }
    }
}
