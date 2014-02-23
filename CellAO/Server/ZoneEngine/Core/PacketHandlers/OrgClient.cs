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

namespace ZoneEngine.Core.PacketHandlers
{
    #region Usings ...

    using System;

    using CellAO.Core.Entities;
    using CellAO.Database.Dao;
    using CellAO.Database.Entities;
    using CellAO.Enums;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages.OrgServerMessages;

    using ZoneEngine.Core.Packets;

    #endregion

    /// <summary>
    /// </summary>
    public static class OrgClient
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="message">
        /// </param>
        /// <param name="client">
        /// </param>
        public static void Read(OrgClientMessage message, ZoneClient client)
        {
            switch ((byte)message.Command)
            {
                case 1:
                {
                    // org create
                    /* client wants to create organization
                             * name of org is message.CommandArgs
                             */

                    if (OrganizationDao.CreateOrganization(
                        message.CommandArgs, 
                        DateTime.UtcNow, 
                        client.Character.Identity.Instance))
                    {
                        client.Character.Playfield.Publish(
                            ChatText.CreateIM(client.Character, "You have created the guild: " + message.CommandArgs));

                        int orgID = OrganizationDao.GetOrganizationId(message.CommandArgs);

                        // Make sure the order of these next two lines is not swapped -NV
                        client.Character.Stats[StatIds.clanlevel].Value = 0;
                        client.Character.Stats[StatIds.clan].Value = orgID;
                        break;
                    }
                    else
                    {
                        client.Character.Playfield.Publish(
                            ChatText.CreateIM(client.Character, "This guild already <font color=#DC143C>exists</font>"));
                        break;
                    }
                }

                case 2:

                    // org ranks
                    // Displays Org Rank Structure.
                    /* Select governingform from DB, Roll through display from GovForm */
                    if (client.Character.Stats[StatIds.clan].BaseValue == 0)
                    {
                        client.Character.Playfield.Publish(
                            ChatText.CreateIM(client.Character, "You're not in an organization!"));
                        break;
                    }

                    int governingForm =
                        OrganizationDao.GetGovernmentForm((int)client.Character.Stats[StatIds.clan].BaseValue);

                    client.Character.Playfield.Publish(
                        ChatText.CreateIM(client.Character, "Current Rank Structure: " + GetRankList(governingForm)));
                    break;

                case 3:

                    // org contract
                    break;

                case 4:
                    Console.WriteLine("Case 4 Started");
                    break;

                case 5:
                {
                    IInstancedEntity tPlayer = null;
                    if ((tPlayer = client.Playfield.FindByIdentity(message.Target)) != null)
                    {
                        string orgDescription = string.Empty, 
                            orgObjective = string.Empty, 
                            orgHistory = string.Empty, 
                            orgLeaderName = string.Empty;
                        int orgGoverningForm = 0, orgLeaderID = 0;

                        DBOrganization orgData =
                            OrganizationDao.GetOrganizationData((int)tPlayer.Stats[StatIds.clan].BaseValue);

                        if (orgData != null)
                        {
                            orgDescription = orgData.Description;
                            orgObjective = orgData.Objective;
                            orgHistory = orgData.History;
                            orgGoverningForm = orgData.GovernmentForm;
                            orgLeaderID = orgData.LeaderID;
                        }

                        orgLeaderName = CharacterDao.Instance.GetCharacterNameById(orgLeaderID);

                        string textGovForm = null;
                        if (orgGoverningForm == 0)
                        {
                            textGovForm = "Department";
                        }
                        else if (orgGoverningForm == 1)
                        {
                            textGovForm = "Faction";
                        }
                        else if (orgGoverningForm == 2)
                        {
                            textGovForm = "Republic";
                        }
                        else if (orgGoverningForm == 3)
                        {
                            textGovForm = "Monarchy";
                        }
                        else if (orgGoverningForm == 4)
                        {
                            textGovForm = "Anarchism";
                        }
                        else if (orgGoverningForm == 5)
                        {
                            textGovForm = "Feudalism";
                        }
                        else
                        {
                            textGovForm = "Department";
                        }

                        string orgRank = GetRank(orgGoverningForm, tPlayer.Stats[StatIds.clanlevel].BaseValue);

                        var infoMessage = new OrgInfoMessage
                                          {
                                              Identity = tPlayer.Identity, 
                                              Unknown = 0x00, 
                                              Unknown1 = 0x00000000, 
                                              Unknown2 = 0x00000000, 
                                              Organization =
                                                  new Identity
                                                  {
                                                      Type = IdentityType.Organization, 
                                                      Instance =
                                                          (int)
                                                          tPlayer.Stats[StatIds.clan]
                                                          .BaseValue
                                                  }, 

                                              // TODO: Possible NULL here
                                              OrganizationName =
                                                  (tPlayer as Character).OrganizationName, 
                                              Description = orgDescription, 
                                              Objective = orgObjective, 
                                              GoverningForm = textGovForm, 
                                              LeaderName = orgLeaderName, 
                                              Rank = orgRank, 
                                              Unknown3 = new object[0]
                                          };

                        client.SendCompressed(infoMessage);
                    }
                }

                    break;

                case 6:

                    // Add Org Bank to prez
                    DBOrganization orgDisband =
                        OrganizationDao.GetOrganizationData((int)client.Character.Stats[StatIds.clan].BaseValue);
                    client.Character.Stats[StatIds.cash].BaseValue += (uint)orgDisband.Bank;

                    // Clear stat 5 (Clan) from all chars where value=orgId
                    StatDao.DisbandOrganization((int)client.Character.Stats[StatIds.clan].BaseValue);
                    break;

                case 7:

                    // org startvote <"text"> <duration(minutes)> <entries>
                    // arguments (<text> <duration> and <entries>) are in CmdStr
                    break;

                case 8:

                    // org vote info
                    break;

                case 9:

                    // <entry> is CmdStr
                    break;

                case 10:
                {
                    // some arg in CmdByte. No idea what it is

                    // create the target namespace t_promote
                    Character toPromote = null;
                    int targetOldRank = -1;
                    int targetNewRank;
                    toPromote = client.Playfield.FindByIdentity<Character>(message.Target);
                    if (toPromote != null)
                    {
                        // First we check if target is in the same org as you
                        if (toPromote.Stats[StatIds.clan].BaseValue != client.Character.Stats[StatIds.clan].BaseValue)
                        {
                            // not in same org

                            client.Character.Playfield.Publish(
                                ChatText.CreateIM(client.Character, "Target is not in your organization!"));
                            break;
                        }

                        // Target is in same org, are you eligible to promote?  Promoter Rank has to be TargetRank-2 or == 0
                        if ((client.Character.Stats[StatIds.clanlevel].Value
                             == (toPromote.Stats[StatIds.clanlevel].Value - 2))
                            || (client.Character.Stats[StatIds.clanlevel].Value == 0))
                        {
                            // Promoter is eligible. Start the process

                            // First we get the details about the org itself
                            DBOrganization orgPromote =
                                OrganizationDao.GetOrganizationData((int)client.Character.Stats[StatIds.clan].BaseValue);

                            int promoteGovForm = -1;
                            string promotedToRank = string.Empty;

                            if (orgPromote != null)
                            {
                                // Check if new rank == 0, if so, demote promoter
                                if ((targetOldRank - 1) == 0)
                                {
                                    /* This is a bit more complex.  Here we need to promote new president first
                                                 * then we go about demoting old president
                                                 * finally we set the new leader in Sql
                                                 * Reset OrgName to set changes
                                                 */

                                    OrganizationDao.SetNewPrez(orgPromote.ID, toPromote.Identity.Instance);
                                    toPromote.Stats[StatIds.clanlevel].Value = 0;
                                    client.Character.Stats[StatIds.clanlevel].Value = 1;

                                    client.Character.Playfield.Publish(
                                        ChatText.CreateIM(
                                            client.Character, 
                                            "You've passed leadership of the organization to: "
                                            + (toPromote as Character).Name));
                                    client.Character.Playfield.Publish(
                                        ChatText.CreateIM(
                                            toPromote, 
                                            "You've been promoted to the rank of " + promotedToRank + " by "
                                            + client.Character.Name));
                                }
                                else
                                {
                                    // Just Promote
                                    targetOldRank = toPromote.Stats[StatIds.clanlevel].Value;
                                    targetNewRank = targetOldRank - 1;
                                    promotedToRank = GetRank(promoteGovForm, (uint)targetNewRank);
                                    toPromote.Stats[StatIds.clanlevel].Value = targetNewRank;
                                    client.Character.Playfield.Publish(
                                        ChatText.CreateIM(
                                            client.Character, 
                                            "You've promoted " + (toPromote as Character).Name + " to " + promotedToRank));
                                    client.Character.Playfield.Publish(
                                        ChatText.CreateIM(
                                            toPromote, 
                                            "You've been promoted to the rank of " + promotedToRank + " by "
                                            + client.Character.Name));
                                }
                            }
                            else
                            {
                                client.Character.Playfield.Publish(
                                    ChatText.CreateIM(client.Character, "Organization does not exist?"));
                            }
                        }
                        else
                        {
                            // Promoter not eligible to promote
                            client.Character.Playfield.Publish(
                                ChatText.CreateIM(
                                    client.Character, 
                                    "Your Rank is not high enough to promote " + (toPromote as Character).Name));
                        }
                    }

                    break;
                }

                case 11:

                    // demote target player
                    // create the target namespace t_demote
                    Character toDemote = null;
                    int targetCurRank = -1;
                    int targetNewerRank = -1;
                    toDemote = client.Character.Playfield.FindByIdentity<Character>(message.Target);
                    if (toDemote != null)
                    {
                        // First we check if target is in the same org as you
                        if (toDemote.Stats[StatIds.clan].BaseValue != client.Character.Stats[StatIds.clan].BaseValue)
                        {
                            // not in same org
                            client.Character.Playfield.Publish(
                                ChatText.CreateIM(client.Character, "Target is not in your organization!"));
                            break;
                        }

                        // Target is in same org, are you eligible to demote?  Promoter Rank has to be TargetRank-2 or == 0
                        if ((client.Character.Stats[StatIds.clanlevel].Value
                             <= (toDemote.Stats[StatIds.clanlevel].Value - 2))
                            || (client.Character.Stats[StatIds.clanlevel].Value == 0))
                        {
                            // Promoter is eligible. Start the process

                            // First we get the details about the org itself
                            DBOrganization orgDemote =
                                OrganizationDao.GetOrganizationData((int)client.Character.Stats[StatIds.clan].BaseValue);
                            int demoteGovForm = -1;
                            string demotedToRank = string.Empty;
                            if (orgDemote == null)
                            {
                                client.Character.Playfield.Publish(
                                    ChatText.CreateIM(client.Character, "Organization does not exist?"));
                                break;
                            }

                            // Check whether new rank would be lower than lowest for current govform
                            if ((targetCurRank + 1) > GetLowestRank(orgDemote.GovernmentForm))
                            {
                                client.Character.Playfield.Publish(
                                    ChatText.CreateIM(client.Character, "You can't demote character any lower!"));
                                break;
                            }

                            targetCurRank = toDemote.Stats[StatIds.clanlevel].Value;
                            targetNewerRank = targetCurRank + 1;
                            demotedToRank = GetRank(demoteGovForm, (uint)targetNewerRank);
                            toDemote.Stats[StatIds.clanlevel].Value = targetNewerRank;
                            client.Character.Playfield.Publish(
                                ChatText.CreateIM(
                                    client.Character, 
                                    "You've demoted " + (toDemote as Character).Name + " to " + demotedToRank));
                            client.Character.Playfield.Publish(
                                ChatText.CreateIM(
                                    toDemote, 
                                    "You've been demoted to the rank of " + demotedToRank + " by "
                                    + client.Character.Name));
                            break;
                        }
                        else
                        {
                            // Promoter not eligible to promote
                            client.Character.Playfield.Publish(
                                ChatText.CreateIM(
                                    client.Character, 
                                    "Your Rank is not high enough to demote " + (toDemote as Character).Name));
                            break;
                        }
                    }

                    break;

                case 12:
                    Console.WriteLine("Case 12 Started");
                    break;

                case 13:

                    // kick <name> from org
                    // <name> is CmdStr

                    // create the t_player Client namespace, using CmdStr to find character id, in replacement of target.Instance
                    uint kickedFrom = client.Character.Stats[StatIds.clan].BaseValue;
                    DBCharacter kickChar = CharacterDao.Instance.GetByCharName(message.CommandArgs);
                    if (kickChar == null)
                    {
                        client.Character.Playfield.Publish(
                            ChatText.CreateIM(
                                client.Character, 
                                "No character with name " + message.CommandArgs + " exists."));
                        break;
                    }

                    int kickeeId = kickChar.Id;

                    Character targetPlayer =
                        client.Playfield.FindByIdentity<Character>(
                            new Identity() { Type = IdentityType.CanbeAffected, Instance = kickeeId });

                    if (targetPlayer != null)
                    {
                        // Check if CmdStr is actually part of the org

                        uint kickeeOrgId = targetPlayer.Stats[StatIds.clan].BaseValue;
                        if (kickeeOrgId != client.Character.Stats[StatIds.clan].BaseValue)
                        {
                            // Not part of Org. break out.
                            client.Character.Playfield.Publish(
                                ChatText.CreateIM(
                                    client.Character, 
                                    message.CommandArgs + "is not a member of your organization!"));
                            break;
                        }

                        // They are part of the org, so begin the processing...
                        // First we check if the player is online...
                        int onlineStatus = OnlineDao.IsOnline(client.Character.Identity.Instance).Online;

                        if (onlineStatus == 0)
                        {
                            // Player isn't online. Org Kicks are processed in a different method
                            // TODO: Offline Org KICK
                            // StatDao.SetStat(targetPlayer.Identity, )
                            break;
                        }

                        // Player is online. Start the kick.
                        string kickedFromName = targetPlayer.OrganizationName;
                        targetPlayer[StatIds.clanlevel].Value = 0;
                        targetPlayer[StatIds.clan].Value = 0;

                        client.Character.Playfield.Publish(
                            ChatText.CreateIM(
                                targetPlayer, 
                                "You've been kicked from the organization " + kickedFromName));
                    }

                    // TODO: Offline Org KICK
                    break;

                case 14:
                {
                    Character tPlayer = client.Playfield.FindByIdentity<Character>(message.Target);
                    if (tPlayer != null)
                    {
                        var inviteMessage = new OrgInviteMessage
                                            {
                                                Identity = tPlayer.Identity, 
                                                Unknown = 0x00, 
                                                Unknown1 = 0x00000000, 
                                                Unknown2 = 0x00000000, 
                                                Organization =
                                                    new Identity
                                                    {
                                                        Type = IdentityType.Organization, 
                                                        Instance =
                                                            (int)
                                                            client.Character.Stats[
                                                                (int)StatIds.clan].Value
                                                    }, 
                                                OrganizationName = client.Character.OrganizationName, 
                                                Unknown3 = 0x00000000
                                            };

                        tPlayer.Client.SendCompressed(inviteMessage);
                    }
                }

                    break;

                    

                case 15:
                {
                    // target.Instance holds the OrgID of the Org wishing to be joined.
                    int orgIdtoJoin = message.Target.Instance;
                    int gov_form = OrganizationDao.GetGovernmentForm(orgIdtoJoin);

                    // Make sure the order of these next two lines is not swapped -NV
                    client.Character.Stats[StatIds.clanlevel].Value = GetLowestRank(gov_form);
                    client.Character.Stats[StatIds.clan].Value = orgIdtoJoin;
                }

                    break;

                    

                    #region /org leave

                case 16:

                    // org leave
                    // TODO: Disband org if it was leader that left org. -Suiv-
                    // I don't think a Disband happens if leader leaves. I don't think leader -can- leave without passing lead to another
                    // Something worth testing on Testlive perhaps ~Chaz
                    // Just because something happens on TL, doesnt mean its a good idea. Really tbh id prefer it if you had to explicitly type /org disband to disband rather than /org leave doing it... -NV
                    // Agreeing with NV.  Org Leader can't leave without passing lead on.  org disband requires /org disband to specifically be issued, with a Yes/No box.

                    int govern_form = OrganizationDao.GetGovernmentForm(client.Character.Stats[StatIds.clan].Value);

                    if ((client.Character.Stats[StatIds.clanlevel].Value == 0) && (govern_form != 4))
                    {
                        client.Character.Playfield.Publish(
                            ChatText.CreateIM(
                                client.Character, 
                                "Organization Leader cannot leave organization without Disbanding or Passing Leadership!"));
                    }
                    else
                    {
                        int oldOrgId = client.Character.Stats[StatIds.clan].Value;
                        string orgName = OrganizationDao.GetOrganizationData(oldOrgId).Name;
                        client.Character.Playfield.Publish(
                            ChatText.CreateIM(client.Character, "You left the organization " + orgName + "."));
                    }

                    break;

                    #endregion

                    #region /org tax | /org tax <tax>

                case 17:

                    // gets or sets org tax
                    // <tax> is CmdStr
                    // if no <tax>, then just send chat text with current tax info
                    if (message.CommandArgs == null)
                    {
                        client.Character.Playfield.Publish(
                            ChatText.CreateIM(client.Character, "The current organization tax rate is: "));
                        break;
                    }
                    else
                    {
                        break;
                    }

                    #endregion

                    #region /org bank

                case 18:
                {
                    /*
                            // org bank
                            dt = ms.ReadDatatable("SELECT * FROM organizations WHERE ID=" + client.Character.OrgId);
                            if (dt.Rows.Count > 0)
                            {
                                var bank_credits = (UInt64)dt.Rows[0]["Bank"];
                                client.SendChatText("Your bank has " + bank_credits + " credits in its account");
                            }
                          */
                }

                    break;

                    #endregion

                    #region /org bank add <cash>

                case 19:
                {
                    /*
                            if (client.Character.OrgId == 0)
                            {
                                client.SendChatText("You are not in an organisation.");

                                break;
                            }

                            // org bank add <cash>
                            var minuscredits_fromplayer = Convert.ToInt32(message.CommandArgs);
                            var characters_credits = client.Character.Stats.Cash.Value;

                            if (characters_credits < minuscredits_fromplayer)
                            {
                                client.SendChatText("You do not have enough Credits");
                            }
                            else
                            {
                                var total_Creditsspent = characters_credits - minuscredits_fromplayer;
                                client.Character.Stats.Cash.Set(total_Creditsspent);

                                ms.SqlUpdate(
                                    "UPDATE `organizations` SET `Bank` = `Bank` + " + minuscredits_fromplayer
                                    + " WHERE `ID` = " + client.Character.OrgId);
                                client.SendChatText("You have donated " + minuscredits_fromplayer + " to the organization");
                            }*/
                }

                    break;

                    #endregion

                    #region /org bank remove <cash>

                case 20:

                    /*
                    // org bank remove <cash>
                    // <cash> is CmdStr
                    // player wants to take credits from org bank
                    // only leader can do that
                    if ((client.Character.Stats.ClanLevel.Value != 0) || (client.Character.OrgId == 0))
                    {
                        client.SendChatText("You're not the leader of an Organization");
                        break;
                    }

                    var removeCredits = Convert.ToInt32(message.CommandArgs);
                    long orgBank = 0;
                    dt = ms.ReadDatatable("SELECT Bank FROM organizations WHERE ID = " + client.Character.OrgId);
                    if (dt.Rows.Count > 0)
                    {
                        orgBank = (Int64)dt.Rows[0][0];
                    }

                    if (removeCredits > orgBank)
                    {
                        client.SendChatText("Not enough credits in Organization Bank!");
                        break;
                    }
                    else
                    {
                        var neworgbank = orgBank - removeCredits;
                        var existingcreds = 0;
                        existingcreds = client.Character.Stats.Cash.Value;
                        var newcreds = existingcreds + removeCredits;
                        ms.SqlUpdate(
                            "UPDATE organizations SET Bank = " + neworgbank + " WHERE ID = " + client.Character.OrgId);
                        client.Character.Stats.Cash.Set(newcreds);
                        client.SendChatText("You've removed " + removeCredits + " credits from the organization bank");
                    }
                    */
                    break;

                    #endregion

                    #region /org bank paymembers <cash>

                case 21:

                    // <cash> is CmdStr
                    // give <cash> credits to every org member
                    // credits are taken from org bank
                    // only leader can do it
                    break;

                    #endregion

                    #region /org debt

                case 22:

                    // send player text about how big is his/her tax debt to org
                    break;

                    #endregion

                    #region /org history <text>

                case 23:
                {
                    /*
                            if (client.Character.Stats.ClanLevel.Value == 0)
                            {
                                // org history <history text>
                                ms.SqlUpdate(
                                    "UPDATE organizations SET history = '" + message.CommandArgs + "' WHERE ID = '"
                                    + client.Character.OrgId + "'");
                                client.SendChatText("History Updated");
                            }
                            else
                            {
                                client.SendChatText("You must be the Organization Leader to perform this command!");
                            }*/
                }

                    break;

                    #endregion

                    #region /org objective <text>

                case 24:
                {
                    /*
                            if (client.Character.Stats.ClanLevel.Value == 0)
                            {
                                // org objective <objective text>
                                ms.SqlUpdate(
                                    "UPDATE organizations SET objective = '" + message.CommandArgs + "' WHERE ID = '"
                                    + client.Character.OrgId + "'");
                                client.SendChatText("Objective Updated");
                            }
                            else
                            {
                                client.SendChatText("You must be the Organization Leader to perform this command!");
                            }*/
                }

                    break;

                    #endregion

                    #region /org description <text>

                case 25:
                {
                    /*
                            if (client.Character.Stats.ClanLevel.Value == 0)
                            {
                                // org description <description text>
                                ms.SqlUpdate(
                                    "UPDATE organizations SET description = '" + message.CommandArgs + "' WHERE ID = '"
                                    + client.Character.OrgId + "'");
                                client.SendChatText("Description Updated");
                            }
                            else
                            {
                                client.SendChatText("You must be the Organization Leader to perform this command!");
                            }*/
                }

                    break;

                    #endregion

                    #region /org name <text>

                case 26:
                {
                    // org name <name>
                    /* Renames Organization
                             * Checks for Existing Orgs with similar name to stop crash
                             * Chaz
                             */
                    /*
                            if (client.Character.Stats.ClanLevel.Value == 0)
                            {
                                var SqlQuery26 = "SELECT * FROM organizations WHERE Name LIKE '" + message.CommandArgs
                                                 + "' LIMIT 1";
                                string CurrentOrg = null;
                                dt = ms.ReadDatatable(SqlQuery26);
                                if (dt.Rows.Count > 0)
                                {
                                    CurrentOrg = (string)dt.Rows[0]["Name"];
                                }

                                if (CurrentOrg == null)
                                {
                                    var SqlQuery27 = "UPDATE organizations SET Name = '" + message.CommandArgs
                                                     + "' WHERE ID = '" + client.Character.OrgId + "'";
                                    ms.SqlUpdate(SqlQuery27);
                                    client.SendChatText("Organization Name Changed to: " + message.CommandArgs);

                                    // Forces reloading of org name and the like
                                    // XXXX TODO: Make it reload for all other members in the org
                                    client.Character.OrgId = client.Character.OrgId;
                                    break;
                                }
                                else
                                {
                                    client.SendChatText("An Organization already exists with that name");
                                    break;
                                }
                            }
                            else
                            {
                                client.SendChatText("You must be the organization leader to perform this command!");
                            }*/

                    break;
                }

                    #endregion

                    #region /org governingform <text>

                case 27:
                {
                    // org governingform <form>
                    /* Current Governing Forms:
                             * Department, Faction, Republic, Monarchy, Anarchism, Feudalism
                             */
                    /*
                            // Check on whether your President or not
                            if (client.Character.Stats.ClanLevel.Value == 0)
                            {
                                // first we drop the case on the input, just to be sure.
                                var GovFormNum = -1;
                                if (message.CommandArgs == null)
                                {
                                    // list gov forms
                                    client.SendChatText(
                                        "List of Accepted Governing Forms is: department, faction, republic, monarchy, anarchism, feudalism");
                                    break;
                                }

                                // was correct input passed?
                                switch (message.CommandArgs.ToLower())
                                {
                                    case "department":
                                        GovFormNum = 0;
                                        break;
                                    case "faction":
                                        GovFormNum = 1;
                                        break;
                                    case "republic":
                                        GovFormNum = 2;
                                        break;
                                    case "monarchy":
                                        GovFormNum = 3;
                                        break;
                                    case "anarchism":
                                        GovFormNum = 4;
                                        break;
                                    case "feudalism":
                                        GovFormNum = 5;
                                        break;
                                    default:
                                        client.SendChatText(message.CommandArgs + " Is an invalid Governing Form!");
                                        client.SendChatText(
                                            "Accepted Governing Forms are: department, faction, republic, monarchy, anarchism, feudalism");
                                        break;
                                }

                                if (GovFormNum != -1)
                                {
                                    ms.SqlUpdate(
                                        "UPDATE organizations SET GovernmentForm = '" + GovFormNum + "' WHERE ID = '"
                                        + client.Character.OrgId + "'");
                                    foreach (var currentCharId in OrgMisc.GetOrgMembers(client.Character.OrgId, true))
                                    {
                                        client.Character.Stats.ClanLevel.Set(GetLowestRank(GovFormNum));
                                    }

                                    client.SendChatText("Governing Form is now: " + message.CommandArgs);
                                    break;
                                }
                            }
                            else
                            {
                                // Haha! You're not the org leader!
                                client.SendChatText("You must be the Org Leader to perform this command");
                                break;
                            }*/
                }

                    break;

                    #endregion

                    #region /org stopvote <text>

                case 28:

                    // <text> is CmdStr
                    break;

                    #endregion

                    #region unknown command

                default:
                    break;

                    #endregion
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="GoverningForm">
        /// </param>
        /// <returns>
        /// </returns>
        internal static int GetLowestRank(int GoverningForm)
        {
            switch (GoverningForm)
            {
                case 0:
                    return 6;
                case 1:
                    return 4;
                case 2:
                    return 4;
                case 3:
                    return 2;
                case 4:
                    return 0;
                case 5:
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="GoverningForm">
        /// </param>
        /// <param name="Rank">
        /// </param>
        /// <returns>
        /// </returns>
        internal static string GetRank(int GoverningForm, uint Rank)
        {
            string[] Department =
            {
                "President", "General", "Squad Commander", "Unit Commander", "Unit Leader", 
                "Unit Member", "Applicant"
            };
            string[] Faction = { "Director", "Board Member", "Executive", "Member", "Applicant" };
            string[] Republic = { "President", "Advisor", "Veteran", "Member", "Applicant" };
            string[] Monarchy = { "Monarch", "Council", "Follower" };
            string[] Anarchism = { "Anarchist" };
            string[] Feudalism = { "Lord", "Knight", "Vassal", "Peasant" };

            switch (GoverningForm)
            {
                case 0:
                    if (Rank > 6)
                    {
                        return string.Empty;
                    }

                    return Department[Rank];
                case 1:
                    if (Rank > 4)
                    {
                        return string.Empty;
                    }

                    return Faction[Rank];
                case 2:
                    if (Rank > 4)
                    {
                        return string.Empty;
                    }

                    return Republic[Rank];
                case 3:
                    if (Rank > 2)
                    {
                        return string.Empty;
                    }

                    return Monarchy[Rank];
                case 4:
                    if (Rank > 0)
                    {
                        return string.Empty;
                    }

                    return Anarchism[Rank];
                case 5:
                    if (Rank > 3)
                    {
                        return string.Empty;
                    }

                    return Feudalism[Rank];
                default:

                    // 	wrong governingform (too high number)
                    return string.Empty;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="GoverningForm">
        /// </param>
        /// <returns>
        /// </returns>
        internal static string GetRankList(int GoverningForm)
        {
            string Department =
                "President, General, Squad Commander, Unit Commander, Unit Leader, Unit Member, Applicant";
            string Faction = "Director, Board Member, Executive, Member, Applicant";
            string Republic = "President, Advisor, Veteran, Member, Applicant";
            string Monarchy = "Monarch, Council, Follower";
            string Anarchism = "Anarchist";
            string Feudalism = "Lord, Knight, Vassal, Peasant";

            switch (GoverningForm)
            {
                case 0:
                    return Department;
                case 1:
                    return Faction;
                case 2:
                    return Republic;
                case 3:
                    return Monarchy;
                case 4:
                    return Anarchism;
                case 5:
                    return Feudalism;
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}