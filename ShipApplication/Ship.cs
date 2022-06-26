using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShipApplication
{
    public class Ship : DrawableGameComponent
    {
        private SpriteBatch spriteBatch;
        private Texture2D tex;
        private Vector2 position;
        private Vector2 speed;
        private float rotation = 0;
        private Rectangle srcRect;
        private Vector2 origin;
        private Vector2 stage;

        private float scale = 1.0f;
        private float scaleChange = 0.05f;
        private const float MAX_SCALE = 3.0F;
        private const float MIN_SCALE = 0.05f;

        private float oldValue;


        public Ship(Game game,
            SpriteBatch spriteBatch,
            Texture2D tex,
            Vector2 position,
            Vector2 speed,
            Vector2 stage): base(game)
        {
            this.spriteBatch = spriteBatch;
            this.tex = tex;
            this.position = position;
            this.speed = speed;
            this.stage = stage;

            this.srcRect = new Rectangle(0, 0, tex.Width, tex.Height);
            this.origin = new Vector2(tex.Width / 2, tex.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Pressed)
            {
                Vector2 target = new Vector2(ms.X, ms.Y);

                //keep the ship within the screen
                if (target.X > stage.X - scale * tex.Width/2 )
                {
                    target.X = stage.X - scale * tex.Width/2;
                }
                if (target.X < scale * tex.Width/2)
                {
                    target.X = scale * tex.Width / 2;
                }
                if (target.Y > stage.Y - scale * tex.Height /2)
                {
                    target.Y = stage.Y - scale * tex.Height / 2;
                }

                if (target.Y < scale * tex.Height /2)
                {
                    target.Y = scale * tex.Height / 2;
                }




                //translation
                float xDiff = target.X - position.X;
                float yDiff = target.Y - position.Y;
                position.X += xDiff * speed.X * 0.05f ;
                position.Y += yDiff * speed.Y * 0.05f;

                //rotation
                float deviation = 0f;

                if (yDiff < 0 && xDiff > 0)
                {
                    deviation = (float)Math.PI / 2;
                }
                else if (yDiff > 0 && xDiff >0)
                {
                    deviation = (float)Math.PI / 2;
                }
                else if (yDiff > 0 && xDiff <0)
                {
                    deviation = -(float)Math.PI / 2;
                }
                else if (yDiff < 0 && xDiff < 0)
                {
                    deviation = -(float)Math.PI / 2;
                }

                rotation = deviation + (float)Math.Atan(yDiff / xDiff);
                // position = target;
            }

            //scaling
            float currValue = ms.ScrollWheelValue;
            if (currValue != oldValue)
            {
                float scaleValue = (currValue - oldValue) / 120;
                scale += scaleValue * scaleChange;
                if (scale > MAX_SCALE)
                {
                    scale = MAX_SCALE;
                }
                else if (scale < MIN_SCALE)
                {
                    scale = MIN_SCALE;
                }
                oldValue = currValue;
            }







            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tex, position, srcRect, Color.White,
                rotation, origin, scale, SpriteEffects.None, 0f);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
