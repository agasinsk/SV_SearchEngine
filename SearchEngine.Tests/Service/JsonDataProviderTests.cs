using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using SearchEngine.Model;
using SearchEngine.Model.Entity;
using SearchEngine.Service.Implementation;
using SearchEngine.Service.Interface;
using System.Collections.Generic;
using Xunit;

namespace SearchEngine.Tests.Service
{
    public class JsonDataProviderTests
    {
        private readonly Mock<IFileContentProvider> _fileContentProviderMock;
        private readonly IDataProvider<RootObject> _jsonDataProvider;

        public JsonDataProviderTests()
        {
            _fileContentProviderMock = new Mock<IFileContentProvider>();
            _jsonDataProvider = new JsonDataProvider<RootObject>(_fileContentProviderMock.Object);
        }

        [Fact]
        public void Should_RetrieveDataFromJson()
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
            _fileContentProviderMock.Setup(x => x.GetFileContent()).Returns(rootObjectString);

            // Act
            var result = _jsonDataProvider.GetData();

            // Assert
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
        }
    }
}