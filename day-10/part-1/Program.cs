var lines = File.ReadAllLines("../input-full.txt");

var machines = new List<Machine>();
foreach (var line in lines)
{
    Machine m = new Machine();

    // Convert the light target state to a binary representation
    var split = line.Split(' ');
    var lights = split[0].Replace("[", "").Replace("]", "");
    m.LightTarget = Convert.ToInt32(
        new string(lights.Replace('#', '1').Replace('.', '0').Reverse().ToArray()),
        2);
    // Convert each button to a binary representation of the bit flips
    m.Buttons = split.Skip(1).Take(split.Length - 2)
        .Select(
            b => b.Trim(['(', ')'])
                .Split(',')
                .Sum(n => 1 << int.Parse(n))
        ).ToList();

    machines.Add(m);
}

long total = 0;
foreach (var machine in machines)
{
    // Keep track of the best number of presses to get to a state
    // ... and note the initial state as taking 0 presses
    var bests = new Dictionary<int, int>();
    bests[machine.LightTarget] = 0;

    // While the best answer for "0" (empty light state) is not set, keep going
    while (bests.GetValueOrDefault(0, -1) == -1)
    {
        // For every pattern we have so far, press every button once
        foreach (var pattern in bests.Keys.ToArray())
            foreach (var button in machine.Buttons)
            {
                // The new pattern is one further than the pattern it was
                // created from.
                var newPattern = pattern ^ button;
                if (!bests.ContainsKey(newPattern))
                    bests[newPattern] = bests[pattern] + 1;
            }
    }

    // Add the best button press count to get to 0 to the running total
    total += bests[0];
} 

// input      = 7
// input-full = 530
Console.WriteLine(total);

public struct Machine
{
    public int LightTarget { get; set; }
    public List<int> Buttons { get; set; }
}
