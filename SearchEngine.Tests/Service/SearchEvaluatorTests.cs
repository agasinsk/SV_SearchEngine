using FluentAssertions;
using SearchEngine.Model.Entity;
using SearchEngine.Service.Implementation;
using System;
using Xunit;

namespace SearchEngine.Tests.Service
{
    public class SearchEvaluatorTests
    {
        private readonly SearchEvaluator _searchEvaluator;

        public SearchEvaluatorTests()
        {
            _searchEvaluator = new SearchEvaluator();
        }

        [Theory]
        [InlineData("head", 14, 8)]
        [InlineData("Head", 14, 8)]
        [InlineData("office", 14, 8)]
        [InlineData("Office", 14, 8)]
        [InlineData("HO", 7, 5)]
        [InlineData("HOFF", 7, 5)]
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
            var result = _searchEvaluator.ApplySearch(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(expectedSearchWeight);
            result.transitiveWeight.Should().Be(expectedTransitiveWeight);
        }

        [Theory]
        [InlineData("head")]
        [InlineData("Head")]
        [InlineData("office")]
        [InlineData("Office")]
        [InlineData("HO")]
        [InlineData("HOFF")]
        [InlineData("Feringastraße")]
        [InlineData("85774")]
        [InlineData("Unterföhring")]
        [InlineData("Main")]
        [InlineData("Unknown")]
        [InlineData("")]
        [InlineData(null)]
        public void Should_GetDefaultSearchWeight_ForEmptyBuilding(string searchString)
        {
            // Arrange
            var building = new Building();

            // Act
            var result = _searchEvaluator.ApplySearch(building, searchString);

            // Assert
            result.Should().NotBeNull();
            result.weight.Should().Be(0);
            result.transitiveWeight.Should().Be(0);
        }
    }
}