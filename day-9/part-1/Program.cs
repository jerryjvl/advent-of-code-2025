var lines = File.ReadAllLines("../input-full.txt");

var coords = lines.Select(line => line.Split(',')).Select(arr => (long.Parse(arr[0]), long.Parse(arr[1]))).ToList();

long max = 0;
for (int ix = 0; ix < coords.Count; ix++)
{
    for (int iy = ix + 1; iy < coords.Count; iy++)
    {
        var fst = coords[ix];
        var sec = coords[iy];

        var x1 = Math.Min(fst.Item1, sec.Item1);
        var x2 = Math.Max(fst.Item1, sec.Item1);
        var y1 = Math.Min(fst.Item2, sec.Item2);
        var y2 = Math.Max(fst.Item2, sec.Item2);

        long area = (x2 - x1 + 1) * (y2 - y1 + 1);
        if (area > max) max = area;
    }
}

// input      = 50
// input-full = 4750297200
Console.WriteLine(max);
