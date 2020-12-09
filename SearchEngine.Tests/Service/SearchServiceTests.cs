using AutoMapper;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using SearchEngine.Model.DTO;
using SearchEngine.Model.Entity;
using SearchEngine.Model.Enum;
using SearchEngine.Service.Configuration;
using SearchEngine.Service.Implementation;
using SearchEngine.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SearchEngine.Tests.Service
{
    public class SearchServiceTests
    {
        private readonly Mapper _mapper;
        private readonly Mock<IDataProvider<RootObject>> _dataProviderMock;
        private readonly ISearchService _searchService;

        public SearchServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(new SearchProfile())));
            _dataProviderMock = new Mock<IDataProvider<RootObject>>();
            _searchService = new SearchService(_dataProviderMock.Object, _mapper);
        }

        [Fact]
        public async Task Should_GetExpectedSearchResults_ForGivenSearchString()
        {
            // Arrange
            var searchString = "head";
            var data = GetMockedData();

            _dataProviderMock.Setup(p => p.GetData()).ReturnsAsync(data);

            var expectedFirstResult = data.Buildings.FirstOrDefault(x => x.Name.ToLower().Contains(searchString.ToLower()));
            var expectedSecondResult = data.Locks.FirstOrDefault(x => x.BuildingId == expectedFirstResult.Id);

            // Act
            var searchResults = await _searchService.ApplySearch(searchString);

            // Assert
            using var scope = new AssertionScope();
            searchResults.Should().NotBeNull();
            searchResults.Should().BeAssignableTo<IEnumerable<SearchResultDTO>>();
            searchResults.Should().HaveCount(data.Buildings.Count() + data.Locks.Count());

            searchResults.Count(x => x.SearchObjectType == SearchObjectType.Building).Should().Be(data.Buildings.Count());
            searchResults.Count(x => x.SearchObjectType == SearchObjectType.Lock).Should().Be(data.Locks.Count());
            searchResults.Count(x => x.SearchObjectType == SearchObjectType.Group).Should().Be(data.Groups.Count());
            searchResults.Count(x => x.SearchObjectType == SearchObjectType.Medium).Should().Be(data.Media.Count());

            searchResults.First().ResultObjectId.Should().Be(expectedFirstResult.Id);
            searchResults.ElementAt(1).ResultObjectId.Should().Be(expectedSecondResult.Id);

            _dataProviderMock.Verify(x => x.GetData(), Times.Once);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Should_GetAllResults_WhenEmptySearchString(string searchString)
        {
            // Arrange
            var data = GetMockedData();
            _dataProviderMock.Setup(p => p.GetData()).ReturnsAsync(data);

            // Act
            var searchResults = await _searchService.ApplySearch(searchString);

            // Assert
            using var scope = new AssertionScope();
            searchResults.Should().NotBeNull();
            searchResults.Should().BeAssignableTo<IEnumerable<SearchResultDTO>>();
            searchResults.Should().HaveCount(data.Buildings.Count() + data.Locks.Count());

            searchResults.Count(x => x.SearchObjectType == SearchObjectType.Building).Should().Be(data.Buildings.Count());
            searchResults.Count(x => x.SearchObjectType == SearchObjectType.Lock).Should().Be(data.Locks.Count());
            searchResults.Count(x => x.SearchObjectType == SearchObjectType.Group).Should().Be(data.Groups.Count());
            searchResults.Count(x => x.SearchObjectType == SearchObjectType.Medium).Should().Be(data.Media.Count());

            searchResults.Select(x => x.ResultObjectId).Should()
                .BeEquivalentTo(data.Buildings.Select(x => x.Id).Concat(data.Locks.Select(x => x.Id)));
            searchResults.Select(x => x.ResultObjectName).Should()
                .BeEquivalentTo(data.Buildings.Select(x => x.Name).Concat(data.Locks.Select(x => x.Name)));
            searchResults.Select(x => x.ResultObjectKey).Should()
                .BeEquivalentTo(data.Buildings.Select(x => x.Id.ToString()).Concat(data.Locks.Select(x => x.SerialNumber)));
            searchResults.Select(x => x.ResultObjectDescription).Should()
                .BeEquivalentTo(data.Buildings.Select(x => x.Description).Concat(data.Locks.Select(x => x.Description)));

            _dataProviderMock.Verify(x => x.GetData(), Times.Once);
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

            return new RootObject
            {
                Buildings = buildings,
                Locks = locks,
                Groups = Enumerable.Empty<Group>(),
                Media = Enumerable.Empty<Medium>(),
            };
        }
    }
}