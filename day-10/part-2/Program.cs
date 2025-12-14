var lines = File.ReadAllLines("../input-full.txt");

var machines = new List<Machine>();
foreach (var line in lines)
{
    var split = line.Split(' ');
    var buttons = split.Skip(1).Take(split.Length - 2)
        .Select(
            b => b.Trim(['(', ')'])
                .Split(',')
                .Select(n => int.Parse(n))
                .ToArray()
        ).OrderBy(b => -b.Length).ToList();
    var joltage = split.Last().Trim(['{', '}']).Split(',').Select(n => int.Parse(n)).ToArray();

    Machine m = new Machine
    {
        Buttons = buttons,
        Joltage = joltage,

        JixToBix = new List<int[]>(),
        OrderedPartition = new List<(int, int)>(),
        ButtonIxOrder = new List<int>(),

        BestPresses = int.MaxValue,
        Solution = new int[buttons.Count],
        JoltageSummers = new SumJoltage[joltage.Length],
        Checksum = () => true
    };

    machines.Add(m);
}

// Generate all N-tuples that sum to X
IEnumerable<int[]> GenerateTuples(int n, int x, int[] limit)
{
    int[] tuple = new int[n];
    int[] limitSums = new int[n];
    for (int ix = 0; ix < n; ix++)
        limitSums[ix] = limit.Skip(ix).Take(n - ix).Sum();

    IEnumerable<int[]> Iterate(int ix, int remainder)
    {
        // Escape if there are no assignments that fit the remainder under the limit
        if (limitSums[ix] < remainder) yield break;

        if (ix == n - 1)
        {
            tuple[ix] = remainder;
            yield return tuple;
        }
        else
            // Skip over assignments that break the limit in this position
            for (int v = Math.Min(remainder, limit[ix]); v >= 0; v--)
            {
                tuple[ix] = v;
                foreach (var result in Iterate(ix + 1, remainder - v))
                    yield return result;
            }
    }

    foreach (var result in Iterate(0, x))
        yield return result;
}

void PreprocessMachine(Machine m)
{
    // Calculate m.JixToBix, so that: m.JixToBix[jIx] = bIx of m.Buttons that contribute to Joltage at jIx
    for (int jIx = 0; jIx < m.Joltage.Length; jIx++)
        m.JixToBix.Add(m.Buttons.Select((b, bIx) => (b, bIx)).Where(p => p.b.Contains(jIx)).Select(p => p.bIx).ToArray());

    // Calculate m.OrderedPartition, so that: (target jIx, bIx contributing to jIx that were not earlier in the list)
    List<int> doneJix = new List<int>();
    List<int> doneBix = new List<int>();
    while (doneBix.Count < m.Buttons.Count)
    {
        // Keep going until all buttons are assigned to a partition
        var bestLength = int.MaxValue;
        var bestJoltage = int.MaxValue;
        var bestJix = 0;
        for (var jIx = 0; jIx < m.Joltage.Length; jIx++)
            if (!doneJix.Contains(jIx))
            {
                var bIxCount = m.JixToBix[jIx].Except(doneBix).Count();
                if (bIxCount == 0) continue;

                if ((bIxCount < bestLength) || (bIxCount == bestLength && m.Joltage[jIx] < bestJoltage))
                {
                    // Fewer buttons is much better, but otherwise less joltage is slightly better
                    bestLength = bIxCount;
                    bestJoltage = m.Joltage[jIx];
                    bestJix = jIx;
                }
            }

        var bIxArr = m.JixToBix[bestJix].Except(doneBix).ToArray();
        m.OrderedPartition.Add((bestJix, bIxArr.Length));
        m.ButtonIxOrder.AddRange(bIxArr);

        doneJix.Add(bestJix);
        doneBix.AddRange(bIxArr);
    }

    // Pre-allocate summers for each joltage index
    for (int jIx = 0; jIx < m.Joltage.Length; jIx++)
        m.JoltageSummers[jIx] = m.CreateSummer(jIx);

    // Build a checksum function that checks the joltage output on each jIx not in OrderedPartition
    var checks = m.Joltage.Select((j, jIx) => (j, jIx))
        .Where(p => !m.OrderedPartition.Any(p2 => p2.Item1 == p.jIx)).ToArray();
    m.Checksum = () => checks.All(p => m.JoltageSummers[p.jIx]() <= p.j);
}

var zeroes = new int[100];
void SolvePartition(Machine m, int partIx, int solutionIx, int totalPresses)
{
    if (solutionIx == m.ButtonIxOrder.Count)
    {
        if (!m.Joltage.Where((j, jIx) => m.JoltageSummers[jIx]() != j).Any())
            if (totalPresses < m.BestPresses) m.BestPresses = totalPresses;
    }
    else
    {
        int jIx = m.OrderedPartition[partIx].Item1;
        int bCount = m.OrderedPartition[partIx].Item2;

        // Calculate the iteration input
        int pressCount = m.Joltage[jIx] - m.JoltageSummers[jIx]();
        if (pressCount < 0) return;
        if (totalPresses + pressCount > m.BestPresses) return;

        var limits = m.ButtonIxOrder.Skip(solutionIx).Take(bCount)
            .Select(bIx => m.Buttons[bIx].Min(jIx => m.Joltage[jIx] - m.JoltageSummers[jIx]()))
            .ToArray();

        foreach (var presses in GenerateTuples(bCount, pressCount, limits))
        {
            presses.CopyTo(m.Solution, solutionIx);
            if (!m.Checksum()) continue;

            SolvePartition(m, partIx + 1, solutionIx + bCount, totalPresses + pressCount);
        }

        // Clear out the portion of the solution we are recursing out of
        Array.Copy(zeroes, 0, m.Solution, solutionIx, bCount);
    }
}


long total = 0;
int l = 1;
foreach (var machine in machines)
{
    // Prepare button partition for the machine
    PreprocessMachine(machine);
    SolvePartition(machine, 0, 0, 0);

    Console.WriteLine($"{l:000} = {machine.BestPresses}");

    total += machine.BestPresses;
    l++;
}

// input      = 33
// input-full = 20172
Console.WriteLine(total);

public delegate int SumJoltage();
public delegate bool CheckJoltage();
public class Machine
{
    // Input values
    public required List<int[]> Buttons { get; set; }
    public required int[] Joltage { get; set; }

    // Calculated search space parameters
    public required List<int[]> JixToBix { get; set; } // Lookup from Joltage IX to array of contributing button IX
    public required List<(int, int)> OrderedPartition { get; set; } // Simplest jIx first, each paired number of buttons
    public required List<int> ButtonIxOrder { get; set; }

    // Solution accumulators
    public required int BestPresses { get; set; }
    public required int[] Solution { get; set; }
    public required SumJoltage[] JoltageSummers { get; set; }
    public required CheckJoltage Checksum { get; set; }

    public SumJoltage CreateSummer(int jIx)
    {
        // Calculate the solution offsets representing buttons contributing to this joltage IX
        var bIxOffsets = JixToBix[jIx].Select(bIx => ButtonIxOrder.IndexOf(bIx)).ToArray();

        // Return a fast summation delegate
        return () => bIxOffsets.Sum(o => Solution[o]);
    }
}
