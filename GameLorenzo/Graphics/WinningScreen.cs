using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo.Engine
{
    //Logica achter het win scherm
    class WinningScreen
    {
            Texture2D texture;
            Map map = new Map();
        public bool HasWon { get; set; } = false;


        public void Load(ContentManager Content)
            {
                texture = Content.Load<Texture2D>("WinningScreen");
            }
            public void Draw(SpriteBatch spriteBatch, int x, int y)
            {
            if (HasWon)
                spriteBatch.Draw(texture, new Rectangle(160, 0, x, y), Color.White);
            }
        }
    }


