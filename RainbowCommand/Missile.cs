using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RainbowCommand
{
    class Missile
    {
        private const float SPEED = 15.0f;

        private PointF _startPos;
        private PointF _currentPos;
        private PointF _targetPos;
        private Vector _velocity;
        private bool _isBad;

        public Missile(PointF startPos, PointF targetPos, bool isBad)
        {
            _isBad = isBad;
            _startPos = startPos;
            _targetPos = targetPos;

            _currentPos = _startPos;

            _velocity = new Vector(_targetPos) - new Vector(_startPos);

            _velocity.Normalize();

            _velocity = _velocity * SPEED;
        }

        public PointF Position
        {
            get
            {
                return _currentPos;
            }
        }

        public bool IsBad
        {
            get
            {
                return _isBad;
            }
        }

        public bool Update()
        {
            bool reachedTarget = false;
            Vector vectorToTarget;
            float remainingDistance;

            vectorToTarget = new Vector(_targetPos) - new Vector(_currentPos);

            remainingDistance = vectorToTarget.Length;

            if (remainingDistance > SPEED)
            {
                _currentPos = _currentPos + _velocity;
            }
            else
            {
                _currentPos = _currentPos + vectorToTarget;
                reachedTarget = true;
            }
            
            return reachedTarget;
        }

        public void Draw(Graphics g)
        {
            Pen pen;
            if (_isBad)
            {
                pen = new Pen(Color.Black, 4);
                g.DrawLine(pen, _startPos.X - 8, _startPos.Y, _currentPos.X - 8, _currentPos.Y);
                pen = new Pen(Color.DarkGreen, 4);
                g.DrawLine(pen, _startPos.X - 4, _startPos.Y, _currentPos.X - 4, _currentPos.Y);
                pen = new Pen(Color.Green, 4);
                g.DrawLine(pen, _startPos.X, _startPos.Y, _currentPos.X, _currentPos.Y);
                pen = new Pen(Color.DarkGreen, 4);
                g.DrawLine(pen, _startPos.X + 4, _startPos.Y, _currentPos.X + 4, _currentPos.Y);
                pen = new Pen(Color.Black, 4);
                g.DrawLine(pen, _startPos.X + 8, _startPos.Y, _currentPos.X + 8, _currentPos.Y);
            }
            else
            {
                pen = new Pen(Color.Red, 4);
                g.DrawLine(pen, _startPos.X - 8, _startPos.Y, _currentPos.X - 8, _currentPos.Y);
                pen = new Pen(Color.Yellow, 4);
                g.DrawLine(pen, _startPos.X - 4, _startPos.Y, _currentPos.X - 4, _currentPos.Y);
                pen = new Pen(Color.Magenta, 4);
                g.DrawLine(pen, _startPos.X, _startPos.Y, _currentPos.X, _currentPos.Y);
                pen = new Pen(Color.Blue, 4);
                g.DrawLine(pen, _startPos.X + 4, _startPos.Y, _currentPos.X + 4, _currentPos.Y);
                pen = new Pen(Color.Orange, 4);
                g.DrawLine(pen, _startPos.X + 8, _startPos.Y, _currentPos.X + 8, _currentPos.Y);
            }
            pen.Dispose();
        }
    }
}
