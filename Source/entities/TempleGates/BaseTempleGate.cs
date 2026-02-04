using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

using System;
using System.Runtime.CompilerServices;

namespace Celeste.Mod.ClckHelper.Entities;

public class BaseTempleGate : Solid {
	public string LevelID;

	public bool ClaimedByASwitch;

	private bool theoGate;

	public int closedHeight;

	private Sprite sprite;

	private Shaker shaker;

	public float drawHeight;

	private float drawHeightMoveSpeed;
	public bool inverted = false;
	public bool openState;
	public Vector2 NearbyCheckFrom;

	public BaseTempleGate(EntityData data, Vector2 offset) : base(data.Position + offset, 8f, 1, safe: true)
	{
		string spriteName = data.String("sprite", "TempleGate_default");
		closedHeight = data.Int("height", 40);
		inverted = data.Bool("inverted", false);


		LevelID = data.Level.Name;
		Add(sprite = GFX.SpriteBank.Create(spriteName));
		sprite.X = base.Collider.Width / 2f;
		sprite.Play("idle");
		Add(shaker = new Shaker(on: false));
		base.Depth = -9000;
		theoGate = spriteName.Equals("TempleGate_theo", StringComparison.InvariantCultureIgnoreCase);
		NearbyCheckFrom = Position + new Vector2(base.Width / 2f, closedHeight / 2);
	}

	public bool get_openState() {
		if (!inverted) return openState;
		else return !openState;
	}

#region methods respecting inverted flag
	public void ToggleOpenState() {
		if (openState) base_Close();
		else base_Open();

	}

	public void Open() {
		if (!inverted) base_Open();
		else base_Close();
	}

	public void Close() {
		if (!inverted) base_Close();
		else base_Open();
	}

	public void StartOpen() {
		if (!inverted) base_StartOpen();
		else base_StartClosed();
	}

	public void StartClosed() {
		if (!inverted) base_StartClosed();
		else base_StartOpen();
	}

	public void LongOpen() {
		if (!inverted) base_LongOpen();
		else base_LongClose();
	}

	public void LongClose() {
		if (!inverted) base_LongClose();
		else base_LongOpen();
	}

#endregion

#region base methods (not respecting inverted flag)
	public void base_Open() {
		Audio.Play(theoGate ? "event:/game/05_mirror_temple/gate_theo_open" : "event:/game/05_mirror_temple/gate_main_open", Position);
		drawHeightMoveSpeed = 200f;
		drawHeight = base.Height;
		shaker.ShakeFor(0.2f, removeOnFinish: false);
		SetHeight(0);
		sprite.Play("open");
		openState = true;
	}

	public void base_Close() {
		Audio.Play(theoGate ? "event:/game/05_mirror_temple/gate_theo_close" : "event:/game/05_mirror_temple/gate_main_close", Position);
		drawHeightMoveSpeed = 300f;
		drawHeight = Math.Max(4f, base.Height);
		shaker.ShakeFor(0.2f, removeOnFinish: false);
		SetHeight(closedHeight);
		sprite.Play("hit");
		openState = false;
	}

	public void base_StartOpen() {
		SetHeight(0);
		drawHeight = 4f;
		openState = true;
	}

	public void base_StartClosed() {
		SetHeight(closedHeight);
		drawHeight = closedHeight;
		openState = false;
	}
	public void base_LongOpen() {
		sprite.Play("open");
		Alarm.Set(this, 0.2f, [MethodImpl(MethodImplOptions.NoInlining)] () => {
			shaker.ShakeFor(0.2f, removeOnFinish: false);
			Alarm.Set(this, 0.2f, Open);
		});
	}
	public void base_LongClose() {
		sprite.Play("hit");
		Alarm.Set(this, 0.2f, [MethodImpl(MethodImplOptions.NoInlining)] () => {
			shaker.ShakeFor(0.2f, removeOnFinish: false);
			Alarm.Set(this, 0.2f, Close);
		});
	}
#endregion

#region IsNearby methods
	public bool IsNearby<T>(float radius=64f) where T : Entity
	{
		bool TIsNearby = false;
		foreach (T entity in base.Scene.Tracker.GetEntities<T>()) {
			if (entity != null && !TIsNearby)
			{
				TIsNearby = Vector2.Distance(NearbyCheckFrom, entity.Center) < radius;
				if (TIsNearby) break;
			}
		}
		return TIsNearby;
	}
	public bool IsNearby(string Name, float radius=64f)
	{
		//EntityRegistry.GetKnownSidsFromType()
		
		bool EntityIsNearby = false;
		foreach (Entity entity in base.Scene.FindEntitiesWithSid(Name)) {
			if (entity == null) continue;
			if (!EntityIsNearby)
			{
				EntityIsNearby = Vector2.Distance(NearbyCheckFrom, entity.Center) < radius;
				if (EntityIsNearby) break;
			}
		}
		return EntityIsNearby;
	}
#endregion

#region basic entity methods
	public override void Awake(Scene scene)
	{
		base.Awake(scene);
		drawHeight = Math.Max(4f, base.Height);
		StartClosed();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update()
	{
		base.Update();
		float num = Math.Max(4f, base.Height);
		if (drawHeight != num)
		{
			drawHeight = Calc.Approach(drawHeight, num, drawHeightMoveSpeed * Engine.DeltaTime);
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Render()
	{
		Vector2 vector = new Vector2(Math.Sign(shaker.Value.X), 0f);
		Draw.Rect(base.X - 2f, base.Y - 8f, 14f, 10f, Color.Black);
		sprite.DrawSubrect(Vector2.Zero + vector, new Rectangle(0, (int)(sprite.Height - drawHeight), (int)sprite.Width, (int)drawHeight));
	}
#endregion

	public void SetHeight(int height) {
		if ((float)height < base.Collider.Height)
		{
			base.Collider.Height = height;
			return;
		}
		float y = base.Y;
		int num = (int)base.Collider.Height;
		if (base.Collider.Height < 64f)
		{
			base.Y -= 64f - base.Collider.Height;
			base.Collider.Height = 64f;
		}
		MoveVExact(height - num);
		base.Y = y;
		base.Collider.Height = height;
	}

}
