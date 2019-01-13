using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo.Engine
{
    //De knop van het main menu, klik logica staat in de game1 klasse. Hier gaat het vooral om de intersect. Ik heb een soort "hover" gecreeerd die de knop lichter maakt als de muis erover zweeft
    class Button
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        Rectangle mouseRect;
        public bool isClicked { get; set; }

        Color color = new Color(255, 255, 255, 255);
        public Vector2 size;

        public Button(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;
            size = new Vector2(500, 100);
        }
        public void Update(MouseState mouse) {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            mouseRect = new Rectangle(mouse.X, mouse.Y, 2, 2);
            if (mouseRect.Intersects(rectangle))
            {
                color.A = 200;
                if (mouse.LeftButton == ButtonState.Pressed) isClicked = true;
            }
            else
            {
                color.A = 255;
                isClicked = false;
            }
        }
        public void Positioning(Vector2 Position)
        {
            position = Position;
        }
        public void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(texture, rectangle, color);
        }
    }
}
