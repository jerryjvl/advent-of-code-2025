var lines = File.ReadAllLines("../input-full.txt");

var coords = lines.Select(line => line.Split(',')).Select(arr => (long.Parse(arr[0]), long.Parse(arr[1]))).ToList();

// Find just the verticals, and re-order coordinates in increasing Y
var circuit = coords.Concat([coords[0]]).ToList();
var verticals = new List<(long, long, long)>();
for (int i = 0; i < circuit.Count - 1; i++)
{
    var fst = circuit[i];
    var sec = circuit[i + 1];

    // Skip horizontal segments
    if (fst.Item2 == sec.Item2) continue;

    var y1 = Math.Min(fst.Item2, sec.Item2);
    var y2 = Math.Max(fst.Item2, sec.Item2);

    if (fst.Item1 != sec.Item1)
    {
        Console.WriteLine("ERROR");
    }

    verticals.Add((y1, fst.Item1, y2));
}
verticals.Sort();

// Build a scanline conversion of the map:
// map[Y] = List<(X1, X2)>
var map = new Dictionary<long, List<(long, long)>>();
for (int y = 0; y < 100000; y++)
{
    long lastX = 0;
    bool inside = false;
    foreach (var edge in verticals.Where(v => v.Item1 <= y && v.Item3 > y).OrderBy(v => v.Item2))
    {
        if (!map.ContainsKey(y)) map[y] = new List<(long, long)>();
        if (inside)
            map[y].Add((lastX, edge.Item2));
        
        lastX = edge.Item2;
        inside = !inside;
    }
}

// List of coordinate index pairs ordered by size of the area it creates
var sortedPairs =
    Enumerable.Range(0, coords.Count)
        .SelectMany(i => Enumerable.Range(0, coords.Count).Select(j => (i, j)))
        .Where(p => p.i < p.j)
        .OrderBy(p =>
        {
            var fst = coords[p.i];
            var sec = coords[p.j];

            var x1 = Math.Min(fst.Item1, sec.Item1);
            var x2 = Math.Max(fst.Item1, sec.Item1);
            var y1 = Math.Min(fst.Item2, sec.Item2);
            var y2 = Math.Max(fst.Item2, sec.Item2);

            long area = (x2 - x1 + 1) * (y2 - y1 + 1);
            return area;
        });

long max = 0;
foreach (var ixPair in sortedPairs)
{
    var fst = coords[ixPair.i];
    var sec = coords[ixPair.j];

    var x1 = Math.Min(fst.Item1, sec.Item1);
    var x2 = Math.Max(fst.Item1, sec.Item1);
    var y1 = Math.Min(fst.Item2, sec.Item2);
    var y2 = Math.Max(fst.Item2, sec.Item2);

    long area = (x2 - x1 + 1) * (y2 - y1 + 1);

    // Check if the square is valid
    bool valid = true;
    for (var y = y1; y < y2 && valid; y++)
    {
        // Find a segment in the map that encloses [x1, x2]
        if (map[y].Where(s => s.Item1 <= x1 && s.Item2 >= x2).Count() == 0)
            valid = false;
    }

    if (valid) max = area;
}

// input      = 24
// input-full = 1578115935
Console.WriteLine(max);
