var lines = File.ReadAllLines("../input-full.txt");

var ranges = lines[0].Split(',').Select(range =>
    {
        var nums = range.Split('-');
        return (long.Parse(nums[0]), long.Parse(nums[1]));
    });

long total = 0;
foreach (var (start, end) in ranges)
    for (long num = start; num <= end; num++)
    {
        var numstr = num.ToString();
        int len = numstr.Length;

        if (len % 2 == 0)
        {
            var slice = len / 2;
            var chunks = numstr.Chunk(slice).Select(i => new string(i)).ToArray();
            if (chunks.All(i => i == chunks[0]))
            {
                total += num;
            }
        }
    }

// input      = 1227775554
// input-full = 12850231731
Console.WriteLine(total);
