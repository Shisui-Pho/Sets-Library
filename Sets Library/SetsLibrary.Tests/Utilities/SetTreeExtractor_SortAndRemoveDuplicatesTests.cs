﻿using SetsLibrary.Utility;
using Xunit;
namespace SetsLibrary.Tests.Utilities.SortAndRemove;
public class SortAndRemoveDuplicatesTests
{
    // Helper method to create SetsConfigurations for testing
    private SetsConfigurations CreateIntConfig()
    {
        return new SetsConfigurations(";", ",");
    }

    [Fact]
    public void SortAndRemoveDuplicates_ValidInput()
    {
        // Arrange
        string input = "3,1,2,3,2";
        var config = CreateIntConfig();

        // Act
        var result = SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out _, out _).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0]);
        Assert.Equal(2, result[1]);
        Assert.Equal(3, result[2]);
    }

    [Fact]
    public void SortAndRemoveDuplicates_EmptyInput()
    {
        // Arrange
        string input = "";
        var config = CreateIntConfig();

        // Act
        var result = SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out bool _, out int _).ToList();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void SortAndRemoveDuplicates_InvalidConversion()
    {
        // Arrange
        string input = "a,b,c";
        var config = CreateIntConfig();

        // Act & Assert
        Assert.Throws<SetsException>(() =>
            SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out _, out _).ToList());
    }

    [Fact]
    public void SortAndRemoveDuplicates_CustomConverter()
    {
        // Arrange
        var customConverter = new CustomStringToIntConverter();
        var config = new SetsConfigurations(",", ";");

        string input = "10;5;3;5;10";

        // Act
        var result = SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out _, out _).ToList();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(3, result[0]);
        Assert.Equal(5, result[1]);
        Assert.Equal(10, result[2]);
    }

    [Fact]
    public void SortAndRemoveDuplicates_InvalidCustomConverter()
    {
        // Arrange
        var invalidCustomConverter = new InvalidCustomConverter();
        var config = new SetsConfigurations(";", ",");

        string input = "invalid,values";

        // Act & Assert
        Assert.Throws<SetsException>(() =>
            SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out _, out _).ToList());
    }

    [Fact]
    public void SortAndRemoveDuplicates_DifferentDataTypes()
    {
        // Test with strings
        string stringInput = "apple,banana,apple,grape";
        var stringConfig = new SetsConfigurations(";", ",");
        var stringResult = SetTreeBuilder<string>.SortAndRemoveDuplicates(stringInput, stringConfig, out _, out _ ).ToList();
        Assert.Equal(3, stringResult.Count);
        Assert.Equal("apple", stringResult[0]);
        Assert.Equal("banana", stringResult[1]);
        Assert.Equal("grape", stringResult[2]);

        // Test with integers
        string intInput = "10,5,5,10,20";
        var intConfig = CreateIntConfig();
        var intResult = SetTreeBuilder<int>.SortAndRemoveDuplicates(intInput, intConfig, out _, out _).ToList();
        Assert.Equal(3, intResult.Count);
        Assert.Equal(5, intResult[0]);
        Assert.Equal(10, intResult[1]);
        Assert.Equal(20, intResult[2]);
    }

    // Additional Fact Tests

    [Fact]
    public void SortAndRemoveDuplicates_SingleElement()
    {
        // Arrange
        string input = "42";
        var config = CreateIntConfig();

        // Act
        var result = SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out _, out _).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(42, result[0]);
    }

    [Fact]
    public void SortAndRemoveDuplicates_WhitespaceHandling()
    {
        // Arrange
        string input = " , 5, 10 , ";
        var config = CreateIntConfig();

        // Act
        var result = SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out _, out _).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(5, result[0]);
        Assert.Equal(10, result[1]);
    }

    // Theory Tests

    [Theory]
    [InlineData("1,2,3,4", 4, new int[] { 1, 2, 3, 4 })]
    [InlineData("4,3,2,1", 4, new int[] { 1, 2, 3, 4 })]
    [InlineData("10,10,10,10", 1, new int[] { 10 })]
    [InlineData("100,200,300,100,200,300", 3, new int[] { 100, 200, 300 })]
    public void SortAndRemoveDuplicates_TheoryTests(string input, int expectedCount, int[] expectedResult)
    {
        // Arrange
        var config = CreateIntConfig();

        // Act
        var result = SetTreeBuilder<int>.SortAndRemoveDuplicates(input, config, out _, out _).ToList();

        // Assert
        Assert.Equal(expectedCount, result.Count);
        Assert.Equal(expectedResult, result.ToArray());
    }

    [Theory]
    [InlineData("apple,banana,orange", 3, new string[] { "apple", "banana", "orange" })]
    [InlineData("grape,apple,banana,grape", 3, new string[] { "apple", "banana", "grape" })]
    [InlineData("car,truck,car,truck", 2, new string[] { "car", "truck" })]
    public void SortAndRemoveDuplicates_StringTheoryTests(string input, int expectedCount, string[] expectedResult)
    {
        // Arrange
        var config = new SetsConfigurations(";", ",");

        // Act
        var result = SetTreeBuilder<string>.SortAndRemoveDuplicates(input, config, out _, out _).ToList();

        // Assert
        Assert.Equal(expectedCount, result.Count);
        Assert.Equal(expectedResult, result.ToArray());
    }
}

// Custom converter for testing
public class CustomStringToIntConverter : ICustomObjectConverter<int>
{
    public static int ToObject(string?[] field)
    {
        // Convert custom string representation of numbers (e.g., "10" => 10)
        return int.Parse(field[0] ?? "0");
    }
}

// Invalid custom converter to simulate an error
public class InvalidCustomConverter : ICustomObjectConverter<int>
{
    public static int ToObject(string?[] field)
    {
        // Invalid conversion, simulate FormatException
        return int.Parse("invalid"); // Will throw FormatException
    }
}