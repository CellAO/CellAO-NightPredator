#region License

// Copyright (c) 2005-2013, CellAO Team
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
// Last modified: 2013-10-30 22:52
// Created:       2013-10-30 17:25

#endregion

namespace CellAO.Interfaces
{
    public interface IQuaternion
    {
        /// <summary>
        /// x component of the Quaternion
        /// </summary>
        float xf { get; }

        /// <summary>
        /// y component of the Quaternion
        /// </summary>
        float yf { get; }

        /// <summary>
        /// z component of the Quaternion
        /// </summary>
        float zf { get; }

        /// <summary>
        /// w component of the Quaternion
        /// </summary>
        float wf { get; }

        double x { get; set; }

        double y { get; set; }

        double z { get; set; }

        double w { get; set; }

        /// <summary>
        /// Return the yaw/heading of the Quaternion (flight dynamics style). Value 0 - 2pi Radians or 0 to 360 if converted to degrees (North turning clockwise to a complete revolution)
        /// </summary>
        double yaw { get; }

        /// <summary>
        /// Return the pitch/attitude of the Quaternion (flight dynamics style). Value pi/2 through -pi/2 or 90 to -90 if converted to degrees (90 is nose in the air, 0 is level, -90 is nose to the ground)
        /// </summary>
        double pitch { get; }

        /// <summary>
        /// Return the roll/bank of the Quaternion (flight dynamics style). Value range unknown, but should always be 0 really (give or take floating point errors)
        /// </summary>
        double roll { get; }

        /// <summary>
        /// Return the Magnitude of the Quaternion
        /// </summary>
        double magnitude { get; }

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
        void update(double x, double y, double z, double w);

        /// <summary>
        /// Return the Conjugate (Spacial Inverse) of the Quaternion
        /// </summary>
        /// <returns>
        /// </returns>
        IQuaternion Conjugate();

        /// <summary>
        /// Returns the Hamilton Product of two Quaternions
        /// </summary>
        /// <param name="vRight">
        /// Other Quaternion
        /// </param>
        /// <returns>
        /// </returns>
        IQuaternion Hamilton(IQuaternion vRight);

        /// <summary>
        /// Return a Normalized Quaternion
        /// </summary>
        /// <returns>
        /// </returns>
        IQuaternion Normalize();

        /// <summary>
        /// Fill this info in
        /// </summary>
        /// <param name="vDirection">
        /// </param>
        /// <returns>
        /// Fill this info in
        /// </returns>
        IQuaternion GenerateRotationFromDirectionVector(IVector3 vDirection);

        /// <summary>
        /// Return a Vector rotated around the Quaternion
        /// Note: Only works for Unit Quaternions at present due to lazyness (AO-provided Quaternions are all Unit Quaternions)
        /// </summary>
        /// <param name="v1">
        /// Vector
        /// </param>
        /// <returns>
        /// </returns>
        IVector3 RotateVector3(IVector3 v1);

        /// <summary>
        /// Return a Vector representation of a Quaternion (w is dropped)
        /// </summary>
        /// <returns>
        /// </returns>
        IVector3 VectorRepresentation();
    }
}