using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Listiclization.Tests
{
    [TestClass]
    public class ListiclizerTests
    {
        [TestMethod]
        public void Should_break_list_of_ints_into_sub_groups_each_starting_with_1()
        {
            // Arrange
            var expectedList1 = new[] { 1, 2, 3, 4, 0 };
            var expectedList2 = new[] { 1, 2, 3 };
            var intInputs = expectedList1.Concat(expectedList2).ToList();

            // Act
            var listsOfInts = Listiclizer.SplitIntoListicles(intInputs, i => i == 1);

            // Assert
            listsOfInts.Should().HaveCount(2);

            listsOfInts[0].Should().HaveCount(5);
            listsOfInts[0].Should().ContainInOrder(expectedList1);
            listsOfInts[1].Should().HaveCount(3);
            listsOfInts[1].Should().ContainInOrder(expectedList2);
        }

        [TestMethod]
        public void Should_return_trailing_orphan_as_its_own_listicle()
        {
            // Arrange
            var intInputs = new[] { 1, 2, 3, 4, 0, 1 }.ToList();

            // Act
            var listsOfInts = Listiclizer.SplitIntoListicles(intInputs, i => i == 1);

            // Assert
            listsOfInts.Should().HaveCount(2);
            listsOfInts[1].Should().HaveCount(1);
            listsOfInts[1][0].Should().Be(1);
        }

        [TestMethod]
        public void Should_return_both_leading_orphan_and_trailing_orphan()
        {
            // Arrange
            var intInputs = new[] { 1, 1, 2, 3, 4, 0, 1 }.ToList();

            // Act
            var listsOfInts = Listiclizer.SplitIntoListicles(intInputs, i => i == 1);

            // Assert
            listsOfInts.Should().HaveCount(3);
            listsOfInts[0].Should().ContainInOrder(1);
            listsOfInts[1].Should().ContainInOrder(1, 2, 3, 4, 0);
            listsOfInts[2].Should().ContainInOrder(1);
        }

        [TestMethod]
        public void Should_preserve_any_nonmatching_leading_elements_as_its_own_subgroup()
        {
            // Arrange
            var intInputs = new[] { -1, -2, 1, 2, 3, 4, 0, 1, 2 }.ToList();

            // Act
            var listsOfInts = Listiclizer.SplitIntoListicles(intInputs, i => i == 1);

            // Assert
            listsOfInts.Should().HaveCount(3);
            listsOfInts[0].Should().ContainInOrder(-1, -2);
            listsOfInts[1].Should().ContainInOrder(1, 2, 3, 4, 0);
            listsOfInts[2].Should().ContainInOrder(1, 2);
        }

        [TestMethod]
        public void Should_return_just_one_listicle_if_there_is_no_predicate_match()
        {
            // Arrange
            var intInputs = new[] { 1, 2, 3, 4, 0, 1, 2, 3 }.ToList();

            // Act
            var listsOfInts = Listiclizer.SplitIntoListicles(intInputs, i => i == 99);

            // Assert
            listsOfInts.Should().HaveCount(1);
            listsOfInts[0].Should().HaveCount(8);
            listsOfInts[0].Should().ContainInOrder(intInputs);
        }

        [TestMethod]
        public void Should_return_one_listicle_if_only_one_element_is_provided_and_matches()
        {
            // Arrange
            var intInputs = new[] { 1 }.ToList();

            // Act
            var listOfInts = Listiclizer.SplitIntoListicles(intInputs, i => i == 1);

            // Assert
            listOfInts.Should().HaveCount(1);
            listOfInts[0].Should().HaveCount(1);
            listOfInts[0].Should().ContainInOrder(intInputs);
        }

        [TestMethod]
        public void Should_return_one_listicle_if_only_one_element_is_provided_and_doesnt_match()
        {
            // Arrange
            var intInputs = new[] { 1 }.ToList();

            // Act
            var listOfInts = Listiclizer.SplitIntoListicles(intInputs, i => i == 99);

            // Assert
            listOfInts.Should().HaveCount(1);
            listOfInts[0].Should().HaveCount(1);
            listOfInts[0].Should().ContainInOrder(intInputs);
        }

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

        [TestMethod]
        public void Should_allow_for_type_specific_criteria_like_string_StartsWith()
        {
            // Arrange
            var stringInputs = new[]
            {
                "System Log 111   ",
                "--------------   ",
                "Alpha lorem      ",
                "Bravo ipsum      ",
                "Charlie dolor    ",
                "Delta sit        ",
                "                 ",
                "System Log 112   ",
                "--------------   ",
                "Alpha lorem      ",
                "Bravo ipsum      ",
                "Charlie dolor    "
            }.ToList();

            // Act
            var listsOfStrings = Listiclizer.SplitIntoListicles(stringInputs, s => s.StartsWith("System Log"));

            // Assert
            listsOfStrings.Should().HaveCount(2);

            listsOfStrings[0].Should().ContainInOrder(stringInputs.Take(7));
            listsOfStrings[1].Should().ContainInOrder(stringInputs.Skip(7).Take(5));
        }

        [TestMethod]
        public void Should_accomodate_any_unrelated_leading_text_and_preserve_it_as_its_own_subgroup()
        {
            // Arrange
            var stringInputs = new[]
            {
                "Red: 1       ",
                "Green: 2     ",
                "Blue: 3      ",
                "             ",
                "Alpha lorem  ",
                "Bravo ipsum  ",
                "Charlie dolor",
                "Delta sit    ",
                "             ",
                "Alpha lorem  ",
                "Bravo ipsum  ",
                "Charlie dolor"
            }.ToList();

            // Act
            var listsOfStrings = Listiclizer.SplitIntoListicles(stringInputs, s => s.StartsWith("Alpha"));

            // Assert
            listsOfStrings.Should().HaveCount(3);

            listsOfStrings[0].Should().ContainInOrder(stringInputs.Take(4));
            listsOfStrings[1].Should().ContainInOrder(stringInputs.Skip(4).Take(5));
            listsOfStrings[2].Should().ContainInOrder(stringInputs.Skip(9).Take(3));
        }

        [TestMethod]
        public void Should_also_split_groups_of_objects_into_subgroups_each_beginning_with_a_given_property_criteria()
        {
            // Arrange
            var logReadout = new[]
            {
                new { LogTypeId = 1 },
                new { LogTypeId = 2 },
                new { LogTypeId = 3 },
                new { LogTypeId = 4 },
                new { LogTypeId = 1 },
                new { LogTypeId = 2 }
            }.ToList();
            
            // Act
            var splitLogs = Listiclizer.SplitIntoListicles(logReadout, log => log.LogTypeId == 1);

            // Assert
            splitLogs.Should().HaveCount(2);
            splitLogs[0].Should().ContainInOrder(logReadout.Take(4));
            splitLogs[1].Should().ContainInOrder(logReadout.Skip(4).Take(2));
        }
    }
}