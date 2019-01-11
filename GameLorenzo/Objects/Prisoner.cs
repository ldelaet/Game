using GameLorenzo.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo
{
    class Prisoner: ObjectMother
    {
        public Prisoner(Vector2 Position)
        {
            position = Position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, 80, 80);
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        public override void Load(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("locked");
        }
    }
}
