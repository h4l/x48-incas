using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BirthdayCake
{
    class HUD
    {
        SpriteSheet player1Ring;
        SpriteSheet player1Background;
        SpriteSheet player1Small;
        SpriteSheet player1Medium;
        SpriteSheet player1Large;
        float opacity = 0.80f;

        int player1Head = 0;
        int player2Head = 0;
        int player3Head = 0;
        int player4Head = 0;

        SpriteSheet player2Ring;
        SpriteSheet player2Background;
        SpriteSheet player2Small;
        SpriteSheet player2Medium;
        SpriteSheet player2Large;

        SpriteSheet player3Ring;
        SpriteSheet player3Background;
        SpriteSheet player3Small;
        SpriteSheet player3Medium;
        SpriteSheet player3Large;

        SpriteSheet player4Ring;
        SpriteSheet player4Background;
        SpriteSheet player4Small;
        SpriteSheet player4Medium;
        SpriteSheet player4Large;

        public HUD()
        {
            player1Ring = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringWhite"), new Vector2(129, 129), 1);
            player1Ring.Tint = Engine.playerOne.PlayerColor;
            player1Ring.Opacity = opacity;

            player1Background = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringBkg"), new Vector2(129, 129), 1);
            player1Background.Opacity = opacity;

            player1Small = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringOne"), new Vector2(129, 129), 1);
            player1Small.Opacity = opacity;

            player1Medium = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringTwo"), new Vector2(129, 129), 1);
            player1Medium.Opacity = opacity;

            player1Large = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringThree"), new Vector2(129, 129), 1);
            player1Large.Opacity = opacity;

            Vector2 player2Position = new Vector2(1151, 895);
            player2Ring = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringWhite"), new Vector2(129, 129), 1);
            player2Ring.Tint = Engine.playerTwo.PlayerColor;
            player2Ring.Position = player2Position;
            player2Ring.Opacity = opacity;

            player2Background = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringBkg"), new Vector2(129, 129), 1);
            player2Background.Opacity = opacity;
            player2Background.Position = player2Position;

            player2Small = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringOne"), new Vector2(129, 129), 1);
            player2Small.Opacity = opacity;
            player2Small.Position = player2Position;

            player2Medium = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringTwo"), new Vector2(129, 129), 1);
            player2Medium.Opacity = opacity;
            player2Medium.Position = player2Position;

            player2Large = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringThree"), new Vector2(129, 129), 1);
            player2Large.Opacity = opacity;
            player2Large.Position = player2Position;

            Vector2 player3Position = new Vector2(0, 895);
            player3Ring = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringWhite"), new Vector2(129, 129), 1);
            player3Ring.Tint = Engine.playerThree.PlayerColor;
            player3Ring.Position = player3Position;
            player3Ring.Opacity = opacity;

            player3Background = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringBkg"), new Vector2(129, 129), 1);
            player3Background.Opacity = opacity;
            player3Background.Position = player3Position;

            player3Small = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringOne"), new Vector2(129, 129), 1);
            player3Small.Opacity = opacity;
            player3Small.Position = player3Position;

            player3Medium = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringTwo"), new Vector2(129, 129), 1);
            player3Medium.Opacity = opacity;
            player3Medium.Position = player3Position;

            player3Large = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringThree"), new Vector2(129, 129), 1);
            player3Large.Opacity = opacity;
            player3Large.Position = player3Position;

            Vector2 player4Position = new Vector2(1151, 0);
            player4Ring = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringWhite"), new Vector2(129, 129), 1);
            player4Ring.Tint = Engine.playerFour.PlayerColor;
            player4Ring.Position = player4Position;
            player4Ring.Opacity = opacity;

            player4Background = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringBkg"), new Vector2(129, 129), 1);
            player4Background.Opacity = opacity;
            player4Background.Position = player4Position;

            player4Small = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringOne"), new Vector2(129, 129), 1);
            player4Small.Opacity = opacity;
            player4Small.Position = player4Position;

            player4Medium = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringTwo"), new Vector2(129, 129), 1);
            player4Medium.Opacity = opacity;
            player4Medium.Position = player4Position;

            player4Large = new SpriteSheet(Engine.ContentManager.Load<Texture2D>(@"ringThree"), new Vector2(129, 129), 1);
            player4Large.Opacity = opacity;
            player4Large.Position = player4Position;

        }
        public void Update(GameTime t)
        {
            player1Ring.Update(t);
            player1Background.Update(t);
            player1Small.Update(t);
            player1Medium.Update(t);
            player1Large.Update(t);
            if (Engine.temples[PlayerIndex.One].Prayer <= Engine.temples[PlayerIndex.One].MaxPrayer / 3)
            {
                player1Head = 0;
            }
            if (Engine.temples[PlayerIndex.One].Prayer >= Engine.temples[PlayerIndex.One].MaxPrayer / 3 && Engine.temples[PlayerIndex.One].Prayer <= (Engine.temples[PlayerIndex.One].MaxPrayer / 3) * 2)
            {
                player1Head = 1;
            }
            if (Engine.temples[PlayerIndex.One].Prayer >= (Engine.temples[PlayerIndex.One].MaxPrayer / 3) * 2 && Engine.temples[PlayerIndex.One].Prayer <= (Engine.temples[PlayerIndex.One].MaxPrayer))
            {
                player1Head = 2;
            }

            player2Ring.Update(t);
            player2Background.Update(t);
            player2Small.Update(t);
            player2Medium.Update(t);
            player2Large.Update(t);
            if (Engine.temples[PlayerIndex.Two].Prayer <= Engine.temples[PlayerIndex.Two].MaxPrayer / 3)
            {
                player2Head = 0;
            }
            if (Engine.temples[PlayerIndex.Two].Prayer >= Engine.temples[PlayerIndex.Two].MaxPrayer / 3 && Engine.temples[PlayerIndex.Two].Prayer <= (Engine.temples[PlayerIndex.Two].MaxPrayer / 3) * 2)
            {
                player2Head = 1;
            }
            if (Engine.temples[PlayerIndex.Two].Prayer >= (Engine.temples[PlayerIndex.Two].MaxPrayer / 3) * 2 && Engine.temples[PlayerIndex.Two].Prayer <= (Engine.temples[PlayerIndex.Two].MaxPrayer))
            {
                player2Head = 2;
            }

            player3Ring.Update(t);
            player3Background.Update(t);
            player3Small.Update(t);
            player3Medium.Update(t);
            player3Large.Update(t);
            if (Engine.temple3.Prayer <= Engine.temple3.MaxPrayer / 3)
            {
                player3Head = 0;
            }
            if (Engine.temple3.Prayer >= Engine.temple3.MaxPrayer / 3 && Engine.temple3.Prayer <= (Engine.temple3.MaxPrayer / 3) * 2)
            {
                player3Head = 1;
            }
            if (Engine.temple3.Prayer >= (Engine.temple3.MaxPrayer / 3) * 2 && Engine.temple3.Prayer <= (Engine.temple3.MaxPrayer))
            {
                player3Head = 2;
            }

            player4Ring.Update(t);
            player4Background.Update(t);
            player4Small.Update(t);
            player4Medium.Update(t);
            player4Large.Update(t);
            if (Engine.temple4.Prayer <= Engine.temple4.MaxPrayer / 3)
            {
                player4Head = 0;
            }
            if (Engine.temple4.Prayer >= Engine.temple4.MaxPrayer / 3 && Engine.temple4.Prayer <= (Engine.temple4.MaxPrayer / 3) * 2)
            {
                player4Head = 1;
            }
            if (Engine.temple4.Prayer >= (Engine.temple4.MaxPrayer / 3) * 2 && Engine.temple4.Prayer <= (Engine.temple4.MaxPrayer))
            {
                player4Head = 2;
            }
            

        }
        public void Draw()
        {
           
            player1Background.Draw();
            
            if(player1Head == 0)
            {
                player1Small.Draw();
            }
            if(player1Head == 1)
            {
                player1Medium.Draw();
            }
            if(player1Head == 2)
            {
                player1Large.Draw();
            }
            player1Ring.Draw();

            player2Background.Draw();
            if (player2Head == 0)
            {
                player2Small.Draw();
            }
            if (player2Head == 1)
            {
                player2Medium.Draw();
            }
            if (player2Head == 2)
            {
                player2Large.Draw();
            }
            player2Ring.Draw();

            player3Background.Draw();
            if (player3Head == 0)
            {
                player3Small.Draw();
            }
            if (player3Head == 1)
            {
                player3Medium.Draw();
            }
            if (player3Head == 2)
            {
                player3Large.Draw();
            }
            player3Ring.Draw();

            player4Background.Draw();
            if (player4Head == 0)
            {
                player4Small.Draw();
            }
            if (player4Head == 1)
            {
                player4Medium.Draw();
            }
            if (player4Head == 2)
            {
                player4Large.Draw();
            }
            player4Ring.Draw();
        }
    }
}
