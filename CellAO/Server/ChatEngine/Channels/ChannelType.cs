using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChatEngine.Channels
{
    /// <summary>
    /// ChannelType is the first byte of the Channel Id
    /// </summary>
    public enum ChannelType : byte
    {
        /// <summary>
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// </summary>
        Admin = 1,

        /// <summary>
        /// </summary>
        Team = 2 | 0x80,

        /// <summary>
        /// </summary>
        Organization = 3,

        /// <summary>
        /// </summary>
        Leaders = 4,

        /// <summary>
        /// </summary>
        GM = 5,

        /// <summary>
        /// </summary>
        Shopping = 6 | 0x80,

        /// <summary>
        /// </summary>
        General = 7 | 0x80,

        /// <summary>
        /// </summary>
        Towers = 10,

        /// <summary>
        /// </summary>
        Announcements = 12,

        /// <summary>
        /// </summary>
        Raid = 15 | 0x80,

        /// <summary>
        /// </summary>
        Battlestation = 16 | 0x80
    }
}
