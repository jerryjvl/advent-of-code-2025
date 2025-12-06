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

        for (var splits = 2; splits <= len; splits++)
            if (len % splits == 0)
            {
                var slice = len / splits;
                var chunks = numstr.Chunk(slice).Select(i => new string(i)).ToArray();
                if (chunks.All(i => i == chunks[0]))
                {
                    total += num;
                    break;
                }
            }
    }

// input      = 4174379265
// input-full = 24774350322
Console.WriteLine(total);
