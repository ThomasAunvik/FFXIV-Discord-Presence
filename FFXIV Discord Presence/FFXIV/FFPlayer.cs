using Sharlayan;
using Sharlayan.Core;
using Sharlayan.Core.Enums;
using Sharlayan.Models.ReadResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFXIV_DiscordPresence.Presence
{
    public static class FFPlayer
    {
        public static CurrentPlayer GetPlayer()
        {
            return Reader.GetCurrentPlayer().CurrentPlayer;
        }

        public static ActorItem GetPlayerItem()
        {
            ActorResult aResult = Reader.GetActors();
            KeyValuePair<uint, ActorItem> playerKeyValue = aResult.CurrentPCs.ToList().Find(x => x.Value.Name == GetPlayer().Name);
            ActorItem playerItem = playerKeyValue.Value;
            
            return playerItem;
        }

        public static int GetPlayerLevel()
        {
            ActorItem playerItem = GetPlayerItem();
            if (playerItem != null)
            {
                return playerItem.Level;
            }
            return -1;
        }
    }
}
