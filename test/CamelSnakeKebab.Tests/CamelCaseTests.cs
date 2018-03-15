using Xunit;

namespace CamelSnakeKebab.Tests
{
    public class CamelCaseTests
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("I", "i")]
        [InlineData("ID", "id")]
        [InlineData("VmQ", "vmQ")]
        [InlineData("URL", "url")]
        [InlineData("IsCIA", "isCIA")]
        [InlineData("Person", "person")]
        [InlineData("iPhone", "iPhone")]
        [InlineData("IPhone", "iPhone")]
        [InlineData("I  Phone", "iPhone")]
        [InlineData("  IPhone", "iPhone")]
        [InlineData("BUILDING", "building")]
        [InlineData("URLValue", "urlValue")]
        [InlineData("  IPhone  ", "iPhone")]
        [InlineData("Xml2Json", "xml2Json")]
        [InlineData("XML2JSON", "xml2json")]
        [InlineData("SnAkEcAsE", "snAkEcAsE")]
        [InlineData("SnA__kEcAsE", "snAKEcAsE")]
        [InlineData("SnA__ kEcAsE", "snAKEcAsE")]
        [InlineData("OAuth2Scheme", "oAuth2Scheme")]
        [InlineData("SHOUTING_CASE", "shoutingCase")]
        [InlineData("IsJSONProperty", "isJSONProperty")]
        [InlineData("BUILDING Property", "buildingProperty")]
        [InlineData("Building Property", "buildingProperty")]
        [InlineData("building PROPERTY", "buildingProperty")]
        [InlineData("already_snake_case_ ", "alreadySnakeCase")]
        [InlineData("Property.NestedProperty", "property.nestedProperty")]
        [InlineData("Hello World! This is a test.", "helloWorld!thisIsATest.")]
        public void ShouldProduceCorrectResults(string value, string expected)
        {
            Assert.Equal(expected, value.ToCamelCase());
        }
    }
}