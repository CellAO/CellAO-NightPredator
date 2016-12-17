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

        public Team(Identity parent, Identity teamId) : base(parent, teamId)
        {
            LeaderIdentity = parent;
            Characters = new List<Identity>();
            CellAO.ObjectManager.Pool.Instance.AddObject<Team>(teamId, this);
            Characters.Add(parent);

            Leader.Send(new TeamMemberMessage()
            {
                Identity = Leader.Identity,
                Character = Leader.Identity,
                Team = this.Identity,
                Name = Leader.Name
            });
        }

        public static Identity GenerateNewTeamIdentity()
        {
            return new Identity() { Instance = Pool.Instance.GetFreeInstance<Team>(0, IdentityType.TeamWindow), Type = IdentityType.TeamWindow };
        }

        public bool HasMember(Identity char1)
        {
            return this.Characters.Count(c => c.Equals(char1)) > 0;
        }

        public static void SendInvite(ICharacter char1)
        {

        }

        public void AddPlayer(Identity charIdentity)
        {
            var char1 = CellAO.ObjectManager.Pool.Instance.GetAll<Character>((int)IdentityType.CanbeAffected)
                          .Where(x => x.Identity.Equals(charIdentity)).FirstOrDefault();
            // Team is full
            if (this.Characters.Count >= 6)
            {

            }
            // Player already in other team
            else if (Pool.Instance.GetAll<Team>((int)IdentityType.TeamWindow).Where(t => t.HasMember(char1.Identity)).Count() > 0)
            {

            }
            else
            {
                var newMember = CellAO.ObjectManager.Pool.Instance.GetAll<Character>((int)IdentityType.CanbeAffected)
                              .Where(x => x.Identity.Equals(charIdentity)).FirstOrDefault();

                var members = Characters.Select(c => CellAO.ObjectManager.Pool.Instance.GetAll<Character>((int)IdentityType.CanbeAffected)
                             .Where(x => x.Identity.Equals(c)).FirstOrDefault()).ToList();
                foreach (var c in members)
                {
                    c.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(
                      c,
                      String.Format("{0}, a level {1} {2} {3}, has joined your team.",
                        newMember.Name,
                        (newMember.Stats[CellAO.Enums.StatIds.level].Value).ToString(),
                        ((Side)newMember.Stats[CellAO.Enums.StatIds.side].Value).ToString(),
                        ((Profession)newMember.Stats[CellAO.Enums.StatIds.visualprofession].Value).ToString())));
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

                char1.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(char1, "You have joined a team. Members:"));

                newMember.Send(new TeamMemberMessage()
                {
                    Team = this.Identity,
                    Identity = newMember.Identity,
                    Character = newMember.Identity,
                    Name = newMember.Name
                });
                foreach (var member in members)
                {
                    newMember.Playfield.Publish(ChatTextMessageHandler.Default.CreateIM(
                      char1,
                      String.Format("    {0} - Level {1} {2} {3}",
                        member.Name,
                        (member.Stats[CellAO.Enums.StatIds.level].Value).ToString(),
                        ((Side)member.Stats[CellAO.Enums.StatIds.side].Value).ToString(),
                        ((Profession)member.Stats[CellAO.Enums.StatIds.visualprofession].Value).ToString())));
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