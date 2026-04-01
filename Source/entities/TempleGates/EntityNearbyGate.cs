using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

namespace Celeste.Mod.ClckHelper.Entities;

[CustomEntity("ClckHelper/EntityNearbyGate")]
public class EntityNearbyGate : BaseTempleGate
{
    public string EntitySID;
    public float open_radius;
    public float close_radius;
    public EntityNearbyGate(EntityData data, Vector2 offset) : base(data, offset)
    {
        EntitySID = data.String("SID", "theoCrystal");
        open_radius = data.Float("open_radius", 64f);
        close_radius = data.Float("close_radius", 80f);
    }

    
    public override void Awake(Scene scene)
    {
        base.Awake(scene);
        if (IsNearby(EntitySID, open_radius)) StartOpen();
 
    }
    
    
    public override void Update()
    {
        base.Update();

        if (!IsNearby(EntitySID, close_radius) && get_openState()) Close();
        if (IsNearby(EntitySID, open_radius) && !get_openState()) Open();
    }

	public bool IsNearby(string Name, float radius=64f)
	{
		//EntityRegistry.GetKnownSidsFromType()
		
		bool EntityIsNearby = false;
		foreach (Entity entity in base.Scene.FindEntitiesWithSid(Name)) {
			if (entity == null) continue;
			if (!EntityIsNearby)
			{
				EntityIsNearby = Vector2.Distance(Position + new Vector2(base.Width / 2f, closedHeight / 2), entity.Center) < radius;
				if (EntityIsNearby) break;
			}
		}
		return EntityIsNearby;
	}
}