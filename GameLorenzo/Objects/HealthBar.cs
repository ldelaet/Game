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

    //logica achter de health bar
    class HealthBar : ObjectMother
    {
        private Texture2D greenBar;
        private Rectangle rectangleRed;
        private Rectangle rectangleGreen;
        private int width = 5;
        public HealthBar(Vector2 newPosition, int Health)
        {
            position = newPosition;
            width = Health * 8;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            rectangleRed = new Rectangle((int)position.X, (int)position.Y -10,40,7);
            rectangleGreen = new Rectangle((int)position.X, (int)position.Y - 10, width, 7);

            if (width > 0) { 
            spriteBatch.Draw(texture, rectangleRed, Color.White);
            spriteBatch.Draw(greenBar, rectangleGreen, Color.White);
            }
        }

        public override void Load(ContentManager Content)
        {
            greenBar = Content.Load<Texture2D>("HealthGreen");
            texture = Content.Load<Texture2D>("HealthRed");
        }
        public void Update(Vector2 newPosition, int Health) {
            position = newPosition;
            width = Health * 8;
        }
    }
}
