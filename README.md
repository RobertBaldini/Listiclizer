# Listiclizer
Splits lists into listicles (sub-groups), with each list beginning with a given criteria

```
  [TestMethod]
  public void Should_split_list_of_strings_into_two_lists_each_beginning_with_A()
  {
      // Arrange
      var stringInput = new[] { "A", "B", "C", "D", "A", "B", "C" }.ToList();

      // Act
      var listsOfStrings = Listiclizer.SplitIntoListicles(stringInput, s => s == "A");

      // Assert
      listsOfStrings.Should().HaveCount(2);

      listsOfStrings[0].Should().ContainInOrder("A", "B", "C", "D");
      listsOfStrings[1].Should().ContainInOrder("A", "B", "C");
  }
```
