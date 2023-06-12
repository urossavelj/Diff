using Diff.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private WebApplicationFactory<Program> _factory;

        public IntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;

            // Invoke the app
            _factory.CreateDefaultClient();
        }

        [Fact]
        public void LeftDiffDoesntExist()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            var actionResult = controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byB0ZXN0IHNvbWV0aGluZw==");

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public void RightDiffDoesntExist()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            var actionResult = controller.Right("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byBmYXJ0IHNvbWV0aGluZwo=");

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public void LeftDiffExists()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            var actionResult = controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byB0ZXN0IHNvbWV0aGluZw==");
            var actionResult2 = controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byB0ZXN0IHNvbWV0aGluZw==");

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult2);
            Assert.IsType<OkResult>(actionResult);
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public void RightDiffExists()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            var actionResult = controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byB0ZXN0IHNvbWV0aGluZw==");
            var actionResult2 = controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byB0ZXN0IHNvbWV0aGluZw==");

            // Assert
            Assert.NotNull(actionResult);
            Assert.NotNull(actionResult2);
            Assert.IsType<OkResult>(actionResult);
            Assert.IsType<OkResult>(actionResult2);
        }

        [Fact]
        public void DiffDoesntExistInCache()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            var actionResult = controller.Get("1");

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<NotFoundResult>(actionResult);
        }
    }
}