using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo.Engine
{
    //beheerd animaties op basis van de breedte van de sprite sheet en schuift telkens op tot het einde van de sheet bereikt is.
    //sprite sheets worden toegvoegd via de dictionary in de game1 klasse
    class AnimationManager
    {
        private Animation _animation;
        Player player = new Player();

        private float _timer;

        public Vector2 Position { get; set; }

        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(_animation.Texture, position
                             ,
                             new Rectangle(_animation.CurrentFrame * _animation.FrameWidth,
                                           0,
                                           _animation.FrameWidth,
                                           _animation.FrameHeight),
                             Color.White);
        }

        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;
            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
        }

        public void Stop()
        {
            _timer = 0;
            _animation.CurrentFrame = 0;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0;
                _animation.CurrentFrame++;
                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }
    }
}
