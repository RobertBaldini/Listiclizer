![ ](https://raw.githubusercontent.com/RobertBaldini/Listiclizer/master/Listiclizer-Logo-png.png)

# Listiclizer
Splits lists into listicles (sub-groups), where each list begins with a given criteria

```
  [TestMethod]
  public void Should_split_list_of_strings_into_two_lists_each_beginning_with_A()
  {
      // Arrange
      var stringInput = new[] { "A", "B", "C", "0", "1", "A", "B", "C" }.ToList();

      // Act
      var listsOfStrings = Listiclizer.SplitIntoListicles(stringInput, s => s == "A");

      // Assert
      listsOfStrings.Should().HaveCount(2);

      listsOfStrings[0].Should().ContainInOrder("A", "B", "C", "0", "1");
      listsOfStrings[1].Should().ContainInOrder("A", "B", "C");
  }
```
