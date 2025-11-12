using Godot;
using System;
using System.Runtime.InteropServices.ComTypes;

public partial class Shaker : Node
{
    [Export] private Node _target;
    [Export] private StringName _variable;
    
    [Export] private Curve _curve;
    [Export] private float _duration;
    [Export] private float _strength;

    private bool _shaking = false;
    private float _time = 0;
    
    private RandomNumberGenerator _rng;
    private Variant _initialValue;

    public override void _Ready()
    {
        _rng = new RandomNumberGenerator();
        _rng.Randomize();
    }

    public void Shake()
    {
        _initialValue = _target.Get(_variable);
        _time = 0;
        _shaking = true;
    }

    public override void _Process(double delta)
    {
        if (_shaking)
        {
            _time += (float) delta;

            if (_time >= _duration)
            {
                _shaking = false;
                _time = 0;
                _target.Set(_variable, _initialValue);
                return;
            }
            var variable = _target.Get(_variable);
            
            switch (variable.VariantType)
            {
                case Variant.Type.Float:
                    _target.Set(_variable, _rng.RandfRange(-_strength, _strength) * _curve.Sample(_time /  _duration));
                    break;
                case Variant.Type.Int:
                    _target.Set(_variable, Math.Round(_rng.RandfRange(-_strength, _strength) * _curve.Sample(_time / _duration)));
                    break;
                case Variant.Type.Vector2:
                    _target.Set(_variable,
                        new Vector2(_rng.RandfRange(-_strength, _strength) * _curve.Sample(_time / _duration),
                            _rng.RandfRange(-_strength, _strength) * _curve.Sample(_time / _duration)));
                    break;
                case Variant.Type.Vector3:
                    _target.Set(_variable,
                        new Vector3(_rng.RandfRange(-_strength, _strength) * _curve.Sample(_time /  _duration), _rng.RandfRange(-_strength, _strength) * _curve.Sample(_time /  _duration), _rng.RandfRange(-_strength, _strength) * _curve.Sample(_time /  _duration)));
                    break;
                default:
                    GD.Print("yo idk what ts veriable is");
                    break;
            }
        }
    }
}
