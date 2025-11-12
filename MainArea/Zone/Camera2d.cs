using Godot;
using System;

public partial class Camera2d : Camera2D
{
    [Export]
    private Shaker _shaker;

    private int _previousHealth = Global.Health;
    
    public override void _Process(double delta)
    {
        if (_previousHealth != Global.Health)
        {
            _previousHealth = Global.Health;
            _shaker.Shake();
            GD.Print("health changed");
        }
    }
}
