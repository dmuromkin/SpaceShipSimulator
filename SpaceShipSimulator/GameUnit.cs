using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShipSimulator
{
    public class GameUnit
    {
        private Texture2D _tex;
        private Vector2 _pos;
        protected Color _color;

        public Texture2D Texture
        {
            get { return this._tex; }
        }
        public Vector2 Position
        {
            get { return _pos; }
        }
        public void SetPosition(Vector2 pos)
        {
            _pos = pos;
        }

        public void SetPosition(int x, int y)
        {
            _pos = new Vector2(x, y);
        }

        public void Move(Vector2 delta)
        {
            _pos += delta;
        }

        public GameUnit(Texture2D tex)
        {
            _tex = tex;
            _color = Color.White;
            _pos = new Vector2(0, 0);
         
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(_tex,
                _pos,
                null,
                _color
                );
        }
        public void Update(GameTime gameTime, Vector2 speed)
        {
            _pos += speed;
        }
    }
}
