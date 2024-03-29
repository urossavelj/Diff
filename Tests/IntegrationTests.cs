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

        [Fact]
        public void RightDiffIsMissing()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byB0ZXN0IHNvbWV0aGluZw==");

            var actionResult = controller.Get("1");

            // Assert
            Assert.NotNull(actionResult);
            Assert.IsType<BadRequestResult>(actionResult);
        }

        [Fact]
        public void DiffsAreEqual()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byBmYXJ0IHNvbWV0aGluZwo=");
            controller.Right("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byBmYXJ0IHNvbWV0aGluZwo=");

            var actionResult = controller.Get("1");

            // Assert
            Assert.NotNull(actionResult);
            var res = Assert.IsType<OkObjectResult>(actionResult);
            Assert.True(res.Value.ToString() == "Diffs are equal");
        }

        [Fact]
        public void DiffsNotOfEqualSize()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byBmYXJ0IHNvbWV0aGluZwo=");
            controller.Right("1", "dGhpcyBpcyBqdXN0IG");

            var actionResult = controller.Get("1");

            // Assert
            Assert.NotNull(actionResult);
            var res = Assert.IsType<OkObjectResult>(actionResult);
            Assert.True(res.Value.ToString() == "Input not of equal size");
        }

        [Fact]
        public void CorrectDiffShouldBeReturned()
        {
            // Arrange
            var logger = Mock.Of<ILogger<DiffController>>();
            var cache = new MemoryCache(new MemoryCacheOptions());
            var controller = new DiffController(logger, cache);

            controller.Left("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byBmYXJ0IHNvbWV0aGluZwo=");
            controller.Right("1", "dGhpcyBpcyBqdXN0IGEgdGVzdAp0byB0ZXN0IHNvbWV0aGluZw==");

            var actionResult = controller.Get("1");

            // Assert
            Assert.NotNull(actionResult);
            var res = Assert.IsType<OkObjectResult>(actionResult);
            var dict = res.Value as Dictionary<int, int>;

            Assert.NotNull(dict);
            Assert.True(dict.Count() == 3, "Result does not have 3 differences");
            Assert.True(dict.ContainsKey(31) && dict.ContainsKey(34) && dict.ContainsKey(50));
            Assert.True(dict[31] == 2 && dict[34] == 1 && dict[50] == 1);
        }
    }
}