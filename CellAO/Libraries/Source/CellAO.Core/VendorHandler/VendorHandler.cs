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

namespace CellAO.Core.VendorHandler
{
    #region Usings ...

    using System.Collections.Generic;
    using System.Linq;
    using CellAO.Core.Entities;
    using CellAO.Core.Playfields;
    using CellAO.Core.Statels;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.ObjectManager;
    using SmokeLounge.AOtomation.Messaging.GameData;
    using Utility;
    using Quaternion = CellAO.Core.Vector.Quaternion;

    #endregion

    public static class VendorHandler
    {
        public static void SpawnVendorFromDatabaseTemplate(DBVendor vendor, IPlayfield playfield)
        {
            Identity pfIdentity = new Identity()
            {
                Type = IdentityType.Playfield,
                Instance = vendor.Playfield
            };

            Identity freeIdentity = new Identity()
            {
                Type = IdentityType.VendingMachine,
                Instance = Pool.Instance.GetFreeInstance<Vendor>(0x70000000, IdentityType.VendingMachine)
            };

            Vendor v = new Vendor(pfIdentity, freeIdentity, vendor.Hash)
            {
                RawCoordinates = new Vector3(vendor.X, vendor.Y, vendor.Z),
                Heading = new Quaternion(vendor.HeadingX, vendor.HeadingY, vendor.HeadingZ, vendor.HeadingW),
                Playfield = playfield
            };
        }

        public static void SpawnEmptyVendorFromTemplate(StatelData statelData, IPlayfield playfield, int instance)
        {
            Identity pfIdentity = new Identity() { Type = IdentityType.Playfield, Instance = statelData.PlayfieldId };
            Identity freeIdentity = new Identity()
            {
                Type = IdentityType.VendingMachine,
                Instance = Pool.Instance.GetFreeInstance<Vendor>(0x70000000, IdentityType.VendingMachine)
            };

            Vendor v = new Vendor(pfIdentity, freeIdentity, statelData.TemplateId)
            {
                OriginalIdentity = statelData.Identity,
                RawCoordinates = new Vector3(statelData.X, statelData.Y, statelData.Z),
                Heading = new Quaternion(statelData.HeadingX, statelData.HeadingY, statelData.HeadingZ, statelData.HeadingW),
                Playfield = playfield
            };
        }

        public static void SpawnVendorsForPlayfield(IPlayfield playfield, StatelData[] rdbVendors)
        {
            IEnumerable<DBVendor> vendors = VendorDao.Instance.GetWhere(new { Playfield = playfield.Identity.Instance });

            foreach (StatelData sd in rdbVendors)
            {
                int id = (((sd.Identity.Instance) >> 16) & 0xff | (playfield.Identity.Instance << 16));

                DBVendor vendor = vendors.FirstOrDefault(x => x.Id == id);
                if (vendor is null)
                {
                    LogUtil.Debug(DebugInfoDetail.Statel, sd.Identity.ToString() + " -    " + sd.TemplateId);
                    SpawnEmptyVendorFromTemplate(sd, playfield, id);
                }
                else
                {
                    LogUtil.Debug(DebugInfoDetail.Statel, sd.Identity.ToString() + " - DB " + vendor.TemplateId);
                    SpawnVendorFromDatabaseTemplate(vendor, playfield);
                }
            }
        }
    }
}