using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;   // To use Point class

namespace RainbowCommand
{
    class Vector
    {
        private float _x;
        private float _y;

        public Vector()
        {
            _x = _y = 0f;
        }

        public Vector(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public Vector(PointF pt)
        {
            _x = pt.X;
            _y = pt.Y;
        }

        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        // Multiply the vector by a scalar value
        public static Vector operator *(Vector v, float value)
        {
            return new Vector(v._x * value, v._y * value);
        }

        // Add two vectors
        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1._x + v2._x, v1._y + v2._y);
        }

        // Subtract two vectors
        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1._x - v2._x, v1._y - v2._y);
        }

        // Dot Product of two vectors
        public static float operator *(Vector v1, Vector v2)
        {
            // Dot product
            return v1._x * v2._x + v1._y * v2._y;
        }

        // Conversion from Vertex to Point
        public static implicit operator PointF(Vector v)
        {
            return new PointF(v._x, v._y);
        }

        // Conversion from Point to Vertex
        public static implicit operator Vector(PointF p)
        {
            return new Vector(p.X, p.Y);
        }

        // The scalar magnitude of the vector
        public float Length
        {
            get
            {
                return (float)Math.Sqrt(_x * _x + _y * _y);
            }
        }

        public void Normalize()
        {
            float len = Length;

            _x = _x/len;
            _y = _y/len;
        }
    }
}
