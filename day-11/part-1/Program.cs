var lines = File.ReadAllLines("../input-full.txt");

Dictionary<string, List<string>> devices =
    lines.Select(l => l.Split(": "))
        .Select(arr => new KeyValuePair<string, List<string>>(
            arr[0],
            arr[1].Split(' ').ToList()
        )).ToDictionary();

// Working backwards, keep track of fully resolved counts
var pathCounts = new Dictionary<string, long>();
pathCounts["out"] = 1;

// Iterate until all devices have been assigned a concrete path count
while (devices.Count != 0)
    foreach (var device in devices.ToArray())
        if (device.Value.All(o => pathCounts.ContainsKey(o)))
        {
            // This device only has outputs that are resolved, so add them up!
            pathCounts[device.Key] = device.Value.Select(o => pathCounts[o]).Sum();

            // Device is now computed and can be taken out of the unresolved dict.
            devices.Remove(device.Key);
        }

// input      = 5
// input-full = 508
Console.WriteLine(pathCounts["you"]);
