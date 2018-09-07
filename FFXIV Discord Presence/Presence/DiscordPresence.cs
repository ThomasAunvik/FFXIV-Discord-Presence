using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscordRPC;
using DiscordRPC.Logging;

namespace FFXIV_DiscordPresence.Presence
{
    public class DiscordPresence
    {
        private DiscordRpcClient client;

        public RichPresence presence;
        public Party party = new Party();

        public string playerName;
        public string lvl;
        public string job;

        public string place;

        public bool inParty = false;
        public int maxPartySize = 4;
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

            //Set the rich presence
            client.SetPresence(presence);
        }

        public void UpdatePresence()
        {
            string state = string.Empty;
            if(place != string.Empty)
            {
                state += place;
            }
            
            if (inParty)
            {
                party.Max = maxPartySize;
                party.Size = partySize;
                presence.Party = party;
            }
            else
            {
                presence.Party = null;
            }

            string details = "";
            if (playerName != string.Empty) details = playerName + " - " + job + " lvl" + lvl;
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
