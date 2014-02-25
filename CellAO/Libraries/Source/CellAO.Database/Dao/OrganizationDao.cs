#region License

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

namespace CellAO.Database.Dao
{
    #region Usings ...

    using System;
    using System.Linq;

    #endregion

    /// <summary>
    /// Data access object for Organization queries
    /// </summary>
    public class OrganizationDao : Dao<DBOrganization>
    {
        /// <summary>
        /// </summary>
        public static OrganizationDao Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OrganizationDao();
                    _instance.TableName = getTablename();
                }

                return (OrganizationDao)_instance;
            }
        }

        #region Public Methods and Operators

        /// <summary>
        /// Create a new organization
        /// </summary>
        /// <param name="desiredOrgName">
        /// Desired organization name
        /// </param>
        /// <param name="creationDate">
        /// Date of the creation
        /// </param>
        /// <param name="leaderId">
        /// Id of the president of the new organization
        /// </param>
        /// <returns>
        /// true if successful
        /// </returns>
        public bool CreateOrganization(string desiredOrgName, DateTime creationDate, int leaderId)
        {
            bool canBeCreated = !OrgExists(desiredOrgName);
            if (canBeCreated)
            {
                DBOrganization newOrganization = new DBOrganization();
                newOrganization.LeaderId = leaderId;
                newOrganization.Creation = creationDate;
                newOrganization.Name = desiredOrgName;
                this.Add(newOrganization);
            }

            return canBeCreated;
        }

        /// <summary>
        /// Retreive government form of existing organization
        /// </summary>
        /// <param name="orgId">
        /// Id of the organization
        /// </param>
        /// <returns>
        /// Government form as int
        /// </returns>
        public int GetGovernmentForm(int orgId)
        {
            if (orgId == 0)
            {
                return 0;
            }

            DBOrganization temp = this.Get(orgId);
            return temp.GovernmentForm;
        }

        /// <summary>
        /// Get organization id from name
        /// </summary>
        /// <param name="orgName">
        /// Name of the organization
        /// </param>
        /// <returns>
        /// Id of the organization
        /// </returns>
        public int GetOrganizationId(string orgName)
        {
            DBOrganization temp = this.GetAll(new { Name = orgName }).FirstOrDefault();
            if (temp != null)
            {
                return temp.Id;
            }

            return 0;
        }

        /// <summary>
        /// Check if organization exists
        /// </summary>
        /// <param name="name">
        /// Name of the organization
        /// </param>
        /// <returns>
        /// true if the organization exists
        /// </returns>
        public bool OrgExists(string name)
        {
            return this.GetAll(new { Name = name }).Any();
        }

        /// <summary>
        /// Check if organization exists
        /// </summary>
        /// <param name="orgId">
        /// Id of the organization
        /// </param>
        /// <returns>
        /// true if the organization exists
        /// </returns>
        public bool OrgExists(int orgId)
        {
            return this.Get(orgId) != null;
        }

        /// <summary>
        /// Elect a new president
        /// </summary>
        /// <param name="orgId">
        /// Id of the organization
        /// </param>
        /// <param name="newLeaderId">
        /// Id of the new president
        /// </param>
        public void SetNewPrez(int orgId, int newLeaderId)
        {
            DBOrganization temp = this.Get(orgId);
            temp.LeaderId = newLeaderId;
            this.Save(temp);
        }

        #endregion
    }
}