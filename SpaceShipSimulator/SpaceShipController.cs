using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShipSimulator
{
    public class SpaceshipController : DrawableGameComponent
    {
        protected GameUnit _subject;
        protected Cannon _cannon;
        protected KeyboardState _previousKs;

        public void Attach(GameUnit subject)
        {
            _subject = subject;
        }

        public void AttachCannon(Cannon cannon)
        {
            _cannon=cannon;
        }
        public SpaceshipController(Game game) : base(game)
        {
            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentKs = Keyboard.GetState();
            Keys[] keys = _previousKs.GetPressedKeys();
            Vector2 delta = new Vector2(0, 0);
            foreach (var key in keys)
            {
                switch (key)
                {
                    case Keys.Left: // корабль влево
                        delta.X = -5;
                        break;
                    case Keys.Right: // корабль вправо
                        delta.X = +5;
                        break;
                    case Keys.Escape:// выход из игры
                        Game.Exit();
                        break;
                    case Keys.Space://выстрел
                        if (currentKs.IsKeyUp(Keys.Space) && _cannon.Texture!=null)
                        {
                            _cannon.Fire();
                        }
                        break;
                }
            }
             _subject.Move(delta);
            _previousKs = currentKs;
        }

    }
}
