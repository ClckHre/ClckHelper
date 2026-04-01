using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.ClckHelper.Entities;

[CustomEntity("ClckHelper/PlayerNearbyGate")]
public class PlayerNearbyGate : BaseTempleGate
{
    public float open_radius;
    public float close_radius;
    public PlayerNearbyGate(EntityData data, Vector2 offset) : base(data, offset)
    {
        open_radius = data.Float("open_radius", 64f); 
        close_radius = data.Float("close_radius", 80f);
    }

    
    public override void Awake(Scene scene)
    {
        base.Awake(scene);
        if (IsNearby<Player>(open_radius)) StartOpen();
 
    }
    
    
    public override void Update()
    {
        base.Update();

        if (!IsNearby<Player>(close_radius) && get_openState()) Close();
        if (IsNearby<Player>(open_radius) && !get_openState()) Open();

    }

    public bool IsNearby<T>(float radius=64f) where T : Entity
	{
		bool TIsNearby = false;
		foreach (T entity in base.Scene.Tracker.GetEntities<T>()) {
			if (entity != null && !TIsNearby)
			{
				TIsNearby = Vector2.Distance(Position + new Vector2(base.Width / 2f, closedHeight / 2), entity.Center) < radius;
				if (TIsNearby) break;
			}
		}
		return TIsNearby;
	}
}