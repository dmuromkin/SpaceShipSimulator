using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpaceShipSimulator
{
    public class GameUnitManager : GameUnit
    {
        private List<GameUnit> _enemyUnits = new List<GameUnit>(); // список астероидов
        private Cannon _cannon;
        private GameUnit _playerShip;// корабль игрока
        private int _borderLeft, _borderRight, points;
        private Vector2 _speed;// скорость астероидов
        private Rectangle _unitRect, _enemyRect;


        public GameUnitManager(Texture2D tex) : base(tex)
        {
            _speed = new Vector2(0, 5);
            _borderLeft = 500;
            _borderRight = 1000;
        }

        public int SetLeftBorder
        {
            set { _borderLeft = value; }
        }

        public int SetRightBorder
        {
            set { _borderRight = value; }
        }

        public int Points
        {
            get { return points; }
        }
        
        public void AddEnemy(Texture2D texture)// добавление нового астероида со случайными координатами
        {

            _enemyUnits.Add(new GameUnit(texture));
            var rand = new Random();
            int lastIndex = _enemyUnits.Count - 1;
            _enemyUnits[lastIndex].SetPosition(rand.Next(_borderLeft, _borderRight), 0);
            
        }


        public bool IsCollideWith(Texture2D asteroidText)// проверка столкновения астероида с кораблем
        {
            bool result = false;
            _unitRect = new Rectangle
            {
                Width = _playerShip.Texture.Width,
                Height = _playerShip.Texture.Height
            };
            _enemyRect = new Rectangle
            {
                Width = asteroidText.Width,
                Height = asteroidText.Height
            };
            _unitRect.X = (int)(_playerShip.Position.X - _unitRect.Width / 2);
            _unitRect.Y = (int)(_playerShip.Position.Y - _unitRect.Height / 2 - 30);

            for (int i = 0; i < _enemyUnits.Count; i++)
            {
                _enemyRect.X = (int)(_enemyUnits[i].Position.X - _enemyRect.Width / 2);
                _enemyRect.Y = (int)(_enemyUnits[i].Position.Y - _enemyRect.Height / 2 - 30);

                result = _unitRect.Intersects(_enemyRect);
                if (result)
                {
                    
                    _enemyUnits.Clear();
                    return result;
                }

            }

            return result;
        }

        // проверка столкновения астероида с выпущенной ракетой
        public bool IsCollideWith(GameUnit unit, Texture2D asteroidText) 
        {
            bool result = false;
            _unitRect = new Rectangle
            {
                Width = unit.Texture.Width,
                Height = unit.Texture.Height
            };
            _enemyRect = new Rectangle
            {
                Width = asteroidText.Width,
                Height = asteroidText.Height
            };
            _unitRect.X = (int)(unit.Position.X - _unitRect.Width / 2);
            _unitRect.Y = (int)(unit.Position.Y - _unitRect.Height / 2 - 30);
            //List<GameUnit> _enemies = gameUnits;

            for (int i = 0; i < _enemyUnits.Count; i++)
            {
                _enemyRect.X = (int)(_enemyUnits[i].Position.X - _enemyRect.Width / 2);
                _enemyRect.Y = (int)(_enemyUnits[i].Position.Y - _enemyRect.Height / 2 - 30);

                result = _unitRect.Intersects(_enemyRect);
                if (result)
                {
                    _enemyUnits.RemoveAt(i);
                    points++;// очки за уничтожение ракеты
                    return result;
                }

            }

            return result;
        }

        //перемещение астероидов
        public void MoveUnits(GameTime gameTime, int bottomBorder)
        {
            for (int i = 0; i < _enemyUnits.Count; i++)
            {
                _enemyUnits[i].Update(gameTime, _speed);
                if (_enemyUnits[i].Position.Y > bottomBorder)
                {
                    _enemyUnits.RemoveAt(i);
                    points++;// очки за уворот от астероида
                }
            }
        }
        // создание пушки
        public void CreateCannon(Texture2D tex, BulletsManager manager, SpaceshipController spaceshipController)
        {
            _cannon = new Cannon(tex, manager, _playerShip);
            spaceshipController.AttachCannon(_cannon);
        }

        // создание корабля игрока
        public void AddPlayer(Texture2D spaceshipTex,SpaceshipController spaceshipController)
        {
            _playerShip = (new GameUnit(spaceshipTex));
            _playerShip.SetPosition(850, 850);
            spaceshipController.Attach(_playerShip);
        }


        // отрисовка корабля игрока
        public void DrawPlayer(SpriteBatch batch)
        {
            batch.Draw(_playerShip.Texture,
                _playerShip.Position,
                null,
                _color
                );
            
        }

        // отрисовка астероидов
        public void DrawEnemy(SpriteBatch batch)
        {
            if (_enemyUnits.Count > 0)
            {
                for (int i = 0; i < _enemyUnits.Count; i++)
                    batch.Draw(_enemyUnits[i].Texture,
                    _enemyUnits[i].Position,
                    null,
                    _color
                    );
            }
            
        }
       
    }
}
