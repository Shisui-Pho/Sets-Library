﻿using Xunit;
using SetsLibrary;
namespace SetsLibrary.Tests.Models
{
    public class SetExtractionConfigurationTests
    {
        [Fact]
        public void Constructor_Should_Throw_ArgumentNullException_When_FieldTerminator_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new SetExtractionConfiguration<int>(null, ""));
        }//Constructor_Should_Throw_ArgumentNullException_When_FieldTerminator_Is_Null

        [Fact]
        public void Constructor_Should_Throw_ArgumentNullException_When_RowTerminator_Is_Null()
        {
            Assert.Throws<ArgumentNullException>(() => new SetExtractionConfiguration<int>(",",null));
        }//Constructor_Should_Throw_ArgumentNullException_When_FieldTerminator_Is_Null


        [Fact]
        public void Constructor_Should_Throw_ArgumentException_When_Terminators_Are_The_Same()
        {
            string terminator = ";";

            Assert.Throws<ArgumentException>(() => new SetExtractionConfiguration<int>(terminator, terminator));
        }//Constructor_Should_Throw_ArgumentNullException_When_FieldTerminator_Is_Null


        [Theory]
        [InlineData(";", ",}")]
        [InlineData("{;}", "\"\"")]
        [InlineData("}{", ",'}")]
        [InlineData(" \\{", "','")]
        [InlineData("}", ";;")]
        [InlineData("5434sfsdf}", "sdfsdfsf")]
        public void Constructor_Should_Throw_ArgumentException_When_Terminators_Use_Reserved_Characters(string fieldTerm, string rownTerm)
        {
            Assert.Throws<ArgumentException>(() => new SetExtractionConfiguration<int>(fieldTerm, rownTerm));
        }//Constructor_Should_Throw_ArgumentNullException_When_FieldTerminator_Is_Null

        [Theory]
        [InlineData(";", "'")]
        [InlineData("-", "\"\"")]
        [InlineData("=", ",'")]
        [InlineData("+", "-")]
        [InlineData("''", ";;")]
        [InlineData("5434sfsdf", "sdfsdfs")]
        public void Valid_Terminators(string fieldTerminator, string rowTerminator)
        {
            //Create a null instance
            SetExtractionConfiguration<int>? config = null;

            try
            {
                config = new SetExtractionConfiguration<int>(fieldTerminator, rowTerminator);
            }
            catch { }//Swallow the exeption

            //Check if the 'config variable is not null
            Assert.NotNull(config);
            
            //Verify the terminators
            Assert.Equal(fieldTerminator, config.FieldTerminator);
            Assert.Equal(rowTerminator, config.RowTerminator);
        }//Constructor_Should_Throw_ArgumentNullException_When_FieldTerminator_Is_Null

        [Theory]
        [InlineData(null, "rowTerminator")]
        [InlineData("fieldTerminator", null)]
        public void Constructor_NullTerminators_ThrowsArgumentNullException(string fieldTerminator, string rowTerminator)
        {
            Assert.Throws<ArgumentNullException>(() => new SetExtractionConfiguration<string>(fieldTerminator, rowTerminator));
        }

        [Fact]
        public void Constructor_SameFieldAndRowTerminators_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new SetExtractionConfiguration<string>("terminator", "terminator"));
        }

        [Theory]
        [InlineData("{", "rowTerminator")]
        [InlineData("fieldTerminator", "}")]
        public void Constructor_ReservedCharactersInTerminators_ThrowsArgumentException(string fieldTerminator, string rowTerminator)
        {
            Assert.Throws<ArgumentException>(() => new SetExtractionConfiguration<string>(fieldTerminator, rowTerminator));
        }

        [Fact]
        public void Constructor_ValidTerminators_AssignsTerminators()
        {
            var config = new SetExtractionConfiguration<string>("fieldTerminator", "rowTerminator");
            Assert.Equal("fieldTerminator", config.FieldTerminator);
            Assert.Equal("rowTerminator", config.RowTerminator);
            Assert.False(config.IsICustomObject);
        }

        [Fact]
        public void Constructor_NullConverter_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new SetExtractionConfiguration<string>("fieldTerminator", "rowTerminator", null));
        }

        [Fact]
        public void Constructor_ValidConverter_AssignsConverter()
        {
            var converter = new MockConverter();
            var config = new SetExtractionConfiguration<string>("fieldTerminator", "rowTerminator", converter);
            Assert.Equal("fieldTerminator", config.FieldTerminator);
            Assert.Equal("rowTerminator", config.RowTerminator);
            Assert.True(config.IsICustomObject);
            Assert.Equal(converter, config.Converter);
        }

        [Fact]
        public void ToObject_NullConverter_ThrowsArgumentNullException()
        {
            var config = new SetExtractionConfiguration<string>("fieldTerminator", "rowTerminator");
            Assert.Throws<ArgumentNullException>(() => config.ToObject("record"));
        }

        [Fact]
        public void ToObject_ValidConverter_CallsConverter()
        {
            var converter = new MockConverter();
            var config = new SetExtractionConfiguration<string>("fieldTerminator", "rowTerminator", converter);
            var result = config.ToObject("record");
            Assert.Equal("convertedRecord", result);
        }

        private class MockConverter : ICustomObjectConverter<string>
        {
            public string ToObject(string field, SetExtractionConfiguration<string> settings)
            {
                return "convertedRecord";
            }
        }

    }//class
}//namespace