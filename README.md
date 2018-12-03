![ ](https://raw.githubusercontent.com/RobertBaldini/Listiclizer/master/Listiclizer-Logo-png.png)

# Listiclizer
Splits lists into listicles (sub-groups), where each list begins with a given criteria

For a simple example, notice that the original sort order is preserved.

```
var intInputs = new [] { 1, 2, 3, 0, 1, 2, 3, 4 };
var listOfLists = Listiclizer.SplitIntoListicles(intInputs, num => num == 1);

// listOfLists.Should().HaveCount(2);
// listOfLists[0].Should().ContainInOrder(1, 2, 3, 0);
// listOfLists[1].Should().ContainInOrder(1, 2, 3, 4);
```

Can be useful when parsing through elaborate freeform text, like a system feed or user interface text.

```
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

var listsOfStrings = Listiclizer.SplitIntoListicles(stringInputs, s => s.StartsWith("System Log"));

//listsOfStrings.Should().HaveCount(2);
//listsOfStrings[0].Should().ContainInOrder(stringInputs.Take(7));
//listsOfStrings[1].Should().ContainInOrder(stringInputs.Skip(7).Take(5));
```

But it's especially helpful for splitting lists of objects by interrogating their properties.

```
var logEntries = new[]
{
    new { LogTypeId = 1, IsFaulted = true },
    new { LogTypeId = 2, IsFaulted = false },
    new { LogTypeId = 3, IsFaulted = false },
    new { LogTypeId = 4, IsFaulted = false },
    new { LogTypeId = 1, IsFaulted = true },
    new { LogTypeId = 2, IsFaulted = false }
}.ToList();

var splitLogs = Listiclizer.SplitIntoListicles(logEntries, log => log.IsFaulted);

//splitLogs.Should().HaveCount(2);
//splitLogs[0].Should().ContainInOrder(logEntries.Take(4));
//splitLogs[1].Should().ContainInOrder(logEntries.Skip(4).Take(2));
```