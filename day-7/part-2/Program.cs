var lines = File.ReadAllLines("../input-full.txt");

// Work from the bottom to the top, accumulating universes, starting with
// 1 universe in each column.
var width = lines[0].Length;
long[] universes = Enumerable.Repeat<long>(1, width).ToArray();

foreach (var line in lines.Reverse())
{
    var newUniverses = (long[])universes.Clone();

    for (int ix = 0; ix < width; ix++)
        if (line[ix] == '^')
            newUniverses[ix] = universes[ix - 1] + universes[ix + 1];

    universes = newUniverses;
}

// input      = 40
// input-full = 7759107121385
Console.WriteLine(universes[lines[0].IndexOf('S')]);
