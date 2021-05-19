# Rocksmith2014PsarcLib
Yet another rocksmith2014 psarc reader lib

Designed to parse faster, but only supports unpacking

# Example usage
```cs
// Count total time elapsed
var totaltime = new Stopwatch();
totaltime.Start();

// A concurrent collection for the dlc keys
var allDlcKeys = new ConcurrentBag<string>();

// Find all files in the dlc folder and subfolders
var allfiles = Directory.GetFiles(@"G:\SteamLibrary\steamapps\common\Rocksmith2014\dlc", "*.psarc", SearchOption.AllDirectories).ToList();
// Manually add songs.psarc
allfiles.Add(@"G:\SteamLibrary\steamapps\common\Rocksmith2014\songs.psarc");

// Go through all dlc files in parallel
var partask = Parallel.ForEach<string>(allfiles, (file) =>
{
    // Unnecessary list just for printing how many dlc keys it found in each psarc
    var dlckeys = new List<string>();
    try
    {
        // Use psarclib to parse the psarc
        using (var psarc = new PsarcFile(file))
        {
            // Go through all manifests, add each arrangements song key to the list
            foreach (var manifest in psarc.ExtractArrangementManifests())
            {
                dlckeys.Add(manifest.Attributes.SongKey);
                allDlcKeys.Add(manifest.Attributes.SongKey);
            }
        }

        Console.WriteLine($"Found {dlckeys.Distinct().Count()} dlc in {file}");
    }
    // Handle exceptions, since corrupt cdlc is very common
    catch (Exception e)
    {
        Console.WriteLine($"Unable to read {file}");
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
    }
});

// Wait for the parallel loop to complete
while (!partask.IsCompleted)
{
    Thread.Sleep(100);
}

// Stop timer
totaltime.Stop();

// Remove all duplicate keys from the list, because all arrangements of a song have the same key
var result = allDlcKeys.ToArray().Distinct().ToArray();

Console.WriteLine($"Completed in {totaltime.ElapsedMilliseconds}ms");
Console.WriteLine($"Parsed {allfiles.Count} files");
Console.WriteLine($"Found {result.Length} song keys");

Console.ReadLine();
```
