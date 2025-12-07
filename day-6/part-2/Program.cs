var lines = File.ReadAllLines("../input-full.txt");

var numLines = lines.Length - 1;

long total = 0;
int operpos;
int lastpos = lines[0].Length - 1;
do
{
    operpos = lines[numLines].LastIndexOfAny(['+', '*'], lastpos);
    var oper = lines[numLines][operpos];

    long sum = oper == '+' ? 0 : 1;
    for (int ix = lastpos; ix >= operpos; ix--)
    {
        // Parse the vertical number in column "ix"
        string num = "";
        for (int iy = 0; iy < lines.Length - 1; iy++)
            num += lines[iy][ix];
        var value = long.Parse(num);

        sum = oper == '+' ? sum + value : sum * value;
    }

    total += sum;  
    lastpos = operpos - 2;
} while (operpos != 0);

// input      = 3263827
// input-full = 9625320374409
Console.WriteLine(total);
