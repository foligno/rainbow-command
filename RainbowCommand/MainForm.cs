using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RainbowCommand
{
    public partial class MainForm : Form
    {
        // Attributes
        MissileBase[] _bases = new MissileBase[3];
        MissileBase _activeBase;

        Vector _distanceVector;

        List<Explosion> _explosions = new List<Explosion>();
        List<Explosion> _deletedExplosions = new List<Explosion>();
        List<Missile> _missiles = new List<Missile>();
        List<Missile> _deletedMissiles = new List<Missile>();

        bool _gameRunning = true;

        // Double buffering to prevent flicker
        BufferedGraphics _backBuffer;
        BufferedGraphicsContext _backContext;

        bool _flashBool = false;
        bool _flashBool2 = false;

        int _score = 0;

        Random _randomNumber = new Random();
        int _timeOut = 0;

        Bitmap _back;

        public MainForm()
        {
            PointF pt = new PointF();
            float offset;

            InitializeComponent();

            offset = (float)gamePanel.Width * 0.25f;   // Divide by quarter

            _back = new Bitmap(GetType(), "back_01.png");

            // Add the bases along bottom of window
            pt.X = 0;
            pt.Y = (float)gamePanel.Height * 0.9f; // All bases at same height
            for (int i = 0; i < 3; i++)
            {
                pt.X = pt.X + offset;   // Move along in x-direction
                _bases[i] = new MissileBase(pt, 20);
            }

            _activeBase = _bases[0];
            _bases[0].IsActive = true;

            // Double buffering
            DoubleBuffered = false;
            _backContext = new BufferedGraphicsContext();
            _backBuffer = _backContext.Allocate(gamePanel.CreateGraphics(), gamePanel.DisplayRectangle);
        }

        // Redraw the game
        public void ReDraw()
        {
            Graphics g = _backBuffer.Graphics;

            g.Clear(Color.White);
            g.DrawImage(_back, 0, 0, 800, 600);

            foreach (MissileBase b in _bases)
            {
                b.Draw(g);
            }

            foreach (Missile m in _missiles)
            {
                m.Draw(g);
            }

            foreach (Explosion e in _explosions)
            {
                e.Draw(g);
            }

            g.FillRectangle(Brushes.LightGray, 10, 10, 170, 33);
            g.DrawRectangle(Pens.Black, 10, 10, 170, 33);
            g.DrawString("Score: " + _score, new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, 10, 10);

            if ((_score >= 100))
            {
                g.FillRectangle(Brushes.LightGray, 10, 50, 275, 33);
                g.DrawRectangle(Pens.Green, 10, 50, 275, 33);
                if (_flashBool)
                {
                    g.DrawString("DOUBLE WINNING", new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Cyan, 10, 50);
                    _flashBool = false;
                }
                else
                {
                    g.DrawString("DOUBLE WINNING", new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Green, 10, 50);
                    _flashBool = true;
                }
            }

            if ((_score >= 200))
            {
                g.FillRectangle(Brushes.LightGray, 10, 90, 290, 33);
                g.DrawRectangle(Pens.Yellow, 10, 90, 290, 33);
                if (_flashBool2)
                {
                    g.DrawString("YOU'RE AWESOME", new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Yellow, 10, 90);
                    _flashBool2 = false;
                }
                else
                {
                    g.DrawString("YOU'RE AWESOME", new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Magenta, 10, 90);
                    _flashBool2 = true;
                }
            }

            if (!_gameRunning)
            {
                g.DrawString("GAME OVER", new Font("Arial", 100, FontStyle.Bold, GraphicsUnit.Pixel), Brushes.Black, 70, gamePanel.Height / 3);
            }

            // Double Buffering
            _backBuffer.Render(gamePanel.CreateGraphics());
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            ReDraw();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (_gameRunning == true)
            {
                foreach (Explosion delExp in _deletedExplosions)
                {
                    _explosions.Remove(delExp);
                }

                foreach (Missile delMis in _deletedMissiles)
                {
                    _missiles.Remove(delMis);
                }

                _deletedExplosions.Clear();
                _deletedMissiles.Clear();

                foreach (Explosion exp in _explosions)
                {
                    if (exp.Update() == true)
                    {
                        _deletedExplosions.Add(exp);
                    }

                    foreach (Missile mis in _missiles)
                    {
                        if (mis.IsBad)
                        {
                            _distanceVector = new Vector(exp.Position.X - mis.Position.X, exp.Position.Y - mis.Position.Y);
                            if (_distanceVector.Length < exp.Radius)
                            {
                                _deletedMissiles.Add(mis);
                                _deletedExplosions.Add(exp);
                                _score += 5;
                                _activeBase.AddAmmo(2);
                            }
                        }
                    }

                    foreach (MissileBase mb in _bases)
                    {
                        _distanceVector = new Vector(exp.Position.X - mb.Position.X, exp.Position.Y - mb.Position.Y);
                        if (_distanceVector.Length < exp.Radius)
                        {
                            mb.RemoveHealth(20);
                            _deletedExplosions.Add(exp);
                        }
                    }
                }

                foreach (Missile mis in _missiles)
                {
                    if (mis.Update() == true)
                    {
                        Explosion explosion = new Explosion(mis.Position);
                        _explosions.Add(explosion);

                        _deletedMissiles.Add(mis);
                    }
                }

                _timeOut++;

                if (_timeOut > 50)
                {
                    int rand = GetRandomBase();
                    Missile m;
                    m = new Missile(new PointF((gamePanel.Width / 2f) + 10f, 45f), _bases[rand].MissileStartPosition, true);
                    _missiles.Add(m);
                    _timeOut = 0;
                }

                if ((!_bases[0].IsAlive) && (!_bases[1].IsAlive) && (!_bases[2].IsAlive))
                {
                    _gameRunning = false;
                }
            }

            ReDraw();
        }

        private int GetRandomBase()
        {
            int random = _randomNumber.Next(0, 3);

            while (_bases[random].IsAlive == false)
            {
                random = _randomNumber.Next(0, 3);
            }

            return random;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '1':
                    _activeBase.IsActive = false;
                    _activeBase = _bases[0];
                    _bases[0].IsActive = true;
                    break;
                case '2':
                    _activeBase.IsActive = false;
                    _activeBase = _bases[1];
                    _bases[1].IsActive = true;
                    break;
                case '3':
                    _activeBase.IsActive = false;
                    _activeBase = _bases[2];
                    _bases[2].IsActive = true;
                    break;
                case '7':
                    _score -= 5;
                    break;
                case '8':
                    _score += 5;
                    break;
                case '9':
                    _activeBase.RemoveHealth(5);
                    break;
                case '0':
                    _activeBase.AddHealth(5);
                    break;
                case '-':
                    _activeBase.RemoveAmmo(1);
                    break;
                case '=':
                    _activeBase.AddAmmo(1);
                    break;
                case 'K':
                case 'k':
                    _bases[0].RemoveHealth(100);
                    _bases[1].RemoveHealth(100);
                    _bases[2].RemoveHealth(100);
                    break;
                default:
                    break;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void gamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if ((e.Y < _activeBase.MissileStartPosition.Y - 70) && (_activeBase.HasAmmo) && (_activeBase.IsAlive))
            {
                Missile mis;

                mis = new Missile(_activeBase.MissileStartPosition, e.Location, false);

                _missiles.Add(mis);

                _activeBase.RemoveAmmo(1);
            }
        }
    }
}
