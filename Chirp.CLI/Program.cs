var dbPath = "cheepDB.csv";

if (args[0] == "cheep" && args.Length == 2) 
{
    var cheep = args[1];
    var username = Environment.UserName;
    var date = DateTime.Now;
    
    using var sw = new StreamWriter(dbPath, true);
    var line = $"{date},{username},{cheep}";
    sw.WriteLine(line);
}

if (args[0] == "read" && args.Length == 1)
{
    using var sr = new StreamReader(dbPath);
    for (var line = sr.ReadLine(); line != null; line = sr.ReadLine()) 
    {
        var data = line.Split(',', 3);
        Console.WriteLine($"{data[0]} @ {data[1]}: {data[2]}");
    }
}