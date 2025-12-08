var lines = File.ReadAllLines("../input-full.txt");

var coords = lines.Select(line => line.Split(',').Select(c => long.Parse(c)).ToArray()).ToList();
var circuits = new List<HashSet<int>>();

// Calculate distance table once
long Square(long v) { return v * v; }
long[,] d = new long[coords.Count, coords.Count];
for (int f = 0; f < coords.Count; f++)
    for (int s = f; s < coords.Count; s++)
        if (s == f)
        {
            d[s, f] = long.MaxValue;            
        }
        else
        {
            var dist =
                Square(coords[f][0] - coords[s][0]) +
                Square(coords[f][1] - coords[s][1]) +
                Square(coords[f][2] - coords[s][2]);
            d[s, f] = dist;
            d[f, s] = dist;
        }

// Sort all pairs by distance
var sortedPairs =
    Enumerable.Range(0, coords.Count)
        .SelectMany(i => Enumerable.Range(0, coords.Count).Select(j => (i, j)))
        .Where(p => p.i < p.j)
        .OrderBy(p => d[p.i, p.j]);

foreach (var pair in sortedPairs)
{
    // Find existing circuits (or invent new single-node ones)
    var seti = circuits.Find(set => set.Contains(pair.i)) ?? new HashSet<int>([pair.i]);
    var setj = circuits.Find(set => set.Contains(pair.j)) ?? new HashSet<int>([pair.j]);

    // Remove the existing sets, and re-add the joined set
    circuits.Remove(seti);
    circuits.Remove(setj);

    var joined = seti.Union(setj).ToHashSet();
    circuits.Add(joined);

    // Final join?
    if (joined.Count == coords.Count)
    {
        // input      = 25272
        // input-full = ?
        Console.WriteLine(coords[pair.i][0] * coords[pair.j][0]);
        break;
    }
}
