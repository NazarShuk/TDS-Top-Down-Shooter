using System;
using System.Linq;
using Godot;
// Range: 340 px

namespace tdstopdownshooter.MainArea.SpawnArea;

public partial class SpawnArea : Node2D
{
    [Export]
    public float SpawnMaxRange = 340f;
    
    [Export]
    public float SpawnMinRange = 90f;
    
    private Timer _timer;
    private RandomNumberGenerator _rng;

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
        _rng.Randomize();
        _timer = GetNode<Timer>("Timer");
        
        if (_timer != null)
            _timer.Timeout += TimerOnTimeout;
        else
            GD.PushError("Timer not found.");
    }

    private string PickEnemy()
    {
        var totalWeight = Global.Enemies.Sum(e => e.weight);

        var pick = _rng.RandfRange(0f, totalWeight);
        var cumulative = 0f;

        foreach (var e in Global.Enemies)
        {
            cumulative += e.weight;
            if (pick <= cumulative)
                return e.path;
        }

        return Global.Enemies[^1].path;
    }

    private void TimerOnTimeout()
    {
        var enemyPath = PickEnemy();
        var enemyScene = ResourceLoader.Load<PackedScene>(enemyPath);
        var enemy = enemyScene.Instantiate<Node2D>();
        AddSibling(enemy);
        enemy.Position = new Vector2(0f, _rng.RandfRange(SpawnMinRange, SpawnMaxRange));

        _timer.WaitTime = MathF.Max(0.5f, Global.Difficulty * -0.1f + 10f);
    }
}
