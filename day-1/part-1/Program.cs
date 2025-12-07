var lines = File.ReadAllLines("../input-full.txt");

var deltas = lines.Select(line =>
    {
        var num = long.Parse(line.Substring(1));
        return line[0] == 'L' ? -num : num;
    }).ToArray();

var total = 0;
long position = 50;
foreach (var delta in deltas)
{
    position = (position + delta) % 100;
    if (position == 0) total++;
}

// input      = 3
// input-full = 1089
Console.WriteLine(total);
