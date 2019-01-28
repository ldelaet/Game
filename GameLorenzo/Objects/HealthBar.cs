using GameLorenzo.Objects;
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
    class HealthBar : ObjectMother
    {
        private Texture2D greenBar;
        private Rectangle rectangleRed;
        private Rectangle rectangleGreen;
        private int width;
        public HealthBar()
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            rectangleRed = new Rectangle((int)position.X, (int)position.Y,width,20);
            rectangleGreen = new Rectangle((int)position.X, (int)position.Y, 200, 20);
            spriteBatch.Draw(texture, rectangleRed, Color.White);
            spriteBatch.Draw(greenBar, rectangleGreen, Color.White);
        }

        public override void Load(ContentManager Content)
        {
            greenBar = Content.Load<Texture2D>("HealthGreen");
            texture = Content.Load<Texture2D>("HealthRed");
        }
        public void Update(Vector2 newPosition, int Health) {
            position = newPosition;
            width = Health * 40;

        }
    }
}
