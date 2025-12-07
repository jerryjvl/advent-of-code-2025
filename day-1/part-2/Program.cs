var lines = File.ReadAllLines("../input-full.txt");

var deltas = lines.Select(line =>
    {
        var num = int.Parse(line.Substring(1));
        return line[0] == 'L' ? -num : num;
    }).ToArray();

var total = 0;
long position = 50;
foreach (var delta in deltas)
{
    var next = position + delta;

    for (long num = Math.Min(position, next); num <= Math.Max(position, next); num++)
        if (num != position && num % 100 == 0) total++;

    position = (next + 1000000) % 100;
}

// input      = 6
// input-full = 6530
Console.WriteLine(total);
