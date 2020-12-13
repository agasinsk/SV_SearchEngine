using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SearchEngine.Model.DTO;
using SearchEngine.Model.Entity;
using SearchEngine.Model.Enum;
using SearchEngine.Model.Interface;
using SearchEngine.Service.Configuration;
using SearchEngine.Service.Implementation;
using SearchEngine.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SearchEngine.Tests.Service
{
    public class SearchServiceTests
    {
        private readonly Mapper _mapper;
        private readonly Mock<IDataProvider<RootObject>> _dataProviderMock;
        private readonly Mock<ISearchEvaluator> _searchEvaluatorMock;
        private readonly ISearchService _searchService;

        public SearchServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new SearchProfile())));
            _dataProviderMock = new Mock<IDataProvider<RootObject>>();
            _searchEvaluatorMock = new Mock<ISearchEvaluator>();
            _searchService = new SearchService(_dataProviderMock.Object, _mapper, _searchEvaluatorMock.Object);
        }

        [Fact]
        public async Task Should_GetExpectedBuildingAndLocks_ForGivenSearchString()
        {
            // Arrange
            var searchString = "head";
            var data = GetMockedData();

            _dataProviderMock.Setup(p => p.GetData()).ReturnsAsync(data);
            _searchEvaluatorMock.Setup(p => p.Evaluate(data.Buildings.First(), searchString)).Returns((95, 80));

            var expectedFirstResult = data.Buildings.FirstOrDefault(x => x.Name.ToLower().Contains(searchString.ToLower()));
            var expectedSecondResult = data.Locks.FirstOrDefault(x => x.BuildingId == expectedFirstResult.Id);

            var totalSearchableItemsCount = data.Buildings.Count() + data.Locks.Count() + data.Groups.Count() + data.Media.Count();
            var expectedBuidlingCount = data.Buildings.Count(x => x.Name.ToLower().Contains(searchString.ToLower()));
            var expectedLocksCount = data.Locks.Count(x => x.BuildingId == expectedFirstResult.Id);
            var expectedSearchResultsCount = expectedBuidlingCount + expectedLocksCount;

            // Act
            var searchResults = await _searchService.ApplySearch(searchString);

            // Assert
            using var scope = new AssertionScope();
            searchResults.Should().NotBeNull();
            searchResults.Should().BeAssignableTo<IEnumerable<SearchResultDTO>>();
            searchResults.Should().HaveCount(expectedSearchResultsCount);

            searchResults.Count(x => x.ResultObjectType == ObjectType.Building).Should().Be(expectedBuidlingCount);
            searchResults.Count(x => x.ResultObjectType == ObjectType.Lock).Should().Be(expectedLocksCount);
            searchResults.Count(x => x.ResultObjectType == ObjectType.Group).Should().Be(0);
            searchResults.Count(x => x.ResultObjectType == ObjectType.Medium).Should().Be(0);

            searchResults.First().ResultObjectId.Should().Be(expectedFirstResult.Id);
            searchResults.ElementAt(1).ResultObjectId.Should().Be(expectedSecondResult.Id);

            _dataProviderMock.Verify(x => x.GetData(), Times.Once);

            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<ISearchable>(), It.IsAny<string>()), Times.Exactly(totalSearchableItemsCount));
            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Building>(), searchString), Times.Exactly(data.Buildings.Count()));
            _searchEvaluatorMock.Verify(x => x.Evaluate(data.Buildings.First(), searchString), Times.Once);
            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Lock>(), searchString), Times.Exactly(data.Locks.Count()));
            _searchEvaluatorMock.Verify(x => x.Evaluate(data.Locks.First(), searchString), Times.Once);
            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Group>(), searchString), Times.Exactly(data.Groups.Count()));
            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Medium>(), searchString), Times.Exactly(data.Media.Count()));
        }

        [Fact]
        public async Task Should_GetExpectedGroupAndMedia_ForGivenSearchString()
        {
            // Arrange
            var searchString = "default";
            var data = GetMockedData();

            _dataProviderMock.Setup(p => p.GetData()).ReturnsAsync(data);
            _searchEvaluatorMock.Setup(p => p.Evaluate(data.Groups.First(), searchString)).Returns((9, 8));

            var expectedFirstResult = data.Groups.FirstOrDefault(x => x.Name.ToLower().Contains(searchString.ToLower()));
            var expectedSecondResult = data.Media.FirstOrDefault(x => x.GroupId == expectedFirstResult.Id);

            var totalSearchableItemsCount = data.Buildings.Count() + data.Locks.Count() + data.Groups.Count() + data.Media.Count();
            var expectedGroupCount = data.Groups.Count(x => x.Name.ToLower().Contains(searchString.ToLower()));
            var expectedMediaCount = data.Media.Count(x => x.GroupId == expectedFirstResult.Id);
            var expectedSearchResultsCount = expectedGroupCount + expectedMediaCount;

            // Act
            var searchResults = await _searchService.ApplySearch(searchString);

            // Assert
            using var scope = new AssertionScope();
            searchResults.Should().NotBeNull();
            searchResults.Should().BeAssignableTo<IEnumerable<SearchResultDTO>>();
            searchResults.Should().HaveCount(expectedSearchResultsCount);

            searchResults.Count(x => x.ResultObjectType == ObjectType.Building).Should().Be(0);
            searchResults.Count(x => x.ResultObjectType == ObjectType.Lock).Should().Be(0);
            searchResults.Count(x => x.ResultObjectType == ObjectType.Group).Should().Be(expectedGroupCount);
            searchResults.Count(x => x.ResultObjectType == ObjectType.Medium).Should().Be(expectedMediaCount);

            searchResults.First().ResultObjectId.Should().Be(expectedFirstResult.Id);
            searchResults.ElementAt(1).ResultObjectId.Should().Be(expectedSecondResult.Id);

            _dataProviderMock.Verify(x => x.GetData(), Times.Once);

            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<ISearchable>(), It.IsAny<string>()), Times.Exactly(totalSearchableItemsCount));
            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Building>(), searchString), Times.Exactly(data.Buildings.Count()));
            _searchEvaluatorMock.Verify(x => x.Evaluate(data.Buildings.First(), searchString), Times.Once);
            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Lock>(), searchString), Times.Exactly(data.Locks.Count()));
            _searchEvaluatorMock.Verify(x => x.Evaluate(data.Locks.First(), searchString), Times.Once);
            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Group>(), searchString), Times.Exactly(data.Groups.Count()));
            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<Medium>(), searchString), Times.Exactly(data.Media.Count()));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public async Task Should_GetEmptyResults_WhenEmptySearchString(string query)
        {
            // Act
            var searchResults = await _searchService.ApplySearch(query);

            // Assert
            using var scope = new AssertionScope();
            searchResults.Should().NotBeNull();
            searchResults.Should().BeAssignableTo<IEnumerable<SearchResultDTO>>();
            searchResults.Should().HaveCount(0);

            _dataProviderMock.Verify(x => x.GetData(), Times.Never);

            _searchEvaluatorMock.Verify(x => x.Evaluate(It.IsAny<ISearchable>(), It.IsAny<string>()), Times.Never);
        }

        private static RootObject GetMockedData()
        {
            var buildings = new List<Building>
            {
                new Building
                {
                    Id = Guid.NewGuid(),
                    Name = "Head Office",
                    ShortCut = "HOFF",
                    Description = "Head Office, Feringastraße 4, 85774 Unterföhring",
                },
                new Building
                {
                    Id = Guid.NewGuid(),
                    Name = "Small Office",
                    ShortCut = "SOFF",
                    Description = "Small Office, Feringastraße 4, 85774 Unterföhring",
                },
            };

            var locks = new List<Lock>
            {
                new Lock
                {
                    Id = Guid.NewGuid(),
                    BuildingId = buildings.First().Id,
                    Type = LockType.Cylinder,
                    Name = "Gästezimmer 4.OG",
                    SerialNumber = "UID-A89F98F3",
                    Floor = "4.OG",
                    RoomNumber = "454",
                },
                new Lock
                {
                    Id = Guid.NewGuid(),
                    BuildingId = buildings.First().Id,
                    Type = LockType.Cylinder,
                    Name = "WC Herren 3.OG süd",
                    SerialNumber = "UID-C043133A",
                    Floor = "3.OG",
                    RoomNumber = "WC.HL"
                },
                new Lock
                {
                    Id = Guid.NewGuid(),
                    BuildingId = buildings.Last().Id,
                    Type = LockType.SmartHandle,
                    Name = "Besprechungsraum Osterfeld groß",
                    SerialNumber = "UID-21BC2485",
                    Floor = null,
                    RoomNumber = "B.02",
                }
            };

            var groups = new List<Group>
            {
                new Group
                {
                    Id = Guid.NewGuid(),
                    Name = "Default",
                    Description = "All media belong to this group",
                }
            };

            var media = new List<Medium>
            {
                new Medium
                {
                    Id = Guid.NewGuid(),
                    GroupId = groups.First().Id,
                    Owner = "CEO",
                    Description = "CEO's transponder",
                    Type = MediumType.Transponder,
                    SerialNumber = "UID-9876543",
                },
                new Medium
                {
                    Id = Guid.NewGuid(),
                    GroupId = Guid.NewGuid(),
                    Owner = "CTO",
                    Description = "CTO's card",
                    Type = MediumType.Card,
                    SerialNumber = "UID-123456",
                },
            };

            return new RootObject
            {
                Buildings = buildings,
                Locks = locks,
                Groups = groups,
                Media = media,
            };
        }
    }
}