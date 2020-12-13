using FluentAssertions;
using SearchEngine.Model.Entity;
using SearchEngine.Model.Enum;
using SearchEngine.Service.Implementation;
using SearchEngine.Service.Implementation.SearchConfiguration;
using SearchEngine.Service.Interface;
using System;
using Xunit;

namespace SearchEngine.Tests.Service
{
    public class SearchEvaluatorTests
    {
        private readonly ISearchEvaluator _searchEvaluator;

        public SearchEvaluatorTests()
        {
            _searchEvaluator = new SearchEvaluator(new SearchConfigurationFactory());
        }

        [Theory]
        [InlineData("head", 14, 8)]
        [InlineData("Head", 14, 8)]
        [InlineData("office", 14, 8)]
        [InlineData("Office", 14, 8)]
        [InlineData("Head Office", 95, 80)]
        [InlineData("HO", 7, 5)]
        [InlineData("HOFF", 70, 50)]
        [InlineData("Feringastraße", 5, 0)]
        [InlineData("85774", 5, 0)]
        [InlineData("Unterföhring", 5, 0)]
        [InlineData("Main", 0, 0)]
        [InlineData("Unknown", 0, 0)]
        [InlineData("", 0, 0)]
        [InlineData(null, 0, 0)]
        public void Should_GetCorrectSearchWeight_ForBuilding(string searchString, int expectedSearchWeight, int expectedTransitiveWeight)
        {
            // Arrange
            var building = new Building
            {
                Id = Guid.NewGuid(),
                Name = "Head Office",
                ShortCut = "HOFF",
                Description = "Head Office, Feringastraße 4, 85774 Unterföhring",
            };

            // Act
            var result = _searchEvaluator.Evaluate(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(expectedSearchWeight);
            result.transitiveWeight.Should().Be(expectedTransitiveWeight);
        }

        [Theory]
        [InlineData("head")]
        [InlineData("office")]
        [InlineData("85774")]
        [InlineData("")]
        [InlineData(null)]
        public void Should_GetDefaultSearchWeight_ForEmptyBuilding(string searchString)
        {
            // Arrange
            var building = new Building();

            // Act
            var result = _searchEvaluator.Evaluate(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(0);
            result.transitiveWeight.Should().Be(0);
        }

        [Theory]
        [InlineData("Gästezimmer", 10, 0)]
        [InlineData("zimmer", 10, 0)]
        [InlineData("UID", 8, 0)]
        [InlineData("A89F98F3", 8, 0)]
        [InlineData("Cylinder", 30, 0)]
        [InlineData("4.OG", 16, 0)]
        [InlineData("OG", 16, 0)]
        [InlineData("Floor", 6, 0)]
        [InlineData("454", 60, 0)]
        [InlineData("", 0, 0)]
        [InlineData(null, 0, 0)]
        public void Should_GetCorrectSearchWeight_ForLock(string searchString, int expectedSearchWeight, int expectedTransitiveWeight)
        {
            // Arrange
            var cylinderLock = new Lock
            {
                Id = Guid.NewGuid(),
                BuildingId = Guid.NewGuid(),
                Type = LockType.Cylinder,
                Name = "Gästezimmer 4.OG",
                SerialNumber = "UID-A89F98F3",
                Floor = "Floor 4.OG",
                RoomNumber = "454",
            };

            // Act
            var result = _searchEvaluator.Evaluate(cylinderLock, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(expectedSearchWeight);
            result.transitiveWeight.Should().Be(expectedTransitiveWeight);
        }

        [Theory]
        [InlineData("head")]
        [InlineData("Office")]
        [InlineData("Main")]
        [InlineData("")]
        [InlineData(null)]
        public void Should_GetDefaultSearchWeight_ForEmptyLock(string searchString)
        {
            // Arrange
            var building = new Lock();

            // Act
            var result = _searchEvaluator.Evaluate(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(0);
            result.transitiveWeight.Should().Be(0);
        }

        [Theory]
        [InlineData("main", 9, 8)]
        [InlineData("Main", 9, 8)]
        [InlineData("Group", 14, 8)]
        [InlineData("CEO", 5, 0)]
        [InlineData("CFO", 5, 0)]
        [InlineData("CTo", 5, 0)]
        [InlineData("etc", 5, 0)]
        [InlineData("Unknown", 0, 0)]
        [InlineData("", 0, 0)]
        [InlineData(null, 0, 0)]
        public void Should_GetCorrectSearchWeight_ForGroup(string searchString, int expectedSearchWeight, int expectedTransitiveWeight)
        {
            // Arrange
            var building = new Group
            {
                Id = Guid.NewGuid(),
                Name = "Main Group",
                Description = "Group CEO, CFO, CTO, etc."
            };

            // Act
            var result = _searchEvaluator.Evaluate(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(expectedSearchWeight);
            result.transitiveWeight.Should().Be(expectedTransitiveWeight);
        }

        [Theory]
        [InlineData("head")]
        [InlineData("Office")]
        [InlineData("")]
        [InlineData(null)]
        public void Should_GetDefaultSearchWeight_ForEmptyGroup(string searchString)
        {
            // Arrange
            var building = new Group();

            // Act
            var result = _searchEvaluator.Evaluate(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(0);
            result.transitiveWeight.Should().Be(0);
        }

        [Theory]
        [InlineData("UID", 8, 0)]
        [InlineData("378D17F6", 8, 0)]
        [InlineData("Guest", 10, 0)]
        [InlineData("Card", 30, 0)]
        [InlineData("Unknown", 0, 0)]
        [InlineData("", 0, 0)]
        [InlineData(null, 0, 0)]
        public void Should_GetCorrectSearchWeight_ForMedium(string searchString, int expectedSearchWeight, int expectedTransitiveWeight)
        {
            // Arrange
            var building = new Medium
            {
                Id = Guid.NewGuid(),
                SerialNumber = "UID-378D17F6",
                Type = MediumType.Card,
                Owner = "Guest 1",
            };

            // Act
            var result = _searchEvaluator.Evaluate(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(expectedSearchWeight);
            result.transitiveWeight.Should().Be(expectedTransitiveWeight);
        }

        [Theory]
        [InlineData("head")]
        [InlineData("Office")]
        [InlineData("")]
        [InlineData(null)]
        public void Should_GetDefaultSearchWeight_ForEmptyMedium(string searchString)
        {
            // Arrange
            var building = new Medium();

            // Act
            var result = _searchEvaluator.Evaluate(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(0);
            result.transitiveWeight.Should().Be(0);
        }
    }
}