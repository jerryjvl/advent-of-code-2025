var lines = File.ReadAllLines("../input-full.txt");

var numLines = lines.Length - 1;
var numbers = lines.Take(numLines).Select(line =>
    line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(num => long.Parse(num)).ToArray()).ToList();

var operators = lines[numLines].Split(' ', StringSplitOptions.RemoveEmptyEntries);

long total = 0;
for (int ix = 0; ix < operators.Length; ix++)
{
    long sum = operators[ix] == "+" ? 0 : 1;
  
    for (int iy = 0; iy < numLines; iy++)
        sum = operators[ix] == "+" ? sum + numbers[iy][ix] : sum * numbers[iy][ix];

    total += sum;
}

// input      = 4277556
// input-full = 4387670995909
Console.WriteLine(total);
