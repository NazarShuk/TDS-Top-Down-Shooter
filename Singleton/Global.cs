using Godot;

public partial class Global : Node
{
    private static Global Instance { get; set; }
    public static int Difficulty;
    public static long Money;
    public static int Health = 1000;
    public static float Cooldown = 1.0f;
    public static int Damage = 10;
    public static float Radius = 50.0f;

    // Info:
    public static readonly (string path, float weight)[] Enemies =
    [
        ("res://Enemy/EnemyBasic/EnemyBasic.tscn", .7f),
        ("res://Enemy/EnemySpeedy/EnemySpeedy.tscn", .3f)
    ];

public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            GD.PushError("Global instance already exists.");
    }

    public static void Save()
    {
        using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Write);

        saveFile.StoreVar(Difficulty);
        saveFile.StoreVar(Money);
        saveFile.StoreVar(Health);
        saveFile.StoreVar(Cooldown);
        saveFile.StoreVar(Damage);
        saveFile.StoreVar(Radius);
        saveFile.Close();
    }

    public static void Load()
    {
        if (!FileAccess.FileExists("user://savegame.save"))
            return;
        
        using var saveFile = FileAccess.Open("user://savegame.save", FileAccess.ModeFlags.Read);

        if (saveFile == null)
            return;

        Difficulty = (int)saveFile.GetVar();
        Money = (long)saveFile.GetVar();
        Health = (int)saveFile.GetVar();
        Cooldown = (float)saveFile.GetVar();
        Damage = (int)saveFile.GetVar();
        Radius = (float)saveFile.GetVar();
    }
}
