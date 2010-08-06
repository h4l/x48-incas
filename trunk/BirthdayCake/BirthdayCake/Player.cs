using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace BirthdayCake
{
public class Player : DrawableGameComponent
{
    float PLAYER_SPEED = 200.0f; //Pixels per second

    const float SPEED_MODIFIER = 2.0f;
    const float RADIUS_MODIFIER = 2.0f;

    float favour;
    public float Favour
    {
        get { return favour; }
        set 
        { 
            favour = value;
        }
    }

    static Texture2D playerCircle;
    float circlerot;
    Vector2 circlescale;
    Vector2 circleorigin;
    Texture2D favorRadius;
    Vector2 favorRadiusOrigin;
    SpriteSheet bigHead1;
    SpriteSheet bigHead2;

    PlayerIndex player;

    SpriteSheet tex;

	private Engine game;

	public List<Peon> attachedPeons;

	private Vector2 velocity;

	public Vector2 position;

    float radius = 92;
	private float angle;

	private Vehicle vehicle;

    bool hittest;
    BoundingCircle boundingCircle;
    public BoundingCircle BoundingCircle
    {
        get { return boundingCircle; }
        set { boundingCircle = value; }
    }

    Rectangle bounds;

    Color playerColor;
    public Color PlayerColor
    {
        get { return Engine.PlayerColours[player]; }
    }

    TimeSpan speedTimer;
    bool speedBoost;

    TimeSpan radiusTimer;
    bool radiusBoost;

	public Player(PlayerIndex player, Engine game)
		: base(game) 
	{
		this.game = game;
        

        this.player = player;

        Engine.InputManager.LeftStickMove += new Input.ControllerStickEvent(InputManager_LeftStickMove);
        tex = new SpriteSheet(Engine.ContentManager.Load<Texture2D>("demigod"), new Vector2(30, 31), 3);
        tex.Origin = new Vector2(15, 15);

        bigHead1 = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"mainCharHigh"),new Vector2(64, 89), 1);
        bigHead1.Origin = new Vector2(32f, 44.5f);
        //bighead1.Scale = new Vector2(0.25f, 0.45f);

        bigHead2 = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"mainCharHigh"), new Vector2(64, 89), 1);
        bigHead2.Origin = new Vector2(32f, 44.5f);
        


        bounds = new Rectangle();
		attachedPeons = new List<Peon>();
		vehicle = new Vehicle();

        playerCircle = Engine.ContentManager.Load<Texture2D>("playercircle");
        circleorigin = new Vector2(playerCircle.Width / 2, playerCircle.Height / 2);
        circlescale = Vector2.One;

        favorRadius = Engine.ContentManager.Load<Texture2D>("favorradius");
        favorRadiusOrigin = new Vector2(favorRadius.Width / 2, favorRadius.Height / 2);
        boundingCircle = new BoundingCircle(position, radius);
    }

	public Vector2 Position
	{
		get
		{
			return vehicle.Position;
		}
		set
		{
			vehicle.Position = value;
		}
	}

	public Vehicle Vehicle
	{
		get
		{
			return vehicle;
		}
	}

    public void grantSpeedBoost()
    {
        if (!speedBoost)
        {
            speedBoost = true;
            speedTimer = TimeSpan.Zero;
        }
    }

    public void grantRadiusBoost()
    {
        if (!radiusBoost)
        {
            radiusBoost = true;
            radiusTimer = TimeSpan.Zero;
            boundingCircle.Radius *= RADIUS_MODIFIER;
        }
    }

    void InputManager_LeftStickMove(PlayerIndex player, Vector2 v, GameTime t)
    {
        if (this.player == player)
        {
            Vector2 moveAmount = (v * (float)t.ElapsedGameTime.TotalSeconds) * PLAYER_SPEED;
            if (speedBoost)
            {
                moveAmount *= SPEED_MODIFIER;
            }
            angle = (float)Math.Atan2(v.Y, v.X);
            if (!Engine.terrain.CheckCollision((int)(position.X + moveAmount.X), (int)(position.Y + moveAmount.Y)))
            {
                position += moveAmount;
                boundingCircle.Position = position;
                vehicle.Position = position;
                angle = (float)Math.Atan2(v.Y, v.X) - MathHelper.PiOver2;
            }
        }
    }

	/// <summary>
	/// Adds a peon to this player so it starts being followed.
	/// Handles removing the peon from other players so things can't fuck up.
	/// </summary>
	/// <param name="peon"></param>
	public void attachPeon(Peon peon)
	{
		foreach (Player p in Engine.players.Values)
			p.deattachPeon(peon);

		// add the peon to the back of the queue
		attachedPeons.Add(peon);
	}

    public void deattachPeon(Peon peon)
    {
        attachedPeons.Remove(peon);
    }

    public override void Update(GameTime gt)
	{
		float delta = gt.ElapsedGameTime.Milliseconds / 1000f;

        circlerot += delta*3;

        //needs changing to scale with head
        if (Engine.temples[PlayerIndex.One].Prayer > (Engine.temples[PlayerIndex.One].MaxPrayer / 3) * 2)//&& Engine.temple1.Prayer < Engine.temple1.MaxPrayer)
        if (speedBoost)
        {
            speedTimer += gt.ElapsedGameTime;
        }
        if (speedTimer > TimeSpan.FromMilliseconds(5000))
        {
            speedBoost = false;
        }

        if (radiusBoost)
        {
            radiusTimer += gt.ElapsedGameTime;
        }
        if (radiusTimer > TimeSpan.FromMilliseconds(5000))
        {
            radiusBoost = false;
            radiusTimer = TimeSpan.Zero;
            boundingCircle.Radius /= RADIUS_MODIFIER;
        }

        circlerot += delta;

        circlescale = Vector2.One * 1.5f;//Vector2.One + ((Vector2.One / 2) * (((float)Math.Sin(gt.TotalGameTime.TotalMilliseconds / 300) + 1) / 2));

        if (radiusBoost) { circlescale *= RADIUS_MODIFIER; }
       
        bigHead1.Update(gt);
        bigHead1.Rotation = angle;
        bigHead1.Position = position;

        bigHead2.Update(gt);
        bigHead2.Rotation = angle;
        bigHead2.Position = position;

        bounds = new Rectangle((int)position.X, (int)position.Y, 30, 30);

        tex.Update(gt);
        tex.Rotation = angle;
        tex.Position = position;
        
		//position = velocity * delta;
        
	}

    public void Draw(GameTime t)
    {
        Engine.spriteBatch.End();
        Engine.spriteBatch.Begin(SpriteBlendMode.Additive);
        //Engine.spriteBatch.Draw(favorRadius, position - favorRadiusOrigin, new Color(Engine.PlayerColours[player], (float)(Math.Abs(Math.Sin(circlerot / 2)) * 0.6f) + 0.4f));
        Engine.spriteBatch.Draw(favorRadius, position - favorRadiusOrigin - (radiusBoost ? favorRadiusOrigin : Vector2.Zero), null, new Color(Engine.PlayerColours[player], (float)(Math.Abs(Math.Sin(circlerot / 2)) * 0.6f) + 0.4f), 0, Vector2.Zero, radiusBoost ? Vector2.One * RADIUS_MODIFIER : Vector2.One, SpriteEffects.None, 0);
        Engine.spriteBatch.End();
        Engine.spriteBatch.Begin();
        Engine.spriteBatch.Draw(playerCircle, position, null, new Color(Engine.PlayerColours[player], 100), circlerot, circleorigin, circlescale, SpriteEffects.None, 0);
        tex.Draw();

        float temp = Engine.temples[player].Prayer / Engine.temples[player].MaxPrayer;
        if (temp >= 0.35)
        {
            bigHead1.Draw();
        }
        if (bigHead1.Scale.X <= 1.0f)
        {
            bigHead1.Scale = new Vector2(temp, temp);
        }
        else
        {
            bigHead1.Scale = new Vector2(1.0f, 1.0f);
        }
        //PLAYER_SPEED = temp / PLAYER_SPEED * 100;
        if (PLAYER_SPEED < 75)
        {
            PLAYER_SPEED = 75;
        }
        PLAYER_SPEED = (PLAYER_SPEED) - (temp / 2);


        //if (Engine.temples[player].Prayer > Engine.temples[player].MaxPrayer / 3 && Engine.temples[player].Prayer < (Engine.temples[player].MaxPrayer / 3) * 2)
        //{
        //    float temp = Engine.temples[PlayerIndex.One].Prayer / Engine.temples[PlayerIndex.One].MaxPrayer;

        //    if (temp >= 0.35)
        //    {
        //        bigHead1.Draw();
        //    }
        //    if (bigHead1.Scale.X <= 1.0f)
        //    {
        //        bigHead1.Scale = new Vector2(temp, temp);
        //    }
        //    else
        //    {
        //        bigHead1.Scale = new Vector2(1.0f, 1.0f);
        //    }
        //    //PLAYER_SPEED = temp / PLAYER_SPEED * 100;
        //    if (PLAYER_SPEED < 75)
        //    {
        //        PLAYER_SPEED = 75;
        //    }
            
        //    PLAYER_SPEED = (PLAYER_SPEED) - (temp / 2);
        //    //if (PLAYER_SPEED < 100)
        //    //{
        //    //    PLAYER_SPEED = 200;
        //    //}
        //}
        //if (Engine.temples[player].Prayer > (Engine.temples[player].MaxPrayer / 3) * 2) //&& Engine.temple1.Prayer < Engine.temple1.MaxPrayer)
        //{
        //    float temp = Engine.temple2.Prayer / Engine.temple2.MaxPrayer;

        //    if (temp >= 0.35)
        //    {
        //        bigHead2.Draw();
        //    }
        //    if (bigHead2.Scale.X <= 0.95f)
        //    {
        //        bigHead2.Scale = new Vector2(temp, temp);
        //    }
        //    else
        //    {
        //        bigHead2.Scale = new Vector2(0.95f, 0.95f);
        //    }
        //    if (PLAYER_SPEED < 75)
        //    {
        //        PLAYER_SPEED = 75;
        //    }

        //    PLAYER_SPEED = (PLAYER_SPEED) - (temp / 2);
        //}

        //if (this == Engine.playerOne)
        //{
        //    if (Engine.temple1.Prayer > Engine.temple1.MaxPrayer / 3 && Engine.temple1.Prayer < (Engine.temple1.MaxPrayer / 3) * 2)
        //    {
        //        bighead1.Draw();
        //        PLAYER_SPEED = 150;
        //    }
        //    if (Engine.temple1.Prayer > (Engine.temple1.MaxPrayer / 3) * 2) //&& Engine.temple1.Prayer < Engine.temple1.MaxPrayer)
        //    {
        //        bighead1.Scale = new Vector2(2, 2);
        //        bighead1.Draw();
        //        PLAYER_SPEED = 100;

        //        //bighead2.Draw();
        //    }
        //}
        //if (this == Engine.playerTwo)
        //{
        //    if (Engine.temple2.Prayer > Engine.temple2.MaxPrayer / 3 && Engine.temple2.Prayer < (Engine.temple2.MaxPrayer / 3) * 2)
        //    {
        //        bighead1.Draw();
        //        PLAYER_SPEED = 150;
        //    }
        //    if (Engine.temple2.Prayer > (Engine.temple2.MaxPrayer / 3) * 2) //&& Engine.temple2.Prayer < Engine.temple2.MaxPrayer)
        //    {
        //        bighead1.Scale = new Vector2(2, 2);
        //        bighead1.Draw();
        //        PLAYER_SPEED = 100;

        //        //bighead2.Draw();
        //    }
        //}
    }   

	/// <summary>
	/// Gets the position the specified peon should seek to.
	/// </summary>
	/// <param name="peon"></param>
	/// <returns></returns>
	public Vehicle getFollowTarget(Peon peon)
	{
		int i;
		for (i = 0; i < attachedPeons.Count; ++i)
		{
			if(peon.Equals(attachedPeons[i]))
				break;
		}
		if(i == attachedPeons.Count)
			throw new Exception("Tried to get position of peon who was not attached to player.");

		if(i == 0)
			return vehicle;
		else
			return attachedPeons[i - 1].Vehicle;
	}

	/// <summary>
	/// Called by the button combo class when the player has made a combo thing
	/// </summary>
	public void onComboButtonPressed()
	{
		// check if there are any peons nearby and notify them
		foreach (Peon p in Engine.manager.Peons)
		{
			if(boundingCircle.intersects(p.BoundingCircle))
			{
				p.onNearbyCombo(this);
			}
		}
	}
}
}
