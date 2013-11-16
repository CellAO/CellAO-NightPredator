#region License

// Copyright (c) 2005-2013, CellAO Team
// 
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
// PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
// PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
// NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

namespace CellAO.Core.Vector
{
    #region Usings ...

    using System;
    using System.Globalization;

    using CellAO.Interfaces;

    #endregion

    /// <summary>
    /// Vector3 Class
    /// </summary>
    public class Vector3 : IVector3
    {
        #region Static Fields

        /// <summary>
        /// Vector running along the X Axis
        /// </summary>
        public static readonly Vector3 AxisX = new Vector3(1, 0, 0);

        /// <summary>
        /// Vector running along the Y Axis
        /// </summary>
        public static readonly Vector3 AxisY = new Vector3(0, 1, 0);

        /// <summary>
        /// Vector running along the Z Axis
        /// </summary>
        public static readonly Vector3 AxisZ = new Vector3(0, 0, 1);

        /// <summary>
        /// Smallest possible positive Vector
        /// </summary>
        public static readonly Vector3 Epsilon = new Vector3(double.Epsilon, double.Epsilon, double.Epsilon);

        /// <summary>
        /// Largest possible Vector
        /// </summary>
        public static readonly Vector3 MaxValue = new Vector3(double.MaxValue, double.MaxValue, double.MaxValue);

        /// <summary>
        /// Smallest possible Vector
        /// </summary>
        public static readonly Vector3 MinValue = new Vector3(double.MinValue, double.MinValue, double.MinValue);

        /// <summary>
        /// Vector at the Origin
        /// </summary>
        public static readonly Vector3 Origin = new Vector3(0, 0, 0);

        #endregion

        #region Fields

        /// <summary>
        /// Tolerance used in Equals calculations to allow for floating point errors
        /// </summary>
        public double floatingPointTolerance = double.Epsilon;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates a new Vector3 from its components
        /// </summary>
        /// <param name="x">
        /// x component of the Vector
        /// </param>
        /// <param name="y">
        /// y component of the Vector
        /// </param>
        /// <param name="z">
        /// z component of the Vector
        /// </param>
        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Absolute value of the Vector
        /// </summary>
        public double Magnitude
        {
            get
            {
                return Math.Sqrt(Math.Pow(this.x, 2) + Math.Pow(this.y, 2) + Math.Pow(this.z, 2));
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        "value", 
                        value, 
                        "The magnitude of a Vector must be positive or 0.");
                }
                else if (this.Magnitude == 0)
                {
                    throw new DivideByZeroException("Can not set the magnitude of a Vector with no direction");
                }
                else
                {
                    double factor = value / this.Magnitude;
                    this.x = this.x * factor;
                    this.y = this.y * factor;
                    this.z = this.z * factor;
                }
            }
        }

        /// <summary>
        /// X coordinate of the Vector
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// X coordinate of the Vector
        /// </summary>
        public float xf
        {
            get
            {
                return (float)this.x;
            }
        }

        /// <summary>
        /// Y coordinate of the Vector
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Y coordinate of the Vector
        /// </summary>
        public float yf
        {
            get
            {
                return (float)this.y;
            }
        }

        /// <summary>
        /// Z coordinate of the Vector
        /// </summary>
        public double z { get; set; }

        /// <summary>
        /// Z coordinate of the Vector
        /// </summary>
        public float zf
        {
            get
            {
                return (float)this.z;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns the Absolute value of the Vector
        /// </summary>
        /// <param name="v1">
        /// Vector
        /// </param>
        /// <returns>
        /// </returns>
        public static double Abs(Vector3 v1)
        {
            return v1.Magnitude;
        }

        /// <summary>
        /// Returns the Cross Product of two Vectors
        /// </summary>
        /// <param name="vLeft">
        /// Vector 1
        /// </param>
        /// <param name="vRight">
        /// Vector 2
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Cross(IVector3 vLeft, IVector3 vRight)
        {
            return new Vector3(
                vLeft.y * vRight.z - vLeft.z * vRight.y, 
                vLeft.z * vRight.x - vLeft.x * vRight.z, 
                vLeft.x * vRight.y - vLeft.y * vRight.x);
        }

        /// <summary>
        /// Returns the Dot Product of two Vectors
        /// </summary>
        /// <param name="v1">
        /// Vector 1
        /// </param>
        /// <param name="v2">
        /// Vector 2
        /// </param>
        /// <returns>
        /// </returns>
        public static double Dot(IVector3 v1, IVector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        /// <summary>
        /// Returns true if the Vector is a Unit Vector (ie, is of magnitude 1)
        /// </summary>
        /// <param name="v1">
        /// Vector
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsUnitVector(Vector3 v1)
        {
            return Math.Abs(v1.Magnitude - 1) <= double.Epsilon;
        }

        /// <summary>
        /// Returns the larger of two Vectors
        /// </summary>
        /// <param name="v1">
        /// Vector 1
        /// </param>
        /// <param name="v2">
        /// Vector 2
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return v1 > v2 ? v1 : v2;
        }

        /// <summary>
        /// Returns the smaller of two Vectors
        /// </summary>
        /// <param name="v1">
        /// Vector 1
        /// </param>
        /// <param name="v2">
        /// Vector 2
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return v1 < v2 ? v1 : v2;
        }

        /// <summary>
        /// Returns the Normalized Vector
        /// </summary>
        /// <param name="v1">
        /// Vector
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3 Normalize(Vector3 v1)
        {
            if (v1.Magnitude == 0)
            {
                throw new DivideByZeroException("Can not normalize a Vector with no direction");
            }
            else
            {
                Vector3 UnitVector = v1;

                UnitVector.Magnitude = 1;

                return UnitVector;
            }
        }

        /// <summary>
        /// Operator +
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        /// <summary>
        /// Operator /
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="n2">Number 2</param>
        public static Vector3 operator /(Vector3 v1, double n2)
        {
            return new Vector3(v1.x / n2, v1.y / n2, v1.z / n2);
        }

        /// <summary>
        /// Operator ==
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return v1.Equals(v2);
        }

        /// <summary>
        /// Operator Greater Than
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static bool operator >(Vector3 v1, Vector3 v2)
        {
            return v1.Magnitude > v2.Magnitude;
        }

        /// <summary>
        /// Operator Greater Than or Equal
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static bool operator >=(Vector3 v1, Vector3 v2)
        {
            return v1.Magnitude >= v2.Magnitude;
        }

        /// <summary>
        /// Operator !=
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !v1.Equals(v2);
        }

        /// <summary>
        /// Operator Less Than
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static bool operator <(Vector3 v1, Vector3 v2)
        {
            return v1.Magnitude < v2.Magnitude;
        }

        /// <summary>
        /// Operator Less Than or Equal
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static bool operator <=(Vector3 v1, Vector3 v2)
        {
            return v1.Magnitude <= v2.Magnitude;
        }

        /// <summary>
        /// Operator *
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="n2">Number 2</param>
        public static Vector3 operator *(Vector3 v1, double n2)
        {
            return new Vector3(v1.x * n2, v1.y * n2, v1.z * n2);
        }

        /// <summary>
        /// Operator *
        /// </summary>
        /// <param name="n1">Number 1</param>
        /// <param name="v2">Vector 2</param>
        public static Vector3 operator *(double n1, Vector3 v2)
        {
            return v2 * n1;
        }

        /// <summary>
        /// Operator -
        /// </summary>
        /// <param name="v1">Vector 1</param>
        /// <param name="v2">Vector 2</param>
        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        /// <summary>
        /// Operator +
        /// </summary>
        /// <param name="v1">Vector 1</param>
        public static Vector3 operator -(Vector3 v1)
        {
            return new Vector3(-v1.x, -v1.y, -v1.z);
        }

        /// <summary>
        /// Operator +
        /// </summary>
        /// <param name="v1">Vector 1</param>
        public static Vector3 operator +(Vector3 v1)
        {
            return new Vector3(+v1.x, +v1.y, +v1.z);
        }

        /// <summary>
        /// Returns the Absolute value of the Vector
        /// </summary>
        /// <returns>
        /// </returns>
        public double Abs()
        {
            return this.Magnitude;
        }

        /// <summary>
        /// Returns the Cross Product of two Vectors
        /// </summary>
        /// <param name="vRight">
        /// Other Vector
        /// </param>
        /// <returns>
        /// </returns>
        public Vector3 Cross(IVector3 vRight)
        {
            return Cross(this, vRight);
        }

        /// <summary>
        /// Returns the Dot Product of two Vectors
        /// </summary>
        /// <param name="v1">
        /// Other Vector
        /// </param>
        /// <returns>
        /// </returns>
        public double Dot(IVector3 v1)
        {
            return Dot(this, v1);
        }

        /// <summary>
        /// Determines if Object is the same as this instance
        /// </summary>
        /// <param name="o">
        /// Object
        /// </param>
        /// <returns>
        /// </returns>
        public override bool Equals(object o)
        {
            if (o.GetType() != typeof(Vector3))
            {
                return false;
            }

            Vector3 v1 = this;
            Vector3 v2 = (Vector3)o;

            return (Math.Abs(v1.x - v2.x) <= this.floatingPointTolerance)
                   && (Math.Abs(v1.y - v2.y) <= this.floatingPointTolerance)
                   && (Math.Abs(v1.z - v2.z) <= this.floatingPointTolerance);
        }

        /// <summary>
        /// Returns the hash code for this instance
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GetHashCode()
        {
            return this.x.GetHashCode() + this.y.GetHashCode() + this.z.GetHashCode();
        }

        /// <summary>
        /// Returns true if the Vector is a Unit Vector (ie, is of magnitude 1)
        /// </summary>
        /// <returns>
        /// </returns>
        public bool IsUnitVector()
        {
            return IsUnitVector(this);
        }

        /// <summary>
        /// Returns the larger of two Vectors
        /// </summary>
        /// <param name="v1">
        /// Other Vector
        /// </param>
        /// <returns>
        /// </returns>
        public Vector3 Max(Vector3 v1)
        {
            return Max(this, v1);
        }

        /// <summary>
        /// Returns the smaller of two Vectors
        /// </summary>
        /// <param name="v1">
        /// Other Vector
        /// </param>
        /// <returns>
        /// </returns>
        public Vector3 Min(Vector3 v1)
        {
            return Min(this, v1);
        }

        /// <summary>
        /// Returns the Normalized Vector
        /// </summary>
        /// <returns>
        /// </returns>
        public Vector3 Normalize()
        {
            return Normalize(this);
        }

        /// <summary>
        /// Converts the Vector3 to a string representation
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2})", this.x, this.y, this.z);
        }

        #endregion
    }
}