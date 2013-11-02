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
// Last modified: 2013-11-03 00:30

#endregion


#pragma warning disable 0162 // Suppresses "Unreachable code detected" error when disabling login checking

namespace LoginEngine
{
    #region Usings ...

    using System;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;

    using CellAO.Database.Dao;

    using Utility;
    using Utility.Config;

    #endregion

    /// <summary>
    /// Anarchy Online Encrypted Login Validator
    /// </summary>
    public class LoginEncryption
    {
        #region Fields

        /// <summary>
        /// 
        /// </summary>
        public bool i_Enable = Convert.ToBoolean(ConfigReadWrite.Instance.CurrentConfig.UsePassword);

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Decrypts Login Key from Client
        /// </summary>
        /// <param name="LoginKey">
        /// Login Key from Client
        /// </param>
        /// <param name="UserName">
        /// Username stored in Login Key
        /// </param>
        /// <param name="ServerSalt">
        /// Server Salt stored in Login Key
        /// </param>
        /// <param name="Password">
        /// Password stored in Login Key
        /// </param>
        public void DecryptLoginKey(string LoginKey, out string UserName, out string ServerSalt, out string Password)
        {
            string[] LoginKeySplit = LoginKey.Split('-');

            BigInteger ClientPublicKey = new BigInteger(LoginKeySplit[0], 16);
            string EncryptedBlock = LoginKeySplit[1];

            // These should really be in a config file, but for now hardcoded
            BigInteger ServerPrivateKey =
                new BigInteger("7ad852c6494f664e8df21446285ecd6f400cf20e1d872ee96136d7744887424b", 16);
            BigInteger Prime =
                new BigInteger(
                    "eca2e8c85d863dcdc26a429a71a9815ad052f6139669dd659f98ae159d313d13c6bf2838e10a69b6478b64a24bd054ba8248e8fa778703b418408249440b2c1edd28853e240d8a7e49540b76d120d3b1ad2878b1b99490eb4a2a5e84caa8a91cecbdb1aa7c816e8be343246f80c637abc653b893fd91686cf8d32d6cfe5f2a6f", 
                    16);

            string TeaKey = ClientPublicKey.modPow(ServerPrivateKey, Prime).ToString(16).ToLower();

            if (TeaKey.Length < 32)
            {
                // If TeaKey is not at least 128bits, pad to the left with 0x00
                TeaKey.PadLeft(32, '0');
            }
            else
            {
                // If TeaKey is more than 128bits, truncate
                TeaKey = TeaKey.Substring(0, 32);
            }

            string DecryptedBlock = this.DecryptTea(EncryptedBlock, TeaKey);

            DecryptedBlock = DecryptedBlock.Substring(8); // Strip first 8 bytes of padding

            int DataLength = this.ConvertStringToIntSwapEndian(DecryptedBlock.Substring(0, 4));

            DecryptedBlock = DecryptedBlock.Substring(4);

            string[] BlockParts = DecryptedBlock.Split(new[] { '|' }, 2);

            UserName = BlockParts[0];

            ServerSalt = string.Empty;

            for (int i = 0; i < 32; i += 4)
            {
                ServerSalt += string.Format("{0:x8}", this.ConvertStringToIntSwapEndian(BlockParts[1].Substring(i, 4)));
            }

            Password = BlockParts[1].Substring(33, DataLength - 34 - UserName.Length);
        }

        /// <summary>
        /// Generates a salted password hash for use in the DB
        /// </summary>
        /// <param name="clearPassword">
        /// Cleartext password to hash
        /// </param>
        /// <returns>
        /// </returns>
        public string GeneratePasswordHash(string clearPassword)
        {
            byte[] Salt = new byte[2];
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

            rand.GetBytes(Salt);

            return this.GeneratePasswordHash(clearPassword, Salt);
        }

        /// <summary>
        /// Check if a certain character is on the clients authenticated account
        /// </summary>
        /// <param name="UserName">
        /// Client Username
        /// </param>
        /// <param name="CharacterID">
        /// Client CharacterId
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsCharacterOnAccount(string UserName, uint CharacterID)
        {
            return CharacterDao.IsCharacterOnAccount(UserName, CharacterID);
        }

        /// <summary>
        /// Verifies if given login is valid
        /// </summary>
        /// <param name="LoginKey">
        /// Login Key from Client
        /// </param>
        /// <param name="ServerSalt">
        /// Authentication Salt server sent the Client
        /// </param>
        /// <param name="UserName">
        /// Username to be Validated
        /// </param>
        /// <param name="passwordHash">
        /// Stored password associated with UserName
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsValidLogin(string LoginKey, string ServerSalt, string UserName, string passwordHash)
        {
            string ClientUserName;
            string ClientPassword;
            string ClientServerSalt;

            if (this.i_Enable == false)
            {
                return true;
            }

            try
            {
                this.DecryptLoginKey(LoginKey, out ClientUserName, out ClientServerSalt, out ClientPassword);
            }
            catch
            {
                Console.WriteLine("Invalid password...");
                return false;
            }

            if (ClientUserName != UserName)
            {
                return false;
            }

            if (!this.IsValidPasswordHash(ClientPassword, passwordHash))
            {
                return false;
            }

            if (ClientServerSalt != ServerSalt)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifies if given login key is valid
        /// </summary>
        /// <param name="LoginKey">
        /// Login Key from Client
        /// </param>
        /// <param name="ServerSalt">
        /// Authentication Salt server sent the Client
        /// </param>
        /// <param name="UserName">
        /// Username to be Validated
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsValidLogin(string LoginKey, string ServerSalt, string UserName)
        {
            if (this.i_Enable == false)
            {
                return true;
            }

            string passwordHash = this.GetLoginPassword(UserName);
            return this.IsValidLogin(LoginKey, ServerSalt, UserName, passwordHash);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="HexKey">
        /// </param>
        /// <returns>
        /// </returns>
        private uint[] ConvertHexKeyToUInts(string HexKey)
        {
            uint[] output = new uint[4];
            int i;

            for (i = 0; i < 4; i++)
            {
                output[i] = this.ConvertHexToUInt(HexKey.Substring(i * 8, 8));
            }

            return output;
        }

        // Converts first 32-bits of Input hex string to a 32-bit int
        /// <summary>
        /// </summary>
        /// <param name="HexInput">
        /// </param>
        /// <returns>
        /// </returns>
        private uint ConvertHexToUInt(string HexInput)
        {
            return (uint)IPAddress.NetworkToHostOrder(Convert.ToInt32(HexInput.Substring(0, 8), 16));
        }

        // Converts first 32-bits of Input string to a 32-bit int and Swap Endian
        /// <summary>
        /// </summary>
        /// <param name="Input">
        /// </param>
        /// <returns>
        /// </returns>
        private int ConvertStringToIntSwapEndian(string Input)
        {
            int output;
            output = Input[3];
            output += Input[2] << 8;
            output += Input[1] << 16;
            output += Input[0] << 24;
            return output;
        }

        /// <summary>
        /// </summary>
        /// <param name="Input">
        /// </param>
        /// <returns>
        /// </returns>
        private string ConvertUIntToString(uint Input)
        {
            StringBuilder output = new StringBuilder();
            output.Append((char)(Input & 0xFF));
            output.Append((char)((Input >> 8) & 0xFF));
            output.Append((char)((Input >> 16) & 0xFF));
            output.Append((char)((Input >> 24) & 0xFF));
            return output.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="EncryptedData">
        /// </param>
        /// <param name="Key">
        /// </param>
        /// <returns>
        /// </returns>
        private string DecryptTea(string EncryptedData, string Key)
        {
            uint[] KeyInt = this.ConvertHexKeyToUInts(Key);
            uint[] DataBlock = new uint[2];
            uint[] Prev = new uint[2];
            uint[] NewPrev = new uint[2];
            string DecryptedData = string.Empty;
            int i;

            Prev[0] = 0;
            Prev[1] = 0;

            for (i = 0; i < EncryptedData.Length; i += 16)
            {
                NewPrev[0] = DataBlock[0] = this.ConvertHexToUInt(EncryptedData.Substring(i, 8));
                NewPrev[1] = DataBlock[1] = this.ConvertHexToUInt(EncryptedData.Substring(i + 8, 8));

                this.DecryptTeaRound(DataBlock, KeyInt);

                DataBlock[0] = DataBlock[0] ^ Prev[0];
                DataBlock[1] = DataBlock[1] ^ Prev[1];

                DecryptedData += this.ConvertUIntToString(DataBlock[0]) + this.ConvertUIntToString(DataBlock[1]);

                Prev[0] = NewPrev[0];
                Prev[1] = NewPrev[1];
            }

            return DecryptedData;
        }

        // Converts first 128-bits of Input hex string to an array of 32-bit ints

        // Decrypt 32-bit Data Block of TEA using 128-bit Key
        /// <summary>
        /// </summary>
        /// <param name="data">
        /// </param>
        /// <param name="key">
        /// </param>
        private void DecryptTeaRound(uint[] data, uint[] key)
        {
            uint n = 32;
            uint sum = 0xc6ef3720;
            uint delta = 0x9e3779b9;

            while (n-- > 0)
            {
                data[1] -= ((data[0] << 4) + key[2]) ^ (data[0] + sum) ^ ((data[0] >> 5) + key[3]);
                data[0] -= ((data[1] << 4) + key[0]) ^ (data[1] + sum) ^ ((data[1] >> 5) + key[1]);
                sum -= delta;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="clearPassword">
        /// </param>
        /// <param name="Salt">
        /// </param>
        /// <returns>
        /// </returns>
        private string GeneratePasswordHash(string clearPassword, byte[] Salt)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            // Note we use ASCII here not UTF8 for better compatability with PHP/etc
            byte[] clearPasswordBytes = Encoding.ASCII.GetBytes(clearPassword);
            byte[] saltedPasswordBytes = new byte[clearPasswordBytes.Length + Salt.Length];

            Array.Copy(Salt, 0, saltedPasswordBytes, 0, Salt.Length);
            Array.Copy(clearPasswordBytes, 0, saltedPasswordBytes, Salt.Length, clearPasswordBytes.Length);

            byte[] passwordHash = md5.ComputeHash(saltedPasswordBytes);

            return BitConverter.ToString(Salt).ToLower().Replace("-", string.Empty).PadLeft(4, '0') + "$"
                   + BitConverter.ToString(passwordHash).ToLower().Replace("-", string.Empty).PadLeft(32, '0');
        }

        /// <summary>
        /// </summary>
        /// <param name="RecvLogin">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetLoginPassword(string RecvLogin)
        {
            DBLoginData loginPassword = LoginDataDao.GetByUsername(RecvLogin);

            if (loginPassword != null)
            {
                return loginPassword.Password;
            }

            LogUtil.Debug(string.Format("No entry for account username '{0}' found", RecvLogin));
            return string.Empty;
        }

        /// <summary>
        /// </summary>
        /// <param name="clientPassword">
        /// </param>
        /// <param name="passwordHash">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsValidPasswordHash(string clientPassword, string passwordHash)
        {
            if (passwordHash == string.Empty)
            {
                return false;
            }

            byte[] Salt = new byte[2];

            Salt[0] = Convert.ToByte(passwordHash.Substring(0, 2), 16);
            Salt[1] = Convert.ToByte(passwordHash.Substring(2, 2), 16);

            string clientHash = this.GeneratePasswordHash(clientPassword, Salt);

            if (clientHash != passwordHash)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}