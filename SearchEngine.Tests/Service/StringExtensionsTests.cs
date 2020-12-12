using FluentAssertions;
using SearchEngine.Service.Extensions;
using Xunit;

namespace SearchEngine.Tests.Service
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("PascalCase", "Pascal Case")]
        [InlineData("TransponderWithCardInlay", "Transponder With Card Inlay")]
        [InlineData(" MainMedium WithLotsOfThingsToDo", "Main Medium With Lots Of Things To Do")]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("  ", "  ")]
        [InlineData("A", "A")]
        public void Should_SplitPascalCase(string input, string expectedResult)
        {
            // Act
            var result = input.SplitPascalCase();

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}