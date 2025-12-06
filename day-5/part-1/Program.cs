var lines = File.ReadAllLines("../input-full.txt");

// Process the input into more useful structures
var emptyIx = Array.FindIndex(lines, string.IsNullOrEmpty);
var ranges = lines.Take(emptyIx).Select(
        rangeStr => {
            var split = rangeStr.Split("-");
            return (long.Parse(split[0]), long.Parse(split[1]));
        }
    ).ToList();
var ingredients = lines.Skip(emptyIx + 1).Select(num => long.Parse(num)).ToList();

// Sort the ranges lexicographically, and ingredients is just a simple list
ranges.Sort();
ingredients.Sort();

long fresh = 0;
var rangeIx = 0;
foreach (var item in ingredients)
{
    // Find the first range that ends after the item, which is also the range
    // with the earliest start due to the sort
    while (rangeIx < ranges.Count - 1 && ranges[rangeIx].Item2 < item)
        rangeIx++;

    // If the item is in this range, it's fresh, otherwise move on...
    if (item >= ranges[rangeIx].Item1 && item <= ranges[rangeIx].Item2)
        fresh++;
}

// input      = 3
// input-full = 811
Console.WriteLine(fresh);

