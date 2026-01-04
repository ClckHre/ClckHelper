using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;
namespace Celeste.Mod.ExampleMod.Entities;

[CustomEntity("ClckHre/ClckHelper/BrainfuckManager")]
public class BrainfuckManager : Entity
{
    public string flag;
    public string alphabet;
    

    public string code = "";
    public string output = "";
    public BrainfuckManager(EntityData data, Vector2 offset)
    {
        flag = data.String("flag");
        alphabet = data.String("alphabet");
        
    }
}