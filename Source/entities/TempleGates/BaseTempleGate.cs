using Celeste.Mod.Entities;
using Monocle;
using Microsoft.Xna.Framework;

using System;
using System.Runtime.CompilerServices;

namespace Celeste.Mod.ClckHelper.Entities;

public class BaseTempleGate : Solid {
	private string open_sound;
	private string close_sound;
	public int closedHeight;

	public enum Direction {DOWN, LEFT, UP, RIGHT}
	public Direction direction;
	private Sprite sprite;

	private Shaker shaker;

	public float drawHeight;

	private float drawHeightMoveSpeed;
	public bool inverted = false;
	public bool openState;
	public BaseTempleGate(EntityData data, Vector2 offset) : base(data.Position + offset, 8f, 1, safe: true)
	{
		string spriteName = data.String("sprite", "TempleGate_default");
		closedHeight = data.Int("height", 40);
		inverted = data.Bool("inverted", false);
		open_sound = data.String("open_sound", "event:/game/05_mirror_temple/gate_main_open");
		close_sound = data.String("close_sound", "event:/game/05_mirror_temple/gate_main_close");
		direction = Direction.UP;
		

		Add(sprite = GFX.SpriteBank.Create(spriteName));

		sprite.Rotation = (float)Math.PI/2 * (float)direction;

		sprite.X = base.Collider.Width / 2f;
		sprite.Play("idle");
		Add(shaker = new Shaker(on: false));
		base.Depth = -9000;
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
		Audio.Play(open_sound, Position);
		drawHeightMoveSpeed = 200f;
		drawHeight = base.Height;
		shaker.ShakeFor(0.2f, removeOnFinish: false);
		SetHeight(0);
		sprite.Play("open");
		openState = true;
	}

	public void base_Close() {
		Audio.Play(close_sound, Position);
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
		Alarm.Set(this, 0.2f, () => {
			shaker.ShakeFor(0.2f, removeOnFinish: false);
			Alarm.Set(this, 0.2f, Open);
		});
	}
	public void base_LongClose() {
		sprite.Play("hit");
		Alarm.Set(this, 0.2f, () => {
			shaker.ShakeFor(0.2f, removeOnFinish: false);
			Alarm.Set(this, 0.2f, Close);
		});
	}
#endregion

#region basic entity methods
	public override void Awake(Scene scene)
	{
		base.Awake(scene);
		drawHeight = Math.Max(4f, base.Height);
		StartClosed();
	}
	public override void Update()
	{
		base.Update();
		Console.WriteLine($"Top is {base.Collider.Top}, Bottom is {base.Collider.Bottom}, AbsouluteBottom is {base.Collider.AbsoluteBottom}, Height is {base.Collider.Height}");
		float num = Math.Max(4f, base.Height);
		if (drawHeight != num)
		{
			drawHeight = Calc.Approach(drawHeight, num, drawHeightMoveSpeed * Engine.DeltaTime);
		}
	}

	public override void Render()
	{
		Vector2 vector = new Vector2(Math.Sign(shaker.Value.X), 0f);
		Draw.Rect(base.X - 2f, base.Y - 8f, 14f, 10f, Color.Black);
		sprite.DrawSubrect(Vector2.Zero + vector, new Rectangle(0, (int)(sprite.Height - drawHeight), (int)sprite.Width, (int)drawHeight));
	}
#endregion

	public void SetHeightDOWN(int height) {
		if ((float)height < base.Collider.Height)
		{
			base.Collider.Height = height;
			base.Collider.Top = 0;
			return;
		}
		float y = base.Y;
		int num = (int)base.Collider.Height;
		if (base.Collider.Height < 64f)
		{
			base.Y -= 64f - base.Collider.Height;
			base.Collider.Height = 64f;
			base.Collider.Top = 0;
		}
		MoveVExact(height - num);
		base.Y = y;
		base.Collider.Height = height;
		base.Collider.Top = 0;
	}

	public void SetHeightUP(int height) {
		if ((float)height < base.Collider.Height)
		{
			base.Collider.Height = height;
			base.Collider.Bottom = 0;
			return;
		}
		float y = base.Y;
		int num = (int)base.Collider.Height;
		if (base.Collider.Height < 64f)
		{
			base.Y += 64f - base.Collider.Height;
			base.Collider.Height = 64f;
			base.Collider.Bottom = 0;
		}
		MoveVExact(num - height);
		base.Y = y;
		base.Collider.Height = height;
		base.Collider.Bottom = 0;


	}
	public void SetHeight(int height) {
		switch (direction) {
			case Direction.DOWN:
				SetHeightDOWN(height);
				break;
			case Direction.UP:
				SetHeightUP(height);
				break;
		}



	}

}
