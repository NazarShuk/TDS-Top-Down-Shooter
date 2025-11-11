using System;
using Godot;

namespace tdstopdownshooter.Enemy.EnemyBasic;

[GlobalClass]
public partial class HealthHandler : Node
{
    [Export] public int MaximumHealth = 100;
    [Export] public int CurrentHealth = 100;

    private ProgressBar _progressBar;
    
    public override void _Ready()
    {
        _progressBar = GetNode<ProgressBar>("../EnemyHpBar");

        if (_progressBar != null)
        {
            _progressBar.MaxValue = MaximumHealth;
            _progressBar.Value = CurrentHealth;
        }
        else
            GD.PushError("ProgressBar not found.");
    }

    public void UpdateProgressBar()
    {
        _progressBar.MaxValue = MaximumHealth;
        _progressBar.Value = CurrentHealth;
    }
    
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        
        GD.Print($"{damage} Damage Taken.");
        
        if (CurrentHealth <= 0)
            Kill();
    }

    private void Kill()
    {
        Global.Money += (int)Math.Round(5.0 * Math.Pow(5.0, 0.2 * Global.Difficulty), MidpointRounding.AwayFromZero);
        Global.Difficulty++;
        
        Die();
    }

    public void Die()
    {
        GD.Print("Enemy Died.");
        Global.Save();
        
        GetParent<Node2D>().QueueFree();
    }
}