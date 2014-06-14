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

namespace ZoneEngine.Script
{
    #region Usings ...

    using ZoneEngine.Core.KnuBot;

    #region Usings ...

    using CellAO.Core.Network;

    using ZoneEngine.Core.MessageHandlers;

    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using CellAO.Core.Entities;
    using CellAO.Enums;

    using Microsoft.CSharp;

    using SmokeLounge.AOtomation.Messaging.GameData;

    using Utility;

    using ZoneEngine.ChatCommands;

    #endregion

    #endregion

    #region Class ScriptCompiler

    /// <summary>
    /// Controls Compilation and loading
    /// of *.cs files contained in the
    /// parent and subdirectories
    /// of the "Scripts\\" Directory.
    /// </summary>
    public class ScriptCompiler : IDisposable
    {
        // Holder for Chat commands

        public static ScriptCompiler Instance = new ScriptCompiler();

        #region Fields

        /// <summary>
        /// </summary>
        private readonly Dictionary<string, Type> chatCommands = new Dictionary<string, Type>();

        /// <summary>
        /// Our CSharp compiler object
        /// </summary>
        private readonly CodeDomProvider compiler =
            new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v4.0" } });

        /// <summary>
        /// </summary>
        private readonly List<Assembly> multipleDllList = new List<Assembly>();

        private bool disposed = false;

        /// <summary>
        /// Our compiler parameter command line to pass 
        /// when we compile the scripts.
        /// </summary>
        private readonly CompilerParameters p = new CompilerParameters
                                                {
                                                    GenerateInMemory = false,
                                                    GenerateExecutable = false,
                                                    IncludeDebugInformation = true,
                                                    OutputAssembly = "Scripts.dll",
                                                    TreatWarningsAsErrors = false,
                                                    WarningLevel = 3,
                                                    CompilerOptions = "/optimize"
                                                };

        /// <summary>
        /// </summary>
        private readonly Dictionary<string, Type> scriptList = new Dictionary<string, Type>();

        public List<string> ChatCommands
        {
            get
            {
                return this.chatCommands.Keys.ToList();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        private string[] ScriptsList { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Turn our script names into dll names.
        /// </summary>
        /// <param name="scriptName">
        /// The script name.
        /// </param>
        /// <returns>
        /// The dll name.
        /// </returns>
        public static string DllName(string scriptName)
        {
            scriptName = RemoveCharactersAfterChar(scriptName, '.');
            scriptName = RemoveCharactersBeforeChar(scriptName, '\\');
            scriptName = RemoveCharactersBeforeChar(scriptName, '/');

            return scriptName + ".dll";
        }

        /// <summary>
        /// Logs messages to the console.
        /// </summary>
        /// <param name="owner">
        /// Who or what to log as.
        /// </param>
        /// <param name="ownerColor">
        /// The color of the text the owner shows.
        /// </param>
        /// <param name="message">
        /// The message to display.
        /// </param>
        /// <param name="messageColor">
        /// The color to display the message in.
        /// </param>
        public static void LogScriptAction(
            string owner,
            ConsoleColor ownerColor,
            string message,
            ConsoleColor messageColor)
        {
            Colouring.Push(ownerColor);
            Console.Write(owner + " ");
            Colouring.Pop();

            Colouring.Push(messageColor);
            Console.Write(message + "\n");
            Colouring.Pop();
        }

        /// <summary>
        /// Removes all text from a string
        /// after char chars
        /// </summary>
        /// <param name="hayStack">
        /// The string to trim.
        /// </param>
        /// <param name="needle">
        /// The char to remove all text after EX: '.'
        /// </param>
        /// <returns>
        /// The corrected string.
        /// </returns>
        public static string RemoveCharactersAfterChar(string hayStack, char needle)
        {
            string input = hayStack;
            int index = input.IndexOf(needle);
            if (index > 0)
            {
                input = input.Substring(0, index);
            }

            return input;
        }

        /// <summary>
        /// Remove all text in a string before
        /// the first chars it finds.
        /// If chars is '\\' then 
        /// Debug\\Scripts turns into Scripts
        /// </summary>
        /// <param name="hayStack">
        /// The string to trim the front of.
        /// </param>
        /// <param name="needle">
        /// The first text char in the string 
        /// that matches this, and everything before it will be removed.
        /// </param>
        /// <returns>
        /// The corrected string.
        /// </returns>
        public static string RemoveCharactersBeforeChar(string hayStack, char needle)
        {
            string input = hayStack;
            int index = input.IndexOf(needle);
            if (index >= 0)
            {
                return input.Substring(index + 1);
            }

            // Hmm if we got here then it has no .'s in it so just return input
            return input;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int AddScriptMembers()
        {
            this.scriptList.Clear();
            foreach (Assembly assembly in this.multipleDllList)
            {
                foreach (Type t in assembly.GetTypes())
                {
                    // returns all public types in the asembly
                    foreach (Type iface in t.GetInterfaces())
                    {
                        if (iface.FullName == typeof(IAOScript).FullName)
                        {
                            if (t.Name != "IAOScript")
                            {
                                foreach (MemberInfo mi in t.GetMembers())
                                {
                                    if ((mi.Name == "GetType") || (mi.Name == ".ctor") || (mi.Name == "GetHashCode")
                                        || (mi.Name == "ToString") || (mi.Name == "Equals"))
                                    {
                                        continue;
                                    }

                                    if (mi.MemberType == MemberTypes.Method)
                                    {
                                        if (!this.scriptList.ContainsKey(t.Namespace + "." + t.Name + ":" + mi.Name))
                                        {
                                            this.scriptList.Add(t.Namespace + "." + t.Name + ":" + mi.Name, t);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            this.chatCommands.Clear();
            Assembly wholeassembly = Assembly.GetExecutingAssembly();
            int chatCommands = 0;
            foreach (Type t in wholeassembly.GetTypes().Where(x => x.IsSubclassOf(typeof(AOChatCommand))))
            {
                chatCommands++;
                AOChatCommand aoc = (AOChatCommand)wholeassembly.CreateInstance(t.FullName);
                List<string> acceptedcommands = aoc.ListCommands();
                foreach (string command in acceptedcommands)
                {
                    this.chatCommands.Add(t.FullName + ":" + command, t);
                }
            }

            return chatCommands;
        }

        public string ScriptExists(string scriptname)
        {
            string result = "";
            foreach (string name in this.scriptList.Keys)
            {
                if (name.Substring(name.IndexOf(":", StringComparison.Ordinal) + 1).ToLower() == scriptname.ToLower())
                {
                    result = name.Substring(name.IndexOf(":", StringComparison.Ordinal) + 1);
                    break;
                }
            }
            return result;
        }

        public string ClassExists(string scriptname)
        {
            string result = "";

            foreach (Assembly asm in this.multipleDllList)
            {
                Type tp =
                    asm.GetTypes()
                        .FirstOrDefault(
                            x => (x.BaseType == typeof(BaseKnuBot)) && (x.Name.ToLower() == scriptname.ToLower()));
                if (tp != null)
                {
                    result = tp.Name;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="commandName">
        /// </param>
        /// <param name="client">
        /// </param>
        /// <param name="target">
        /// </param>
        /// <param name="commandArguments">
        /// </param>
        public void CallChatCommand(string commandName, IZoneClient client, Identity target, string[] commandArguments)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            if (commandName.ToUpperInvariant() != "LISTCOMMANDS")
            {
                foreach (KeyValuePair<string, Type> kv in this.chatCommands)
                {
                    if (kv.Key.Substring(kv.Key.IndexOf(":", StringComparison.Ordinal) + 1).ToUpperInvariant()
                        == commandName.ToUpperInvariant())
                    {
                        AOChatCommand aoc =
                            (AOChatCommand)
                                assembly.CreateInstance(
                                    kv.Key.Substring(0, kv.Key.IndexOf(":", StringComparison.Ordinal)));
                        if (aoc != null)
                        {
                            // Check GM Level bitwise
                            if ((client.Controller.Character.Stats[StatIds.gmlevel].Value < aoc.GMLevelNeeded())
                                && (aoc.GMLevelNeeded() > 0))
                            {
                                client.Controller.Character.Playfield.Publish(
                                    ChatTextMessageHandler.Default.CreateIM(
                                        client.Controller.Character,
                                        "You are not authorized to use this command!. This incident will be recorded."));

                                // It is not yet :)
                                return;
                            }

                            // Check if only one argument has been passed for "help"
                            if (commandArguments.Length == 2)
                            {
                                if (commandArguments[1].ToUpperInvariant() == "HELP")
                                {
                                    aoc.CommandHelp(client.Controller.Character);
                                    return;
                                }
                            }

                            // Execute the command with the given command arguments, if CheckCommandArguments is true else print command help
                            if (aoc.CheckCommandArguments(commandArguments))
                            {
                                aoc.ExecuteCommand(client.Controller.Character, target, commandArguments);
                            }
                            else
                            {
                                aoc.CommandHelp(client.Controller.Character);
                            }
                        }
                    }
                }
            }
            else
            {
                client.Controller.Character.Playfield.Publish(
                    ChatTextMessageHandler.Default.CreateIM(client.Controller.Character, "Available Commands:"));
                string[] scriptNames = this.chatCommands.Keys.ToArray();
                for (int i = 0; i < scriptNames.Length; i++)
                {
                    scriptNames[i] = scriptNames[i].Substring(scriptNames[i].IndexOf(":", StringComparison.Ordinal) + 1)
                                     + ":"
                                     + scriptNames[i].Substring(
                                         0,
                                         scriptNames[i].IndexOf(":", StringComparison.Ordinal));
                }

                Array.Sort(scriptNames);

                foreach (string scriptName in scriptNames)
                {
                    string typename = scriptName.Substring(scriptName.IndexOf(":", StringComparison.Ordinal) + 1);
                    AOChatCommand aoc = (AOChatCommand)assembly.CreateInstance(typename);
                    if (aoc != null)
                    {
                        if (client.Controller.Character.Stats[StatIds.gmlevel].Value >= aoc.GMLevelNeeded())
                        {
                            client.Controller.Character.Playfield.Publish(
                                ChatTextMessageHandler.Default.CreateIM(
                                    client.Controller.Character,
                                    scriptName.Substring(0, scriptName.IndexOf(":", StringComparison.Ordinal))));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="functionName">
        /// </param>
        /// <param name="character">
        /// </param>
        public void CallMethod(string functionName, ICharacter character)
        {
            foreach (Assembly assembly in this.multipleDllList)
            {
                foreach (KeyValuePair<string, Type> kv in this.scriptList)
                {
                    if (kv.Key.Substring(kv.Key.IndexOf(":", StringComparison.Ordinal)) == ":" + functionName)
                    {
                        IAOScript aoScript =
                            (IAOScript)
                                assembly.CreateInstance(
                                    kv.Key.Substring(0, kv.Key.IndexOf(":", StringComparison.Ordinal)));
                        if (aoScript != null)
                        {
                            kv.Value.InvokeMember(
                                functionName,
                                BindingFlags.Default | BindingFlags.InvokeMethod,
                                null,
                                aoScript,
                                new object[] { character },
                                CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
        }

        public BaseKnuBot CreateKnuBot(string knuBotName, Identity mobId)
        {
            foreach (Assembly assembly in this.multipleDllList)
            {
                Type knubotType =
                    assembly.GetTypes()
                        .FirstOrDefault(x => (x.BaseType == typeof(BaseKnuBot)) && (x.Name == knuBotName));
                if (knubotType != null)
                {
                    BaseKnuBot bk = (BaseKnuBot)Activator.CreateInstance(knubotType, new object[] { mobId });

                    return bk;
                }
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="multipleFiles">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Compile(bool multipleFiles)
        {
            if (!this.LoadFiles())
            {
                return false;
            }

            // Add all loaded assemblies to the Referenced assemblies list
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies().Where(x=>x.IsDynamic==false))
            {
                this.p.ReferencedAssemblies.Add(assembly.Location);
            }

            if (multipleFiles)
            {
                LogScriptAction(
                    "ScriptCompiler:",
                    ConsoleColor.Yellow,
                    "multiple scripts configuration active.",
                    ConsoleColor.Magenta);
                foreach (string scriptFile in this.ScriptsList)
                {
                    this.p.OutputAssembly = string.Format(
                        CultureInfo.CurrentCulture,
                        Path.Combine("tmp", DllName(scriptFile)));

                    // CreateIM the directory if it doesnt exist
                    FileInfo file = new FileInfo(Path.Combine("tmp", DllName(scriptFile)));
                    if (file.Directory != null)
                    {
                        file.Directory.Create();
                    }

                    // Now compile the dll's
                    CompilerResults results = this.compiler.CompileAssemblyFromFile(this.p, scriptFile);

                    // And check for errors
                    if (ErrorReporting(results).Length != 0)
                    {
                        // We have errors, display them
                        LogScriptAction("Error:", ConsoleColor.Yellow, ErrorReporting(results), ConsoleColor.Red);
                        return false;
                    }

                    LogScriptAction(
                        "Script " + scriptFile,
                        ConsoleColor.Green,
                        "Compiled to: " + this.p.OutputAssembly,
                        ConsoleColor.Green);

                    // Add the compiled assembly to our list
                    this.multipleDllList.Add(Assembly.LoadFile(file.FullName));
                }

                // Ok all good, load em
                foreach (Assembly a in this.multipleDllList)
                {
                    RunScript(a);
                }
            }
            else
            {
                // Compile the full Scripts.dll
                CompilerResults results = this.compiler.CompileAssemblyFromFile(this.p, this.ScriptsList);

                // And check for errors
                if (ErrorReporting(results).Length != 0)
                {
                    // We have errors, display them
                    LogScriptAction("Error:", ConsoleColor.Yellow, ErrorReporting(results), ConsoleColor.Red);
                    return false;
                }

                // Load the full dll
                try
                {
                    FileInfo file = new FileInfo("Scripts.dll");
                    Assembly asm = Assembly.LoadFile(file.FullName);
                    this.multipleDllList.Add(asm);
                    RunScript(asm);
                }
                catch (FileLoadException ee)
                {
                    LogScriptAction(
                        "ERROR",
                        ConsoleColor.Red,
                        "File loading not successful:\r\n" + ee,
                        ConsoleColor.Red);
                    return false;
                }
                catch (FileNotFoundException ee)
                {
                    LogScriptAction("ERROR", ConsoleColor.Red, "Script not found:\r\n" + ee, ConsoleColor.Red);
                    return false;
                }
                catch (BadImageFormatException ee)
                {
                    LogScriptAction("ERROR", ConsoleColor.Red, "Bad image format:\r\n" + ee, ConsoleColor.Red);
                    return false;
                }

                this.AddScriptMembers();
            }

            return true;
        }

        private bool TryResolve(CompilerError e, string scriptFile)
        {
            bool resolved = false;
            string line = this.GetLineOfFile(scriptFile, e.Line).Replace("using", "").Replace(";", "").Trim();
            bool runLoop = true;
            while (runLoop)
            {
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, line + ".dll")))
                {
                    this.p.ReferencedAssemblies.Add(
                        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, line + ".dll"));
                    resolved = true;
                    break;
                }

                runLoop = line.IndexOf(".") > -1;
                if (line.IndexOf(".") > -1)
                {
                    line = line.Substring(0, line.LastIndexOf("."));
                }
            }
            return resolved;
        }

        private string GetLineOfFile(string scriptFile, int p)
        {
            string res = "";
            using (TextReader sr = new StreamReader(scriptFile))
            {
                while (p > 0)
                {
                    res = sr.ReadLine();
                    p--;
                    if ((p==0) && (string.IsNullOrWhiteSpace(res)))
                    {
                        res=sr.ReadLine();
                    }
                }
            }
            return res;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="disposing">
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.disposed)
                {
                    this.compiler.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Our Error reporting method.
        /// </summary>
        /// <param name="results">
        /// </param>
        /// <returns>
        /// </returns>
        private static string ErrorReporting(CompilerResults results)
        {
            StringBuilder report = new StringBuilder();
            if (results.Errors.HasErrors)
            {
                // Count the errors and return them

                int count = results.Errors.Count;
                for (int i = 0; i < count; i++)
                {
                    report.Append(results.Errors[i].FileName);
                    report.AppendLine(
                        " In Line: " + results.Errors[i].Line + " Error: " + results.Errors[i].ErrorNumber + " "
                        + results.Errors[i].ErrorText);
                }
            }

            return report.ToString();
        }

        /// <summary>
        /// Loads all classes contained in our
        /// Assembly file that publically inherit
        /// our IAOScript class.
        /// Entry point for each script is public void Main(string[] args){}
        /// </summary>
        /// <param name="script">
        /// Our .NET dll or exe file.
        /// </param>
        private static void RunScript(Assembly script)
        {
            // Now that we have a compiled script, lets run them
            foreach (Type type in script.GetExportedTypes())
            {
                // returns all public types in the asembly
                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface.FullName == typeof(IAOScript).FullName)
                    {
                        // yay, we found a script interface, lets create it and run it!
                        // Get the constructor for the current type
                        // you can also specify what creation parameter types you want to pass to it,
                        // so you could possibly pass in data it might need, or a class that it can use to query the host application
                        ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                        if (constructor != null && constructor.IsPublic)
                        {
                            // lets be friendly and only do things legitimitely by only using valid constructors

                            // we specified that we wanted a constructor that doesn't take parameters, so don't pass parameters
                            IAOScript scriptObject = constructor.Invoke(null) as IAOScript;
                            if (scriptObject != null)
                            {
                                LogScriptAction(
                                    "Script",
                                    ConsoleColor.Green,
                                    scriptObject.GetType().Name + " Loaded.",
                                    ConsoleColor.Green);

                                // Lets run our script and display its results
                                scriptObject.Main(null);
                            }
                            else
                            {
                                // hmmm, for some reason it didn't create the object
                                // this shouldn't happen, as we have been doing checks all along, but we should
                                // inform the user something bad has happened, and possibly request them to send
                                // you the script so you can debug this problem
                                LogScriptAction("Error!", ConsoleColor.Red, "Script not loaded.", ConsoleColor.Red);
                            }
                        }
                        else
                        {
                            // and even more friendly and explain that there was no valid constructor
                            // found and thats why this script object wasn't run
                            LogScriptAction("Error!", ConsoleColor.Red, "No valid constructor found.", ConsoleColor.Red);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If the Scripts directory is empty
        /// or the Scripts directory is missing
        /// Give the correct error.
        /// </summary>
        /// <returns>true if the Scripts directory exsits, and there is at least one script in it.</returns>
        private bool LoadFiles()
        {
            // Seperated like this, because i want to display different custom errors.
            try
            {
                this.ScriptsList = Directory.GetFiles("Scripts", "*.cs", SearchOption.AllDirectories);
            }
            catch (DirectoryNotFoundException)
            {
                LogScriptAction("Error", ConsoleColor.Red, "Scripts directory does not exist!", ConsoleColor.Red);
                return false;
            }
            catch (PathTooLongException)
            {
                LogScriptAction("Error", ConsoleColor.Red, "Path name is too long", ConsoleColor.Red);
                return false;
            }
            catch (ArgumentException)
            {
                LogScriptAction("Error", ConsoleColor.Red, "Path is zero length or has invalid chars", ConsoleColor.Red);
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                LogScriptAction(
                    "Error",
                    ConsoleColor.Red,
                    "You don't have permission to access this directory",
                    ConsoleColor.Red);
                return false;
            }
            catch (IOException)
            {
                LogScriptAction(
                    "Error",
                    ConsoleColor.Red,
                    "I/O Error occured. (Path is filename or network error)",
                    ConsoleColor.Red);
                return false;
            }

            if (this.ScriptsList.Length == 0)
            {
                LogScriptAction(
                    "Error:",
                    ConsoleColor.Red,
                    "Scripts directory contains no scripts!",
                    ConsoleColor.Yellow);
                return false;
            }

            return true;
        }

        #endregion
    }

    #endregion Class ScriptCompiler
}