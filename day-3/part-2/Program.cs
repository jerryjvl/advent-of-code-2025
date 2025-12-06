var lines = File.ReadAllLines("../input-full.txt");

// Problem dimension
const int DIGITS = 12;
var banks = lines.Select(line => line.ToArray()).ToArray();

long total = 0;
foreach (var bank in banks)
{
    string number = "";

    var ix = 0;
    var remainder = bank.Length - DIGITS + 1;
    for (int iter = 0; iter < DIGITS; iter++)
    {
        var segment = bank.Skip(ix).Take(remainder).ToList();
        var max = segment.Max();
        var offset = segment.IndexOf(max);

        number = number + max;

        ix += offset + 1;
        remainder -= offset;
    }

    total += long.Parse(number);
}

// input      = 3121910778619
// input-full = 173065202451341
Console.WriteLine(total);
