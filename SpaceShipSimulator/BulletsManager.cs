using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShipSimulator
{
    public class BulletsManager : DrawableGameComponent
    {
        protected SpriteBatch _batch;
        protected List<GameUnit> _bullets;
        protected Rectangle _boundsRectangle;
        protected Texture2D _bulletText, _asteroidText;
        protected GameUnitManager _gameUnitManager;
        protected Vector2 _bulletSpeed;

        public void AddBullet(GameUnit bullet)
        {
            _bullets.Add(bullet);
            
        }
        public int BulletsCount
        {
            get { return _bullets.Count; }
        }

        public BulletsManager(Game game, GameUnitManager unitManager) : base(game)
        {
            game.Components.Add(this);
            _bullets = new List<GameUnit>();
            _bulletSpeed = new Vector2(0, -10);
            _bulletText= Game.Content.Load<Texture2D>("Rocket");
            _asteroidText = Game.Content.Load<Texture2D>("Asteroid");
            _gameUnitManager = unitManager;
        }

        public override void Update(GameTime gameTime)
        {
            if (_bullets.Count > 0)
            {
                for(int i=0;i<_bullets.Count;i++)
                {
                    _bullets[i].Move(_bulletSpeed);
                    if(_gameUnitManager.IsCollideWith(_bullets[i],_asteroidText)) // если снаряд попал в астероид
                        _bullets.RemoveAt(i);
                    else if (_bullets[i].Position.Y < 0)// если снаряд за пределами экрана
                       _bullets.RemoveAt(i);
                }                
            }
            base.Update(gameTime);
        }

       public override void Draw(GameTime gameTime)// отрисовка выпущенных снарядов
        {
            _batch = new SpriteBatch(Game.GraphicsDevice);
            if (_batch != null)
            {
                _batch.Begin();
                foreach (var bullet in _bullets)
                {
                    bullet.Draw(_batch);
                }
                _batch.End();
            }
            base.Draw(gameTime);
        }
    }
}
