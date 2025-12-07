var lines = File.ReadAllLines("../input-full.txt");

long total = 0;
var acc = lines[0].Select(c => c == 'S' ? '|' : '.').ToArray();
foreach (var line in lines.Skip(1))
{
    var newAcc = new string('.', line.Length).ToArray();

    for (var ix = 0; ix < line.Length; ix++)
        if (acc[ix] == '|')
            switch (line[ix])
            {
                case '.':
                    newAcc[ix] = '|';
                    break;
                case '^': 
                    newAcc[ix - 1] = '|';
                    newAcc[ix + 1] = '|';
                    total++;
                    break;
            }

    acc = newAcc;
}

// input      = 21
// input-full = 1638
Console.WriteLine(total);
