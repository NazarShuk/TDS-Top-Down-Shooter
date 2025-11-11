using Godot;

namespace tdstopdownshooter.Enemy.EnemyBasic;

[GlobalClass]
public partial class EnemyHpBar : ProgressBar
{
    [Export] Color _green = new Color(0, 0.478f, 0);
    [Export] Color _yellow = new Color(0.497f, 0.44f, 0);
    [Export] Color _red = new Color(0.743f, 0, 0);
    
    HealthHandler _healthHandler;
    StyleBoxFlat _fillStyle = new StyleBoxFlat();

    public override void _Ready()
    {
        _fillStyle.BgColor = new Color(0, 1, 0);
        AddThemeStyleboxOverride("fill", _fillStyle);

        var possibleHealthHandler = GetNode<Node>("../HealthHandler");
        
        if (possibleHealthHandler is HealthHandler healthHandler)
        {
            _healthHandler = healthHandler;
        }
        else
        {
            GD.PushError("HealthHandler not found.");
        }
    }

    Color CalculateHeatColor()
    {

        var heat = 1f - Mathf.Clamp(
            (float)_healthHandler.CurrentHealth / _healthHandler.MaximumHealth,
            0f, 1f
            );

        if (heat < 0.5)
        {
            var t = heat / 0.5f;
            return _green.Lerp(_yellow, t);
        }
        else
        {
            var t = (heat - 0.5f) / 0.5f;
            return _yellow.Lerp(_red, t);
        }
    }

    
    public override void _Process(double delta)
    {
        Value = Mathf.Lerp(Value, _healthHandler.CurrentHealth, 10.0 * delta);
        
        _fillStyle.BgColor = CalculateHeatColor();
        
        
    }
}