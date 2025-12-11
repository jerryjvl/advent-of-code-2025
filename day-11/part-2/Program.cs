var lines = File.ReadAllLines("../input-full.txt");

Dictionary<string, List<string>> devices =
    lines.Select(l => l.Split(": "))
        .Select(arr => new KeyValuePair<string, List<string>>(
            arr[0],
            arr[1].Split(' ').ToList()
        )).ToDictionary();

// Now we also keep attribution for "dac?" and "fft?"
var pathCounts = new Dictionary<string, Dictionary<(bool, bool), long>>();
pathCounts["out"] = new Dictionary<(bool, bool), long>();
pathCounts["out"][(false, false)] = 1;

// Pre-made iterator for all the dac/fft flag combinations
(bool,bool)[] conditions = {(false, false), (false, true), (true, false), (true, true)};

while (devices.Count != 0)
    foreach (var device in devices.ToArray())
        if (device.Value.All(o => pathCounts.ContainsKey(o)))
        {
            // This device only has outputs that are resolved, so add them up!
            pathCounts[device.Key] = new Dictionary<(bool, bool), long>();

            // Sum the downstream by each conditional combo (defaulting to 0)
            foreach (var c in conditions)
                pathCounts[device.Key][c] = device.Value.Select(
                    o => pathCounts[o].GetValueOrDefault(c)).Sum();

            if (device.Key == "dac")
            {
                // Pin first conditional to true
                pathCounts["dac"][(true, false)] += pathCounts["dac"][(false, false)];
                pathCounts["dac"][(true, true)]  += pathCounts["dac"][(false, true)];
                pathCounts["dac"][(false, false)] = 0;
                pathCounts["dac"][(false, true)]  = 0;
            }
            else if (device.Key == "fft")
            {
                // Pin second conditional to true
                pathCounts["fft"][(false, true)] += pathCounts["fft"][(false, false)];
                pathCounts["fft"][(true, true)]  += pathCounts["fft"][(true, false)];
                pathCounts["fft"][(false, false)] = 0;
                pathCounts["fft"][(true, false)]  = 0;
            }

            // Device is now computed and can be taken out of the unresolved dict.
            devices.Remove(device.Key);
        }

// input2     = 2
// input-full = 315116216513280
Console.WriteLine(pathCounts["svr"][(true, true)]);
