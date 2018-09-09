using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscordRPC;
using DiscordRPC.Logging;
using Sharlayan;
using Sharlayan.Models.ReadResults;

namespace FFXIV_DiscordPresence.Presence
{
    public class DiscordPresence
    {
        private DiscordRpcClient client;

        public RichPresence presence;

        public string playerName;
        public string lvl;
        public string job;

        public string place;

        public bool IsInParty() {
            PartyResult party = Reader.GetPartyMembers();
            partySize = party.PartyMembers.Count;
            return partySize > 1;
        }
        public int partySize;

        public DiscordPresence(string clientID, string name = "Final Fantasy XIV")
        {
            client = new DiscordRpcClient(clientID, true, -1);

            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            client.Initialize();

            presence = new RichPresence()
            {
                Details = "Main Menu",
                State = "",
                Assets = new Assets()
                {
                    LargeImageKey = "ffxivmeteorlogo",
                    LargeImageText = "Final Fantasy XIV",
                    SmallImageKey = "image_small"
                }
            };
            
            client.SetPresence(presence);
        }

        public void UpdatePresence()
        {
            string state = string.Empty;
            if(place != string.Empty)
            {
                state += place;
            }
            
            if (IsInParty())
            {
                switch (partySize)
                {
                    case 4: state += " - Light Party"; break;
                    case 8: state += " - Full Party"; break;
                    case 24: state += " - Full Raid"; break;
                }
            }
            else
            {
                state += " - Solo";
            }

            string details = "";
            if (playerName != string.Empty)
            {
                details = playerName + " - " + job + " lvl" + lvl;
            }
            else state = string.Empty;
            

            if(details != string.Empty) presence.Details = details;
            if(state != string.Empty) presence.State = state;

            client.SetPresence(presence);
            client.Invoke();
        }

        public void Deinitialize()
        {
            client.Dispose();
        }
    }
}
