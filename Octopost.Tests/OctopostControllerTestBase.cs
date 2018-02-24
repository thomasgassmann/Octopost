namespace Octopost.Tests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Octopost.Model.Data;
    using Octopost.Services;
    using Octopost.Services.UoW;
    using Octopost.WebApi;
    using System.IO;
    using System.Net.Http;

    [TestClass]
    public class OctopostControllerTestBase
    {
        private HttpClient httpClient;

        public HttpClient Client => this.httpClient;

        [TestInitialize]
        public void Initialize()
        {
            var server = this.CreateTestServer();
            this.httpClient = server.CreateClient();
            this.EmptyDatabase();
        }

        public T GetService<T>() => ServiceLocator.Instance.GetService<T>();

        private TestServer CreateTestServer()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.Testing.json");
                    config.AddEnvironmentVariables();
                }));
            return server;
        }

        private void EmptyDatabase()
        {
            using (var unitOfWork = this.GetService<IUnitOfWorkFactory>().CreateUnitOfWork())
            {
                var voteRepository = unitOfWork.CreateEntityRepository<Vote>();
                var postRepository = unitOfWork.CreateEntityRepository<Post>();
                var locationNameRepository = unitOfWork.CreateEntityRepository<LocationName>();
                voteRepository.Delete(x => true);
                postRepository.Delete(x => true);
                locationNameRepository.Delete(x => true);
                unitOfWork.Save();
            }
        }
    }
}
