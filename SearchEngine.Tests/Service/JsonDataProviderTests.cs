using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Newtonsoft.Json;
using SearchEngine.Model;
using SearchEngine.Model.Entity;
using SearchEngine.Model.Enum;
using SearchEngine.Service.Implementation;
using SearchEngine.Service.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SearchEngine.Tests.Service
{
    public class JsonDataProviderTests
    {
        private readonly Mock<IFileContentProvider> _fileContentProviderMock;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly IDataProvider<RootObject> _jsonDataProvider;

        public JsonDataProviderTests()
        {
            _fileContentProviderMock = new Mock<IFileContentProvider>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _jsonDataProvider = new JsonDataProvider<RootObject>(_fileContentProviderMock.Object, _memoryCacheMock.Object);
        }

        [Fact]
        public async Task Should_RetrieveDataFromJson_ThroughCache()
        {
            // Arrange
            var expectedRootObject = new RootObject
            {
                Buildings = new List<Building>(),
                Groups = new List<Group>(),
                Locks = new List<Lock>(),
                Media = new List<Medium>(),
            };
            var rootObjectString = JsonConvert.SerializeObject(expectedRootObject);
            _fileContentProviderMock.Setup(x => x.GetFileContent()).ReturnsAsync(rootObjectString);

            object cachedObject = expectedRootObject;
            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedObject)).Returns(true);

            // Act
            var result = await _jsonDataProvider.GetData();

            // Assert
            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedRootObject);
            result.Buildings.Should().NotBeNull();
            result.Buildings.Should().BeEmpty();
            result.Groups.Should().NotBeNull();
            result.Groups.Should().BeEmpty();
            result.Locks.Should().NotBeNull();
            result.Locks.Should().BeEmpty();
            result.Media.Should().NotBeNull();
            result.Media.Should().BeEmpty();

            _fileContentProviderMock.Verify(x => x.GetFileContent(), Times.Never);

            _memoryCacheMock.Verify(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny), Times.Once);
            _memoryCacheMock.Verify(x => x.TryGetValue(CacheKey.Data, out cachedObject), Times.Once);
            _memoryCacheMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task Should_RetrieveDataFromJson_FromFile()
        {
            // Arrange
            var expectedRootObject = new RootObject
            {
                Buildings = new List<Building>(),
                Groups = new List<Group>(),
                Locks = new List<Lock>(),
                Media = new List<Medium>(),
            };
            var rootObjectString = JsonConvert.SerializeObject(expectedRootObject);
            _fileContentProviderMock.Setup(x => x.GetFileContent()).ReturnsAsync(rootObjectString);

            object cachedObject = expectedRootObject;
            _memoryCacheMock.Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedObject)).Returns(false);

            var cacheEntry = Mock.Of<ICacheEntry>();
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(cacheEntry);

            // Act
            var result = await _jsonDataProvider.GetData();

            // Assert
            using var scope = new AssertionScope();
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedRootObject);
            result.Buildings.Should().NotBeNull();
            result.Buildings.Should().BeEmpty();
            result.Groups.Should().NotBeNull();
            result.Groups.Should().BeEmpty();
            result.Locks.Should().NotBeNull();
            result.Locks.Should().BeEmpty();
            result.Media.Should().NotBeNull();
            result.Media.Should().BeEmpty();

            _fileContentProviderMock.Verify(x => x.GetFileContent(), Times.Once);

            _memoryCacheMock.Verify(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny), Times.Once);
            _memoryCacheMock.Verify(x => x.TryGetValue(CacheKey.Data, out cachedObject), Times.Once);
            _memoryCacheMock.Verify(x => x.CreateEntry(It.IsAny<object>()), Times.Once);
            _memoryCacheMock.Verify(x => x.CreateEntry(CacheKey.Data), Times.Once);
        }
    }
}