using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace RainbowCommand
{
    class Explosion
    {
        private PointF _position;
        private float _width;
        private float _height;

        private const float MaxExplosion = 100f;
        private const float ExplosionIncrease = 10f;

        public Explosion(PointF position)
        {
            _position = position;
            _width = 0f;
            _height = 0f;
        }

        public PointF Position
        {
            get
            {
                return _position;
            }
        }

        public float Radius
        {
            get
            {
                return _width;
            }
        }

        public bool Update()
        {
            // Grow explosion by ExplosionIncrease
            _width = _width + ExplosionIncrease;
            _height = _height + ExplosionIncrease;

            if (_width < MaxExplosion)
            {
                return false;   // Continue explosion
            }

            return true;    // Done clear explosion
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Red, _position.X - _width / 2f, _position.Y - _height / 2f, _width, _height);
        }

    }
}
