using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLorenzo
{

    public abstract class Bediening
    {
        private bool idleRight = true;
        public bool Right { get; set; }
        public bool Left { get; set; }
        public bool IdleRight
        {
            get { return idleRight; }
            set { idleRight = value; }
        }

        public bool IdleLeft { get; set; }
        public bool Jump { get; set; }
        public bool Shoot { get; set; }
        public bool Sprint { get; set; }
        //protected KeyboardState CurrentKey { get; set; }
        public KeyboardState PrevKey { get; set; }


        public abstract void Update();
    }

    public class BedieningPijltjes : Bediening
    {
        public override void Update()
        {
            KeyboardState stateKey = Keyboard.GetState();
            var mouseState = Mouse.GetState();


            if (stateKey.IsKeyDown(Keys.Q))
            {
                Left = true;
                IdleRight = false;
            }
            if (stateKey.IsKeyUp(Keys.Q))
            {
                Left = false;
            }

            if (stateKey.IsKeyDown(Keys.D))
            {
                Right = true;
                IdleLeft = false;
            }
            if (stateKey.IsKeyUp(Keys.D))
            {
                Right = false;
            }

            if (stateKey.IsKeyUp(Keys.Q) && PrevKey.IsKeyDown(Keys.Q)) IdleLeft = true; 
            if (stateKey.IsKeyUp(Keys.D) && PrevKey.IsKeyDown(Keys.D)) IdleRight = true;

            if (stateKey.IsKeyDown(Keys.Space))
            {
                Jump = true;
            }
            if (stateKey.IsKeyUp(Keys.Space))
            {
                Jump = false;
            }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Shoot = true;
            }
            if (mouseState.LeftButton == ButtonState.Released)
            {
                Shoot = false;
            }
            if (stateKey.IsKeyDown(Keys.LeftShift))
            {
                Sprint = true;
            }
            if (stateKey.IsKeyUp(Keys.LeftShift))
            {
                Sprint = false;
            }
            PrevKey = stateKey;
        }
    }
}

