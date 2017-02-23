using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace RainbowCommand
{
    class MissileBase
    {
        // Attributes       
        private int _ammo;
        private Color _baseColor;
        private float _height = 113f;
        private int _health = 100;
        private Bitmap _image;
        private bool _isActive;
        private PointF _position;
        private float _width = 100f;
        private Bitmap _deadBase;
        private Bitmap _dyingBase;
        private Bitmap _aliveBase;
        private Bitmap _activeBase;

        // Constructors
        public MissileBase(PointF pos, int ammo)
        {
            _ammo = ammo;
            _baseColor = Color.Green;
            _position = pos;
            _deadBase = new Bitmap(GetType(), "dead_base.png");
            _deadBase.MakeTransparent(Color.White);
            _dyingBase = new Bitmap(GetType(), "dying_base.png");
            _dyingBase.MakeTransparent(Color.White);
            _aliveBase = new Bitmap(GetType(), "bases_02.png");
            _aliveBase.MakeTransparent(Color.White);
            _activeBase = new Bitmap(GetType(), "bases_01.png");
            _activeBase.MakeTransparent(Color.White);
            _image = _aliveBase;
        }

        public PointF MissileStartPosition
        {
            get
            {
                return new PointF(_position.X, _position.Y - (_height/2 + 1f ));
            }
        }

        public PointF Position
        {
            get
            {
                return _position;
            }
        }

        public bool IsActive
        {
            set
            {
                _isActive = value;
                UpdateBase();
            }
        }

        public bool IsAlive
        {
            get
            {
                if (_health > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool HasAmmo
        {
            get
            {
                if (_ammo > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(_baseColor);
            SolidBrush brush = new SolidBrush(_baseColor);

            g.DrawImage(_image, _position.X - (_width / 2f), _position.Y - _height);

            g.FillRectangle(Brushes.LightGray, _position.X - 35, _position.Y + 5, 27, 22);
            g.DrawRectangle(Pens.Black, _position.X - 35, _position.Y + 5, 27, 22);
            g.DrawString("" + _ammo, new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, _position.X - 35, _position.Y + 5);

            g.FillRectangle(Brushes.LightGray, _position.X - 2, _position.Y + 5, 38, 22);
            g.DrawRectangle(pen, _position.X - 2, _position.Y + 5, 38, 22);
            g.DrawString("" + _health, new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Pixel), brush, _position.X - 2, _position.Y + 5);
            
            pen.Dispose();
        }

        public int AmmoAmount
        {
            get
            {
                return _ammo;
            }
        }

        public void AddAmmo(int amount)
        {
            _ammo += amount;
        }

        public void RemoveAmmo(int amount)
        {
            _ammo -= amount;
        }

        public int HealthAmount
        {
            get
            {
                return _health;
            }
        }

        public void AddHealth(int amount)
        {
            _health += amount;

            UpdateBase();
        }

        public void RemoveHealth(int amount)
        {
            _health -= amount;

            UpdateBase();
        }

        private void UpdateBase()
        {
            if (_health > 0)
            {
                if (_health < 25)
                {
                    _baseColor = Color.Red;
                }
                else
                {
                    if (_health < 50)
                    {
                        _baseColor = Color.Orange;
                        _image = _dyingBase;
                    }
                    else
                    {
                        if (_health < 75)
                        {
                            _baseColor = Color.Yellow;
                        }
                        else
                        {
                            _baseColor = Color.Green;
                        }

                        if (_isActive == true)
                        {
                            _image = _activeBase;
                        }
                        else
                        {
                            _image = _aliveBase;
                        }
                    }
                }
            }
            else
            {
                _image = _deadBase;
            }

            _image.MakeTransparent(Color.White);
        }
    }
}
