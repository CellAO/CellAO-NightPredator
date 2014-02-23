﻿#region License

// Copyright (c) 2005-2014, CellAO Team
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

namespace CellAO.Database
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Dapper;

    using Utility;

    using Config = Utility.Config.ConfigReadWrite;

    #endregion

    /// <summary>
    /// </summary>
    public static class Misc
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static bool CheckDatabase()
        {
            string applicationFolder = Path.Combine(Directory.GetCurrentDirectory(), "SqlTables");
            string[] files = Directory.GetFiles(applicationFolder, "*.sql", SearchOption.TopDirectoryOnly);

            string errorMessage = string.Empty;
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (errorMessage != string.Empty)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine("Error connecting to database");
                Console.WriteLine(errorMessage);
                Colouring.Pop();
                return false;
            }

            errorMessage = string.Empty;
            string fName = string.Empty;
            List<string> tablesNotFound = new List<string>();

            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    foreach (string sqlFile in files)
                    {
                        if (sqlFile != null)
                        {
                            fName = Path.GetFileNameWithoutExtension(sqlFile).ToLower();
                            if (!Exists(conn, fName))
                            {
                                tablesNotFound.Add(sqlFile);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            if (errorMessage != string.Empty)
            {
                Colouring.Push(ConsoleColor.Red);
                Console.WriteLine("Error checking for table " + fName);
                Console.WriteLine(errorMessage);
                Colouring.Pop();
                return false;
            }

            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    if (tablesNotFound.Count > 0)
                    {
                        Colouring.Push(ConsoleColor.Red);
                        Console.Write("SQL Tables are not complete. Should they be created? (Y/N) ");
                        Colouring.Pop();

                        string answer = Console.ReadLine();
                        string sqlQuery;
                        if (answer.ToLower() == "y")
                        {
                            foreach (string sqlFile in tablesNotFound)
                            {
                                fName = Path.GetFileNameWithoutExtension(sqlFile);
                                long fileSize = new FileInfo(sqlFile).Length;
                                Colouring.Push(ConsoleColor.Green);
                                Console.Write("Table " + fName.PadRight(67) + "[  0%]");
                                Colouring.Pop();
                                if (fileSize > 10000)
                                {
                                    string[] queries = File.ReadAllLines(sqlFile);
                                    int counter = 0;
                                    sqlQuery = string.Empty;
                                    string lastpercent = "0";
                                    while (counter < queries.Length)
                                    {
                                        if (queries[counter].IndexOf("INSERT INTO") == -1)
                                        {
                                            sqlQuery += queries[counter] + "\n";
                                        }
                                        else
                                        {
                                            counter--;
                                            break;
                                        }

                                        counter++;
                                    }
                                    try
                                    {
                                        conn.Execute(sqlQuery);
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine(sqlQuery);
                                        throw;
                                    }

                                    counter++;
                                    string buf1 = string.Empty;
                                    while (counter < queries.Length)
                                    {
                                        if (queries[counter].ToLower().Substring(0, 11) == "insert into")
                                        {
                                            break;
                                        }

                                        counter++;
                                    }

                                    if (counter < queries.Length)
                                    {
                                        buf1 = queries[counter].Substring(
                                            0, 
                                            queries[counter].ToLower().IndexOf("values"));
                                        buf1 = buf1 + "VALUES ";
                                        StringBuilder Buffer = new StringBuilder(0, 1 * 1024 * 1024);
                                        while (counter < queries.Length)
                                        {
                                            if (Buffer.Length == 0)
                                            {
                                                Buffer.Append(buf1);
                                            }

                                            string part = string.Empty;
                                            while (counter < queries.Length)
                                            {
                                                if (queries[counter].Trim() != string.Empty)
                                                {
                                                    part =
                                                        queries[counter].Substring(
                                                            queries[counter].ToLower().IndexOf("values"));
                                                    part = part.Substring(part.IndexOf("(")); // from '(' to end
                                                    part = part.Substring(0, part.Length - 1); // Remove ';'
                                                    if (Buffer.Length + 1 + part.Length > 1024 * 1000)
                                                    {
                                                        Buffer.Remove(Buffer.Length - 2, 2);
                                                        Buffer.Append(";");
                                                        conn.Execute(Buffer.ToString());
                                                        Buffer.Clear();
                                                        Buffer.Append(buf1);
                                                        string lp2 =
                                                            Convert.ToInt32(
                                                                Math.Floor((double)counter / queries.Length * 100))
                                                                .ToString();
                                                        if (lp2 != lastpercent)
                                                        {
                                                            Console.Write(
                                                                "\rTable " + fName.PadRight(67) + "[" + lp2.PadLeft(3)
                                                                + "%]");
                                                            lastpercent = lp2;
                                                        }
                                                    }

                                                    Buffer.Append(part + ", ");
                                                }

                                                counter++;
                                            }

                                            Buffer.Remove(Buffer.Length - 2, 2);
                                            Buffer.Append(";");
                                            conn.Execute(Buffer.ToString());
                                            Buffer.Clear();
                                            string lp =
                                                Convert.ToInt32(Math.Floor((double)counter / queries.Length * 100))
                                                    .ToString();
                                            if (lp != lastpercent)
                                            {
                                                Console.Write(
                                                    "\rTable " + fName.PadRight(67) + "[" + lp.PadLeft(3) + "%]");
                                                lastpercent = lp;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Colouring.Push(ConsoleColor.Green);
                                        Console.Write("\rTable " + fName.PadRight(67) + "[100%]");
                                        Colouring.Pop();
                                    }
                                }
                                else
                                {
                                    sqlQuery = File.ReadAllText(sqlFile);
                                    conn.Execute(sqlQuery);
                                    Colouring.Push(ConsoleColor.Green);
                                    Console.Write("\rTable " + fName.PadRight(67) + "[100%]");
                                    Colouring.Pop();
                                }

                                Console.WriteLine();
                            }
                        }

                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="orgId">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<int> GetOrgMembers(uint orgId)
        {
            return GetOrgMembers(orgId, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="orgId">
        /// </param>
        /// <param name="excludePresident">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<int> GetOrgMembers(uint orgId, bool excludePresident)
        {
            List<int> orgMembers = new List<int>();
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    string pres = string.Empty;

                    if (excludePresident)
                    {
                        pres =
                            " AND `ID` NOT IN (SELECT `ID` FROM `characters_stats` WHERE `Stat` = '48' AND `Value` = '0')";
                    }

                    orgMembers.AddRange(
                        conn.Query<int>(
                            "SELECT `ID` FROM `characters_stats` WHERE `Stat` = '5' AND `Value` = @orgId " + pres, 
                            orgId));
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }

            return orgMembers;
        }

        /// <summary>
        /// </summary>
        public static void LogOffAll()
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute("UPDATE characters set Online=0");
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="characterId">
        /// </param>
        public static void LogOffCharacter(int characterId)
        {
            try
            {
                using (IDbConnection conn = Connector.GetConnection())
                {
                    conn.Execute("UPDATE characters set Online=0 where id=@charid", new { charid = characterId });
                }
            }
            catch (Exception e)
            {
                LogUtil.ErrorException(e);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="conn">
        /// </param>
        /// <param name="fName">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private static bool Exists(IDbConnection conn, string fName)
        {
            switch (Config.Instance.CurrentConfig.SQLType)
            {
                case "MySql":
                    return conn.Query<string>("show tables").Contains(fName);
                case "MsSql":
                    return conn.Query<string>("SELECT table_name FROM INFORMATION_SCHEMA.TABLES").Contains(fName);
                case "PostgreSQL":
                    return conn.Query<string>("SELECT table_name FROM information_schema.tables").Contains(fName);
                default:
                    throw new Exception("Unknown database type encountered. Check your Config.xml or tell the coders");
            }
        }

        #endregion
    }
}