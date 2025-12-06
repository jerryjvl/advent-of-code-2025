using System.Runtime.Serialization;

var lines = File.ReadAllLines("../input-full.txt");

// Process the input into more useful structures
var emptyIx = Array.FindIndex(lines, string.IsNullOrEmpty);
var ranges = lines.Take(emptyIx).Select(
        rangeStr => {
            var split = rangeStr.Split("-");
            return (long.Parse(split[0]), long.Parse(split[1]));
        }
    ).ToList();

// Sort the ranges lexicographically
ranges.Sort();

// Across a list of sorted ranges, deal with overlap between subsequent ranges
var lastRange = ranges[0];
long total = lastRange.Item2 - lastRange.Item1 + 1;
foreach (var range in ranges.Skip(1))
{
    if (range.Item1 > lastRange.Item2)
    {
        // No overlap, so simply add and move on
        total += range.Item2 - range.Item1 + 1;
        lastRange = range;
    }
    else if (range.Item2 > lastRange.Item2)
    {
        // "lastRange" overlaps "range", but does not fully enclose it
        total += range.Item2 - lastRange.Item2;
        lastRange = range;
    }
}

// input      = 14
// input-full = 338189277144473
Console.WriteLine(total);

