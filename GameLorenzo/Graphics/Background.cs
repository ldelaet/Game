using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo.Characters
{
    //Achtergrond logica
    class Background
    {
        Texture2D texture;
        Map map = new Map();


        public void Load(ContentManager Content) {
            texture = Content.Load<Texture2D>("background1");
        }
        public void Draw(SpriteBatch spriteBatch, int x, int y) {
            
            spriteBatch.Draw(texture, new Rectangle(0, 0, x, y), Color.White);
        }
    }
}
