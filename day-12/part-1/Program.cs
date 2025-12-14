var lines = File.ReadAllLines("../input-full.txt");

// Trees = List<(int[] dimensions, int[] present_counts)>
var trees = lines.Skip(30).Select(tree => tree.Split(": "))
    .Select(t => 
        (
            t[0].Split("x").Select(d => int.Parse(d)).ToArray(),
            t[1].Split(" ").Select(n => int.Parse(n)).ToArray()
        )
    ).ToList();

int[] shapeSize = {5, 7, 7, 7, 7, 6};

var impossible = 0;
var trivial = 0;
foreach (var tree in trees)
{
    // Dimensions and make a map indicating the shape variant in each space
    int w = tree.Item1[0];
    int h = tree.Item1[1];
    int tw = w / 3;
    int th = h / 3;

    int shapeArea = tree.Item2.Select((s, ix) => s * shapeSize[ix]).Sum();
    // Shortcuts for easy trees
    if (tree.Item2.Sum() <= tw * th)
        trivial++;
    else if (shapeArea > (w * h))
        impossible++;
    else
        Console.WriteLine("ERROR");
}

// input      = 2
// input-full = 587 --- no ERRORs, so no need to create a real calculation
Console.WriteLine($"Impossible: {impossible}, Trivial: {trivial}");
