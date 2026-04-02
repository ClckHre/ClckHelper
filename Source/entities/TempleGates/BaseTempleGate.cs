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
		direction = Enum.Parse<Direction>(data.String("direction", "DOWN"));

		Add(sprite = GFX.SpriteBank.Create(spriteName));
		sprite.Rotation = (float)Math.PI/2 * (float)direction;

		if (direction == Direction.DOWN || direction == Direction.UP) {
			base.Collider.Width = 8f;
			sprite.X = base.Collider.Width / 2f;
		}
		else {
			base.Collider.Height = 8f;
			sprite.Y = base.Collider.Height / 2f;
		}


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
		//drawHeight = base.Height;
		shaker.ShakeFor(0.2f, removeOnFinish: false);
		SetHeight(0);
		sprite.Play("open");
		openState = true;
	}

	public void base_Close() {
		Audio.Play(close_sound, Position);
		drawHeightMoveSpeed = 300f;
		//drawHeight = Math.Max(4f, base.Height);
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
		if (direction == Direction.DOWN || direction == Direction.UP) drawHeight = Math.Max(4f, base.Height);
		else drawHeight = Math.Max(4f, base.Width);
		StartClosed();
	}
	public override void Update()
	{
		base.Update();
		Console.WriteLine($"Top is {base.Collider.Top}, Bottom is {base.Collider.Bottom}, AbsouluteBottom is {base.Collider.AbsoluteBottom}, Height is {base.Collider.Height}");
		float num;
		if (direction == Direction.DOWN || direction == Direction.UP) num = Math.Max(4f, base.Height);
		else num = Math.Max(4f, base.Width);
		if (drawHeight != num)
		{
			drawHeight = Calc.Approach(drawHeight, num, drawHeightMoveSpeed * Engine.DeltaTime);
		}
	}

	public override void Render()
	{
		if (direction == Direction.DOWN || direction == Direction.UP) {
			Vector2 vector = new Vector2(Math.Sign(shaker.Value.X), 0f);
			Draw.Rect(base.X - 2f, base.Y - 8f, 14f, 10f, Color.Black);
			sprite.DrawSubrect(Vector2.Zero + vector, new Rectangle(0, (int)(sprite.Height - drawHeight), (int)sprite.Width, (int)drawHeight));
		}
		else {
			Vector2 vector = new Vector2(Math.Sign(shaker.Value.X), 0f);
			Draw.Rect(base.X - 2f, base.Y - 8f, 14f, 10f, Color.Black);
			sprite.DrawSubrect(Vector2.Zero + vector, new Rectangle(0, (int)(sprite.Height - drawHeight), (int)sprite.Width, (int)drawHeight));
		}
	}
#endregion

	private void SetHeightDOWN(int height) {
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

	private void SetHeightUP(int height) {
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
	private void SetHeightRIGHT(int height) {
		if ((float)height < base.Collider.Width)
		{
			base.Collider.Width = height;
			base.Collider.Left = 0;
			return;
		}
		float x = base.X;
		int num = (int)base.Collider.Width;
		if (base.Collider.Width < 64f)
		{
			base.X -= 64f - base.Collider.Width;
			base.Collider.Width = 64f;
			base.Collider.Left = 0;
		}
		MoveHExact(height - num);
		base.X = x;
		base.Collider.Width = height;
		base.Collider.Left = 0;
	}
	private void SetHeightLEFT(int height) {
		if ((float)height < base.Collider.Width)
		{
			base.Collider.Width = height;
			base.Collider.Right = 0;
			return;
		}
		float x = base.X;
		int num = (int)base.Collider.Width;
		if (base.Collider.Width < 64f)
		{
			base.X -= 64f - base.Collider.Width;
			base.Collider.Width = 64f;
			base.Collider.Right = 0;
		}
		MoveHExact(num - height);
		base.X = x;
		base.Collider.Width = height;
		base.Collider.Right = 0;
	}
	public void SetHeight(int height) {
		switch (direction) {
			case Direction.DOWN:
				SetHeightDOWN(height);
				break;
			case Direction.UP:
				SetHeightUP(height);
				break;
			case Direction.RIGHT:
				SetHeightRIGHT(height);
				break;
			case Direction.LEFT:
				SetHeightLEFT(height);
				break;
		}



	}

}
