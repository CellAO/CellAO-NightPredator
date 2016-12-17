using System;
using System.Collections.Generic;
using System.Linq;

namespace ZoneEngine.Core
{

    using CellAO.ObjectManager;

    using CellAO.Core.Entities;
    using ZoneEngine.Core.MessageHandlers;

    using SmokeLounge.AOtomation.Messaging.GameData;
    using SmokeLounge.AOtomation.Messaging.Messages.N3Messages;

    public class Team : PooledObject
    {
        public Identity LeaderIdentity { get; set; }
        public Character Leader
        {
            get
            {
                return Pool.Instance.GetAll<Character>((int)IdentityType.CanbeAffected)
                            .Where(x => x.Identity.Equals(this.LeaderIdentity)).FirstOrDefault();
            }
            set
            {
                this.LeaderIdentity = value.Identity;
            }
        }

        public List<Identity> Characters { get; set; }

        public Team(Identity parent, Identity teamId, Identity leader): base(parent, teamId)
        {
            LeaderIdentity = leader;
            Characters = new List<Identity>();
            //CellAO.ObjectManager.Pool.Instance.AddObject<Team>(parent, this);
            Characters.Add(leader);

            Leader.Send(new TeamMemberMessage()
            {
                Identity = Leader.Identity,
                Character = Leader.Identity,
                Team = this.Identity,
                Name = Leader.Name
            });
            TeamMemberInfoMessage teamMemberInfo = new TeamMemberInfoMessage()
            {
                Identity = Leader.Identity,
                Character = Leader.Identity
            };
            Leader.Send(teamMemberInfo);
        }

        public static Team GetCharacterTeam(Identity id)
        {
            return CellAO.ObjectManager.Pool.Instance
                        .GetAll<Team>((int)IdentityType.TeamWindow)
                        .Where(t => t.HasMember(id))
                        .FirstOrDefault();
        }

        public static Team GenerateNewTeam(Identity leader)
        {
            var id = new Identity() { Instance = Pool.Instance.GetFreeInstance<Team>(0, IdentityType.TeamWindow), Type = IdentityType.TeamWindow };
            return new Team(Identity.None, id, leader);
        }

        public bool HasMember(Identity char1)
        {
            return this.Characters.Count(c => c.Equals(char1)) > 0;
        }

        public void Disband()
        {
            for (var i = 0; i < Characters.Count; i++) {
                if (Characters.Count > 0)
                    RemovePlayer(Characters[i]);
            }
        }

        public void RemovePlayer(Identity charIdentity)
        {
            foreach (var member in this.Characters)
            {
                // Alert all team members that the given team member has been removed
                Character.GetCharacter(member).Send(new CharacterActionMessage()
                {
                    Action = CharacterActionType.LeaveTeamOutbound,
                    Target = charIdentity,
                    Identity = member,
                    Parameter1 = this.Identity.Instance,
                    Parameter2 = 0xFFFFFFFF
                });
                Character.GetCharacter(member).Send(new TeamMemberInfoMessage()
                {
                    Identity = member,
                    Character = charIdentity
                });
            }
            foreach (var member in this.Characters)
            {
                // remove all team members from removed person's UI
                Character.GetCharacter(charIdentity).Send(new CharacterActionMessage()
                {
                    Action = CharacterActionType.LeaveTeamOutbound,
                    Target = member,
                    Identity = charIdentity,
                    Parameter1 = this.Identity.Instance,
                    Parameter2 = 0xFFFFFFFF
                });
            }
            this.Characters = this.Characters.Where(c => !c.Equals(charIdentity)).ToList();
            if (this.Characters.Count == 1)
            {
                // Last person in team.. just dissolve the team by removing the last remaining person
                Character.GetCharacter(this.Characters[0]).Send(new CharacterActionMessage()
                {
                    Action = CharacterActionType.LeaveTeamOutbound,
                    Target = this.Characters[0],
                    Identity = this.Characters[0],
                    Parameter1 = this.Identity.Instance,
                    Parameter2 = 0xFFFFFFFF
                });
                this.Characters = new List<Identity>();
                Pool.Instance.RemoveObject<Team>(this);
            }
        }

        public void AddPlayer(Identity charIdentity)
        {
            var char1 = Character.GetCharacter(charIdentity);
            // Team is full
            if (this.Characters.Count >= 6)
            {

            }
            // Player already in other team
            else if (Team.GetCharacterTeam(char1.Identity)!=null)
            {

            }
            else
            {
                CharacterActionMessage teamMemberInfo = new CharacterActionMessage()
                {
                    Action = CharacterActionType.AcceptTeamRequest,
                    Identity = Leader.Identity,
                    Target = Leader.Identity,
                    Parameter1 = 0x0000DEA9,
                    Parameter2 = System.Convert.ToUInt32(this.Identity.Instance)
                };
                Leader.Send(teamMemberInfo);
                var newMember = Character.GetCharacter(charIdentity);

                var members = Characters.Select(c => Character.GetCharacter(c)).ToList();
                foreach (var c in members)
                {
                    //c.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(
                    //  c,
                    //  String.Format("{0}, a level {1} {2} {3}, has joined your team.",
                    //    newMember.Name,
                    //    (newMember.Stats[CellAO.Enums.StatIds.level].Value).ToString(),
                    //    ((Side)newMember.Stats[CellAO.Enums.StatIds.side].Value).ToString(),
                    //    ((Profession)newMember.Stats[CellAO.Enums.StatIds.visualprofession].Value).ToString())));
                    // Everyone gets the new team member added
                    c.Send(new TeamMemberMessage()
                    {
                        Team = this.Identity,
                        Identity = c.Identity,
                        Character = newMember.Identity,
                        Name = newMember.Name
                    });
                };
                Characters.Add(charIdentity);
                members.Add(newMember);

                //char1.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(char1, "You have joined a team. Members:"));

                newMember.Send(new TeamMemberMessage()
                {
                    Team = this.Identity,
                    Identity = newMember.Identity,
                    Character = newMember.Identity,
                    Name = newMember.Name
                });
                foreach (var member in members)
                {
                    //newMember.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(
                    //  char1,
                    //  String.Format("    {0} - Level {1} {2} {3}",
                    //    member.Name,
                    //    (member.Stats[CellAO.Enums.StatIds.level].Value).ToString(),
                    //    ((Side)member.Stats[CellAO.Enums.StatIds.side].Value).ToString(),
                    //    ((Profession)member.Stats[CellAO.Enums.StatIds.visualprofession].Value).ToString())));
                    if (newMember != member)
                    {
                        newMember.Send(new TeamMemberMessage()
                        {
                            Team = this.Identity,
                            Identity = newMember.Identity,
                            Character = member.Identity,
                            Name = member.Name
                        });
                    }
                };
                /*
                TeamMemberInfoMessage teamMemberInfo = new TeamMemberInfoMessage()
                {
                  Identity = member.Identity,
                  Character = member.Identity,
                  Character2 = this.Identity
                };
                member.Send(teamMemberInfo);
                */
            }
        }
    }
}