using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    using Config = Utility.Config.ConfigReadWrite;

    #endregion

    /// <summary>
    /// </summary>
    public static class Misc
    {
        /// <summary>
        /// </summary>
        public static void LogOffAll()
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                conn.Execute("UPDATE characters set Online=0");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="characterId">
        /// </param>
        public static void LogOffCharacter(int characterId)
        {
            using (IDbConnection conn = Connector.GetConnection())
            {
                conn.Execute("UPDATE characters set Online=0 where id=@charid", new { charid = characterId });
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static bool CheckDatabase()
        {
            string applicationFolder = Path.Combine(Directory.GetCurrentDirectory(), "SQLTables");
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error connecting to database");
                Console.WriteLine(errorMessage);
                Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error checking for table " + fName);
                Console.WriteLine(errorMessage);
                Console.ResetColor();
                return false;
            }

            using (IDbConnection conn = Connector.GetConnection())
            {
                if (tablesNotFound.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("SQL Tables are not complete. Should they be created? (Y/N) ");

                    string answer = Console.ReadLine();
                    string sqlQuery;
                    if (answer.ToLower() == "y")
                    {
                        foreach (string sqlFile in tablesNotFound)
                        {
                            fName = Path.GetFileNameWithoutExtension(sqlFile);
                            long fileSize = new FileInfo(sqlFile).Length;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("Table " + fName.PadRight(67) + "[  0%]");

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

                                conn.Execute(sqlQuery);

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
                                    buf1 = queries[counter].Substring(0, queries[counter].ToLower().IndexOf("values"));
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
                                                part = queries[counter].Substring(queries[counter].ToLower().IndexOf("values"));
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
                                                        Convert.ToInt32(Math.Floor((double)counter / queries.Length * 100))
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
                                            Convert.ToInt32(Math.Floor((double)counter / queries.Length * 100)).ToString();
                                        if (lp != lastpercent)
                                        {
                                            Console.Write("\rTable " + fName.PadRight(67) + "[" + lp.PadLeft(3) + "%]");
                                            lastpercent = lp;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.Write("\rTable " + fName.PadRight(67) + "[100%]");
                                }
                            }
                            else
                            {
                                sqlQuery = File.ReadAllText(sqlFile);
                                conn.Execute(sqlQuery);
                                Console.Write("\rTable " + fName.PadRight(67) + "[100%]");
                            }

                            Console.WriteLine();
                        }
                    }

                    return true;
                }
            }

            return false;
        }

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
    }
}