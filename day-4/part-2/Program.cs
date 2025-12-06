var lines = File.ReadAllLines("../input-full.txt");

// Expand the map with a border of dots to simplify further logic
string[] blank = [new string('.', lines[0].Length + 2)];
lines = blank.Concat(lines.Select(line => $".{line}.")).Concat(blank).ToArray();

// Convert lines to a 2D character array
char[][] map = lines.Select(line => line.ToArray()).ToArray();

int CountRolls(int ix, int iy)
{
    (int,int)[] offsets = [
        (-1, -1), (-1, 0), (-1, 1),
        (0, -1), (0, 1),
        (1, -1), (1, 0), (1, 1)];

    return offsets.Select(
        offset => map[iy + offset.Item2][ix + offset.Item1] == '@' ? 1 : 0
    ).Sum();
}

long OnePassCountAndRemove()
{
    long pass = 0;
    for (int iy = 1; iy < lines.Length - 1; iy++)
        for (int ix = 1; ix < lines[iy].Length - 1; ix++)
            if (map[iy][ix] == '@')
                if (CountRolls(ix, iy) < 4)
                {
                    pass++;
                    map[iy][ix] = '.';
                }

    return pass;
}

long total = 0;
long pass = 0;
do
{
    pass = OnePassCountAndRemove();
    total += pass;
} while (pass != 0);

// input      = 43
// input-full = 9024
Console.WriteLine(total);
