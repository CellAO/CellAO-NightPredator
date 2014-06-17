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

    class WebSiteBase
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

        public WebSiteBase()
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
            //section.Add(stringVariableContainingContent);
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
                cssLinks = "<link href='" + filename + "' type='text/css' rel='stylesheet' />";
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
