using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Reflection;

namespace SpaceShipSimulator
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpaceshipController _spaceshipController;// управление ракетой
        private HighScoreFile _scoreFile;// файл с лучшим результатом
        private BulletsManager _bulletsManager;
        private Texture2D _backgroundTexture, _spaceShipTexture, _bulletTexture;
        private Texture2D _gameOverTexture, _asteroidTexture;
        private GameUnitManager _unitManager;
        private int _screenWidth, _screenHeight;
        private int _prevHighscore, _points;// рекорд, текущее кол-во очков
        private double _miliseconds;// период времени для добавления нового астероида 
        private SpriteFont _spritefont;
        private bool collision;// проверка столкновения
       

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _screenWidth = 1920;
            _screenHeight = 1080;
            _graphics.PreferredBackBufferWidth = _screenWidth;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            
            collision = false; //столкновения не было
            _spaceshipController = new SpaceshipController(this);
            _scoreFile = new HighScoreFile();
            _unitManager = new GameUnitManager(_spaceShipTexture);
            _bulletsManager = new BulletsManager(this,_unitManager);
            _unitManager.AddPlayer(_spaceShipTexture,_spaceshipController);// добавление ракеты на позицию по умолчанию
            _unitManager.AddEnemy(_asteroidTexture);// добавление 1-го астероида
            _unitManager.CreateCannon(_bulletTexture, _bulletsManager, _spaceshipController);// инициализация пушки
            _unitManager.SetLeftBorder = 500;//левая граница диапазона возможных координат нового астероида
            _unitManager.SetRightBorder = 800;// правая граница
            _prevHighscore = _scoreFile.Read();// предыдущий рекорд

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _spaceShipTexture = Content.Load<Texture2D>("Spacecraft");
            _asteroidTexture = Content.Load<Texture2D>("Asteroid");
            _backgroundTexture = Content.Load<Texture2D>("Space");
            _bulletTexture = Content.Load<Texture2D>("Rocket");
            _gameOverTexture = Content.Load<Texture2D>("GameOver");
            _spritefont= Content.Load<SpriteFont>("Font");
           
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (collision == false)// пока нет столкновения игрока с астероидом
            {
                _miliseconds += gameTime.ElapsedGameTime.TotalMilliseconds;
                _unitManager.MoveUnits(gameTime, _screenHeight);
                _points = _unitManager.Points;// заработанные очки
                if (_unitManager.IsCollideWith(_asteroidTexture))// если есть столкновение
                {
                    collision = true;
                    // текстура снаряда из пушки null для предотвращения отрисовки новых выстрелов
                    _unitManager.CreateCannon(null, _bulletsManager, _spaceshipController);

                    if (_prevHighscore < _points)
                        _scoreFile.Write(_points);// запись в файл нового рекорда
                }
                if (_miliseconds >= 2000)// через каждые 2 секунды новый астероид
                {
                    _miliseconds = 0;
                    _unitManager.AddEnemy(_asteroidTexture);
                }
            }
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (!collision)
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, Color.White);
                _spriteBatch.DrawString(_spritefont, "Points : " + _points.ToString(), new Vector2(50, 300), Color.White);
                _spriteBatch.DrawString(_spritefont, "Highscore : " + _prevHighscore.ToString(), new Vector2(50, 100), Color.White);
                _unitManager.DrawPlayer(_spriteBatch);
                _unitManager.DrawEnemy(_spriteBatch);
                _spriteBatch.End();
            }  
            else
            {
                _spriteBatch.Begin();
                _spriteBatch.Draw(_gameOverTexture, Vector2.Zero, Color.White);
                if (_prevHighscore < _points)
                {
                    _spriteBatch.DrawString(_spritefont, "New highscore : " + _points.ToString() + " points", new Vector2(_screenWidth / 2 - 500, 100), Color.White);
                }
                else
                   _spriteBatch.DrawString(_spritefont, "Points : " + _points.ToString(), new Vector2(_screenWidth / 2 - 200, 100), Color.White);
                _spriteBatch.DrawString(_spritefont, "Press Esc to Exit ", new Vector2(700, 900), Color.White);
                _spriteBatch.End();
            }
                base.Draw(gameTime);
        }
    }
}
