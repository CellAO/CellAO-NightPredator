using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using CellAO.Database;
using IrcDotNet;

namespace ChatEngine.Relay
{
    using CellAO.Database.Dao;

    public class CellAoBotUser
    {
        public CellAoBotUser(IrcUser ircUser)
        {
            Debug.Assert(ircUser != null);
            this.IrcUser = ircUser;
        }


        public bool IsAuthenticated
        {
            get;
            private set;
        }

        //public CellAoUser CellAoUser
        //{
        //    get;
        //    private set;
        //}

        public IrcUser IrcUser
        {
            get;
            private set;
        }

               public bool LogIn(string username, string password)
        {
            try
            {
                LoginDataDao.GetByUsername(username);



                // Log in to Twitter service using xAuth authentication.
                //var token = this.service.GetAccessTokenWithXAuth(username, password);
               // this.service.AuthenticateWith(token.Token, token.TokenSecret);
                //var user = this.service.GetUserProfile();
               // if (user.Name == null)
                    return false;
                
               // this.TwitterUser = user;
                this.IsAuthenticated = true;

                return true;
            }
            catch (WebException exWeb)
            {
                var httpResponse = exWeb.Response as HttpWebResponse;
                if (httpResponse != null)
                {
                    if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                        return false;
                }
                throw;
            }
        }

        public void LogOut()
        {
            // Log out of Twitter service.
            //this.service.EndSession();
            //this.TwitterUser = null;
            this.IsAuthenticated = false;
        }
    }
}
