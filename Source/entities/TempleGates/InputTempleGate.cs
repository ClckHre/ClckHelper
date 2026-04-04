using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;
using System.Reflection;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using System;

namespace Celeste.Mod.ClckHelper.Entities;

[CustomEntity("ClckHelper/InputTempleGate")]
public class InputTempleGate : BaseTempleGate
{
    bool disabled = false;
    VirtualButton input_button;
    private string input_string;
    public InputTempleGate(EntityData data, Vector2 offset) : base(data, offset)
    {
        input_string = data.String("input", "Grab");
        FieldInfo input_field = typeof(Input).GetField(input_string, BindingFlags.Static | BindingFlags.Public);
        if (input_field == null) {Logger.Log(LogLevel.Error, "ClckHelper/InputTempleGate", $"Input contains no property {input_string}"); disabled = true; return;}
        input_button = (VirtualButton)input_field.GetValue(null);
        input_button.BufferTime = 0f;
        input_button.canRepeat = false;
        input_button.Repeating = false;
    }

    public override void Update()
    {
        base.Update();
        if (disabled) return;
        if (input_button == null) {Logger.Log(LogLevel.Error, "ClckHelper/InputTempleGate", "input_button is null"); return;}
        if (input_button.Pressed)
        {
            ToggleOpenState();
        }
    }
}