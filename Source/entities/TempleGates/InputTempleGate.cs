using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using System;

namespace Celeste.Mod.ClckHelper.Entities;

[CustomEntity("ClckHelper/InputTempleGate")]
public class InputTempleGate : BaseTempleGate
{
    bool disabled = false;
    VirtualButton input_button;
    public InputTempleGate(EntityData data, Vector2 offset) : base(data, offset)
    {
        string binding_key_string = data.String("key");
        string binding_button_string = data.String("controller_button");
        Binding binding = new Binding();
        if (Enum.GetNames<Keys>().Contains(binding_key_string)) binding.Add(Enum.Parse<Keys>(binding_key_string, true));
        else Logger.Log(LogLevel.Warn, "TempleGateHelper/InputTempleGate", $"Invalid keyboard key selected for entity {data.ID}");
        if (Enum.GetNames<Buttons>().Contains(binding_button_string)) binding.Add(Enum.Parse<Buttons>(binding_button_string, true));
        else Logger.Log(LogLevel.Warn, "TempleGateHelper/InputTempleGate", $"Invalid controller button selected for entity {data.ID}");
        input_button = new VirtualButton(binding, Input.Gamepad, 0f, 0.2f);
    }

    public override void Update()
    {
        base.Update();
        if (disabled) return;
        if (input_button == null) {Logger.Log(LogLevel.Error, "TempleGateHelper/InputTempleGate", $"input_button is null"); return;}
        if (input_button.Pressed)
        {
            ToggleOpenState();
        }
    }
}