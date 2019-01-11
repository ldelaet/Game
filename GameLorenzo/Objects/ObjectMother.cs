using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo.Objects
{
    public abstract class ObjectMother
    {
        
        public Vector2 position { get; set; }
        public Rectangle rectangle { get; set; } 


        public Texture2D texture { get; set; }

        

        public abstract void Load(ContentManager Content);

        public abstract void Draw(SpriteBatch spriteBatch);
        
    }
}
