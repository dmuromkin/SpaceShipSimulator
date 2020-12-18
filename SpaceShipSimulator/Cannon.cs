using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceShipSimulator
{
    public class Cannon : GameUnitManager
    {
        protected new BulletsManager _bulletsManager;
        private GameUnit _owner;
        protected Texture2D _bulletTex;


        public void Fire()
        {
            GameUnit bullet = new GameUnit(_bulletTex);
            Vector2 position = new Vector2(_owner.Position.X + 40, _owner.Position.Y);
            bullet.SetPosition(position);
            _bulletsManager.AddBullet(bullet);
           
        }
        public Texture2D bull_Text
        {
            get { return _bulletTex; }
        }

        public Cannon(Texture2D tex, BulletsManager bulletsManager, GameUnit owner)
            : base(tex)
        {
            _bulletsManager = bulletsManager;
            _owner = owner;
            _bulletTex = tex;
        }
    }
}
