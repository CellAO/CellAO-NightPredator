namespace WebEngine.Websites
{
    #region Usings ...

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using CellAO.Database.Dao;
    using CellAO.Database.Entities;

    using _config = Utility.Config.ConfigReadWrite;

    #endregion

    class IndexPHP
    {
        #region Fields

        private string cssContent = string.Empty;
        public string CssContent
        {
            get { return cssContent; }
        }

        private string destinationFilename = string.Empty;
        public string DestinationFilename
        {
            get { return destinationFilename; }
            set { this.destinationFilename = value; }
        }

        private string mergedFile = string.Empty;
        public string MergedFile
        {
            get { return mergedFile; }
        }

        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { this.title = value; }
        }

        private List<string> section = new List<string>();

        #endregion

        #region Constructors

        public IndexPHP()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create the content here
        /// </summary>
        /// <param name="">
        /// </param>
        /// <returns>
        /// </returns>
        public void CreateContent()
        {
            // Some variables
            string sectionHeader = string.Empty;
            string sectionCharactersList = string.Empty;
            string sectionContact = string.Empty;
            string sectionRegistrationLink = string.Empty;
            string sectionInfoTab = string.Empty;
            string sectionRegisteredUsers = string.Empty;
            List<DBCharacter> loggedIn;
            long registeredCount;

            // Get database data - example: logged in characters, number of registered users etc.
            loggedIn = CharacterDao.Instance.GetLoggedInCharacters();
            registeredCount = LoginDataDao.Instance.GetRegisteredCount();


            // Define contents of header section
            sectionHeader = "<div class='headerwrapper'><h1>Welcome to " + _config.Instance.CurrentConfig.WebHostName + "</h1></div>";

            // Define contents of character info section
            sectionCharactersList = "<div class='characterList'>";
            sectionCharactersList += "<table><tr class='italic'>";
            sectionCharactersList += "<th>Name</th><th>Level</th><th>Playfield</th><th>Status</th>";
            sectionCharactersList += "</tr>";

            foreach (DBCharacter character in loggedIn)
            {
                sectionCharactersList += "<tr><td>" + character.Name + "</td><td>lvl</td><td>" + character.Playfield + "</td><td><font color='green'>online</font></td></tr>";
            }

            sectionCharactersList += "</table></div>";

            // Define contents of contact section
            sectionContact = "<div class='dataWrapper'><div class='dataBlock'><h2>Contact</h2>";
            sectionContact += "<a class='italic' href='contact.php'>Hit us up!</a></div>";


            // Define contents of registration formular
            sectionRegistrationLink = "<div class='dataBlock'><h2>Login and Register</h2>";
            sectionRegistrationLink += "<a class='italic' href='register.php'>Register now!</a></div></div>";

            // Define contents of infotab to display some stuff
            sectionInfoTab = "<div class='dataWrapper'><div class='dataBlock'><h3>Known Bugs/Missing Features</h3><p>";
            sectionInfoTab += "<ul class='italic'><li>Level doesn't get displayed</li><li>No feedback formulars yet (successfull, failed etc.)</li></ul></p></div>";

            // Define contents of total registered user count
            sectionRegisteredUsers = "<div class='dataBlock'><h2>Total registered Users</h2>" + "<p class='italic'>" + registeredCount + "</p></div></div>";


            section.Add(sectionHeader);
            section.Add(sectionCharactersList);
            section.Add(sectionContact);
            section.Add(sectionRegistrationLink);
            section.Add(sectionInfoTab);
            section.Add(sectionRegisteredUsers);
        }

        /// <summary>
        /// Gets the content of css files.
        /// </summary>
        /// <param name="">
        /// </param>
        /// <returns>
        /// </returns>
        private void CreateMergedFile(List<string> sbody)
        {
            string headContent = "<head><title>" + title + "</title>" + cssContent + "</head>";
            string bodyContent = "<body>" + sbody[0] + "<div class='mainwrapper'>";

            // Add each content section
            for (int i = 1; i < sbody.Count; i++)
            {
                bodyContent += sbody[i];
            }
            bodyContent += "</div></body>";

            mergedFile = "<!DOCTYPE html><html>" + headContent + bodyContent + "</html>";
        }

        /// <summary>
        /// Gets the content of css files.
        /// </summary>
        /// <param name="">
        /// </param>
        /// <returns>
        /// </returns>
        public void GetCSSContent(params string[] filename)
        {
            string cssLinks = string.Empty;

            foreach (string fname in filename)
            {
                cssLinks = "<link href='" + fname + "' type='text/css' rel='stylesheet' />";
            }

            cssContent = cssLinks;
        }

        /// <summary>
        /// Merge and write your file
        /// </summary>
        /// <param name="">
        /// </param>
        /// <returns>
        /// </returns>
        public void WriteFile()
        {
            CreateContent();

            CreateMergedFile(section);

            StreamWriter phpFile = new StreamWriter("htdocs/" + destinationFilename);
            phpFile.WriteLine(mergedFile);
            phpFile.Close();
        }

        #endregion
    }
}
