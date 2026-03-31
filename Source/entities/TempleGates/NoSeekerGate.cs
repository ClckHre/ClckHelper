using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.ClckHelper.Entities;

[CustomEntity("ClckHelper/NoSeekerGate")]
public class NoSeekerGate : BaseTempleGate
{
    public NoSeekerGate(EntityData data, Vector2 offset) : base(data, offset) {}

    public override void Awake(Scene scene)
    {
        base.Awake(scene);
        Seeker entity = base.Scene.Tracker.GetEntity<Seeker>();
        if (entity == null) StartOpen();
    }
    public override void Update()
    {
        base.Update();

        Seeker entity = base.Scene.Tracker.GetEntity<Seeker>();
        if (entity == null && !get_openState()) Open();
        if (entity != null && get_openState()) Close();

    }
}