#region License

// Copyright (c) 2005-2014, CellAO Team
// 
// 
// All rights reserved.
// 
// 
// Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
// 
// 
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//     * Neither the name of the CellAO Team nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
// 
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
// 

#endregion

#region Using

// Needs Checking might be .Single i dunno.

#endregion

namespace CellAO.Core.Vector
{
    #region Usings ...

    using System;

    using CellAO.Interfaces;

    using MathNet.Numerics.LinearAlgebra.Single;

    #endregion

    /// <summary>
    /// Quaternion Class
    /// </summary>
    public class Quaternion : IQuaternion
    {
        #region Constructors and Destructors

        /// <summary>
        /// Create a Quaternion from its Components
        /// </summary>
        /// <param name="x">
        /// x component of the Quaternion
        /// </param>
        /// <param name="y">
        /// y component of the Quaternion
        /// </param>
        /// <param name="z">
        /// z component of the Quaternion
        /// </param>
        /// <param name="w">
        /// w component of the Quaternion
        /// </param>
        public Quaternion(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Create a Quaternion from a Vector3 and an angle
        /// </summary>
        /// <param name="v">
        /// Vector of Rotation
        /// </param>
        /// <param name="angle">
        /// Angle of Rotation
        /// </param>
        public Quaternion(Vector3 v, double angle)
        {
            double sinAngle;
            Vector3 vNormalized;

            vNormalized = v.Normalize();

            sinAngle = Math.Sin(angle / 2);
            this.x = vNormalized.x * sinAngle;
            this.y = vNormalized.y * sinAngle;
            this.z = vNormalized.z * sinAngle;

            this.w = Math.Cos(angle / 2);
        }

        /// <summary>
        /// </summary>
        public Quaternion()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.w = 0;
        }

        /// <summary>
        /// Create a Quaternion representation from a Vector3 (w is 0)
        /// </summary>
        /// <param name="v">
        /// Vector of Rotation
        /// </param>
        public Quaternion(IVector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
            this.w = 0;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Return the Magnitude of the Quaternion
        /// </summary>
        public double magnitude
        {
            get
            {
                return Math.Sqrt((this.x * this.x) + (this.y * this.y) + (this.z * this.z) + (this.w * this.w));
            }
        }

        /// <summary>
        /// Return the pitch/attitude of the Quaternion (flight dynamics style). Value pi/2 through -pi/2 or 90 to -90 if converted to degrees (90 is nose in the air, 0 is level, -90 is nose to the ground)
        /// </summary>
        public double pitch
        {
            get
            {
                return -2
                       * Math.Atan2(
                           (2 * this.x * this.w) - (2 * this.y * this.z),
                           1 - (2 * this.x * this.y) - (2 * this.z * this.z));
            }
        }

        /// <summary>
        /// Return the roll/bank of the Quaternion (flight dynamics style). Value range unknown, but should always be 0 really (give or take floating point errors)
        /// </summary>
        public double roll
        {
            get
            {
                return Math.Asin((2 * this.x * this.y) + (2 * this.z * this.w));

                // In AO we can't roll, so this is always 0 (give or take floating point errors)
            }
        }

        /// <summary>
        /// w component of the Quaternion
        /// </summary>
        public double w { get; set; }

        /// <summary>
        /// w component of the Quaternion
        /// </summary>
        public float wf
        {
            get
            {
                return (float)this.w;
            }
        }

        /// <summary>
        /// x component of the Quaternion
        /// </summary>
        public double x { get; set; }

        /// <summary>
        /// x component of the Quaternion
        /// </summary>
        public float xf
        {
            get
            {
                return (float)this.x;
            }
        }

        /// <summary>
        /// y component of the Quaternion
        /// </summary>
        public double y { get; set; }

        /// <summary>
        /// Return the yaw/heading of the Quaternion (flight dynamics style). Value 0 - 2pi Radians or 0 to 360 if converted to degrees (North turning clockwise to a complete revolution)
        /// </summary>
        public double yaw
        {
            get
            {
                double _yaw = Math.Atan2(
                    (2 * this.y * this.w) - (2 * this.x * this.z),
                    1 - (2 * this.y * this.y) - (2 * this.z * this.z));
                if (_yaw < 0)
                {
                    // So we get a positive number
                    _yaw += 2 * Math.PI;
                }

                return _yaw;
            }
        }

        /// <summary>
        /// y component of the Quaternion
        /// </summary>
        public float yf
        {
            get
            {
                return (float)this.y;
            }
        }

        /// <summary>
        /// z component of the Quaternion
        /// </summary>
        public double z { get; set; }

        /// <summary>
        /// z component of the Quaternion
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
        /// Return the Conjugate (Spacial Inverse) of the Quaternion
        /// </summary>
        /// <returns>
        /// </returns>
        public IQuaternion Conjugate()
        {
            return Conjugate(this);
        }

        /// <summary>
        /// Fill this info in
        /// </summary>
        /// <param name="vDirection">
        /// </param>
        /// <returns>
        /// Fill this info in
        /// </returns>
        public IQuaternion GenerateRotationFromDirectionVector(IVector3 vDirection)
        {
            // Step 1. Setup basis vectors describing the rotation given the input vector and assuming an initial up direction of (0, 1, 0)
            Vector3 vUp = new Vector3(0, 1.0f, 0.0f); // Y Up vector
            Vector3 vRight = Vector3.Cross(vUp, vDirection); // The perpendicular vector to Up and Direction
            vUp = Vector3.Cross(vDirection, vRight); // The actual up vector given the direction and the right vector

            // Step 2. Put the three vectors into the matrix to bulid a basis rotation matrix
            // This step isnt necessary, but im adding it because often you would want to convert from matricies to quaternions instead of vectors to quaternions
            // If you want to skip this step, you can use the vector values directly in the quaternion setup below
            Matrix mBasis = new DenseMatrix(4, 4);
            mBasis.SetRow(0, new[] { (float)vRight.x, (float)vRight.y, (float)vRight.z, 0.0f });
            mBasis.SetRow(1, new[] { (float)vUp.x, (float)vUp.y, (float)vUp.z, 0.0f });
            mBasis.SetRow(2, new[] { (float)vDirection.x, (float)vDirection.y, (float)vDirection.z, 0.0f });
            mBasis.SetRow(3, new[] { 0.0f, 0.0f, 0.0f, 1.0f });

            // Step 3. Build a quaternion from the matrix
            double dfWScale = Math.Sqrt(1.0f + mBasis.At(0, 0) + mBasis.At(1, 1) + mBasis.At(2, 2)) / 2.0f * 4.0;
            if (dfWScale == 0.0)
            {
                Quaternion q = new Quaternion(0, 1, 0, 0);
                return q;
            }

            Quaternion qrot = new Quaternion(
                (float)((mBasis.At(3, 2) - mBasis.At(2, 3)) / dfWScale),
                (float)((mBasis.At(0, 2) - mBasis.At(2, 0)) / dfWScale),
                (float)((mBasis.At(1, 0) - mBasis.At(0, 1)) / dfWScale),
                (float)Math.Sqrt(1.0f + mBasis.At(0, 0) + mBasis.At(1, 1) + mBasis.At(2, 2)) / 2.0f);
            return qrot;
        }

        /// <summary>
        /// Returns the Hamilton Product of two Quaternions
        /// </summary>
        /// <param name="vRight">
        /// Other Quaternion
        /// </param>
        /// <returns>
        /// </returns>
        public IQuaternion Hamilton(IQuaternion vRight)
        {
            return Hamilton(this, vRight);
        }

        /// <summary>
        /// Return a Normalized Quaternion
        /// </summary>
        /// <returns>
        /// </returns>
        public IQuaternion Normalize()
        {
            return Normalize(this);
        }

        /// <summary>
        /// Return a Vector rotated around the Quaternion
        /// Note: Only works for Unit Quaternions at present due to lazyness (AO-provided Quaternions are all Unit Quaternions)
        /// </summary>
        /// <param name="v1">
        /// Vector
        /// </param>
        /// <returns>
        /// </returns>
        public IVector3 RotateVector3(IVector3 v1)
        {
            return RotateVector3(this, v1);
        }

        /// <summary>
        /// Return a Vector representation of a Quaternion (w is dropped)
        /// </summary>
        /// <returns>
        /// </returns>
        public IVector3 VectorRepresentation()
        {
            return VectorRepresentation(this);
        }

        /// <summary>
        /// Update a Quaternion to a new value using its Components
        /// </summary>
        /// <param name="x">
        /// x component of the Quaternion
        /// </param>
        /// <param name="y">
        /// y component of the Quaternion
        /// </param>
        /// <param name="z">
        /// z component of the Quaternion
        /// </param>
        /// <param name="w">
        /// w component of the Quaternion
        /// </param>
        public void update(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        /// <summary>
        /// Return the Conjugate of the Quaternion
        /// </summary>
        /// <param name="q1">
        /// Quaternion
        /// </param>
        /// <returns>
        /// </returns>
        public static Quaternion Conjugate(Quaternion q1)
        {
            return new Quaternion(-q1.x, -q1.y, -q1.z, q1.w);
        }

        /// <summary>
        /// Returns the Hamilton Product of two Quaternions
        /// </summary>
        /// <param name="vLeft">
        /// Quaternion 1
        /// </param>
        /// <param name="vRight">
        /// Quaternion 2
        /// </param>
        /// <returns>
        /// </returns>
        public static Quaternion Hamilton(IQuaternion vLeft, IQuaternion vRight)
        {
            double w = (vLeft.w * vRight.w) - (vLeft.x * vRight.x) - (vLeft.y * vRight.y) - (vLeft.z * vRight.z);
            double x = (vLeft.w * vRight.x) + (vLeft.x * vRight.w) + (vLeft.y * vRight.z) - (vLeft.z * vRight.y);
            double y = (vLeft.w * vRight.y) - (vLeft.x * vRight.z) + (vLeft.y * vRight.w) + (vLeft.z * vRight.x);
            double z = (vLeft.w * vRight.z) + (vLeft.x * vRight.y) - (vLeft.y * vRight.x) + (vLeft.z * vRight.w);

            return new Quaternion(x, y, z, w);
        }

        /// <summary>
        /// Return a Normalized Quaternion
        /// </summary>
        /// <param name="q1">
        /// Quaternion
        /// </param>
        /// <returns>
        /// </returns>
        public static IQuaternion Normalize(IQuaternion q1)
        {
            double mag = q1.magnitude;

            return new Quaternion(q1.x / mag, q1.y / mag, q1.z / mag, q1.w / mag);
        }

        /// <summary>
        /// Return a Vector rotated around the Quaternion
        /// </summary>
        /// <param name="q1">
        /// Quaternion
        /// </param>
        /// <param name="v2">
        /// Vector
        /// </param>
        /// <returns>
        /// </returns>
        public static IVector3 RotateVector3(IQuaternion q1, IVector3 v2)
        {
            Quaternion QuatVect = new Quaternion(v2.x, v2.y, v2.z, 0);
            Quaternion QuatNorm = (Quaternion)q1.Normalize();
            Quaternion Result = Hamilton(Hamilton(QuatNorm, QuatVect), QuatNorm.Conjugate());
            return new Vector3(Result.x, Result.y, Result.z);
        }

        /// <summary>
        /// Return a Vector representation of a Quaternion (w is dropped)
        /// </summary>
        /// <param name="q1">
        /// Quaternion
        /// </param>
        /// <returns>
        /// </returns>
        public static IVector3 VectorRepresentation(Quaternion q1)
        {
            return new Vector3(q1.x, q1.y, q1.z);
        }

        /// <summary>
        /// </summary>
        /// <param name="q">
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator SmokeLounge.AOtomation.Messaging.GameData.Quaternion(Quaternion q)
        {
            return new SmokeLounge.AOtomation.Messaging.GameData.Quaternion() { X = q.xf, Y = q.yf, Z = q.zf, W = q.wf };
        }

        /// <summary>
        /// </summary>
        /// <param name="q">
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator Quaternion(SmokeLounge.AOtomation.Messaging.GameData.Quaternion q)
        {
            return new Quaternion(q.X, q.Y, q.Z, q.W);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", this.x, this.y, this.z, this.w);
        }

        #endregion
    }
}