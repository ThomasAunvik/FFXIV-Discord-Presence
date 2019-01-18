using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FFXIV_DiscordPresence.FFXIV;
using Sharlayan;
using Sharlayan.Core;
using Sharlayan.Events;
using Sharlayan.Models;
using Sharlayan.Models.ReadResults;

using FFXIV_DiscordPresence.Presence;
using Sharlayan.Models.XIVDatabase;
using Sharlayan.Utilities;

namespace FFXIV_DiscordPresence
{
    class Program
    {
        public const string DISCORD_CLIENT_ID = "487662820284956692";



        static void Main(string[] args)
        {
            FFProcess process = new FFProcess(FFProcess.DX11_DEFAULT_NAME, true);
            DiscordPresence presence = new DiscordPresence(DISCORD_CLIENT_ID);

            bool shutdown = false;

            new Thread(x => {
                while (!shutdown)
                {
                    if(Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        shutdown = true;
                        Console.WriteLine("Stopping application...");
                    }
                }
            }).Start();

            Console.WriteLine("Press ESC to stop");
            do
            {
                while (Scanner.Instance.IsScanning)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Scanning...");
                }
                CurrentPlayer currentPlayer = Reader.GetCurrentPlayer().CurrentPlayer;
                ActorItem player = null/*ActorItem.CurrentUser*/;
                if(player == null)
                {
                    ActorResult aResult = Reader.GetActors();
                    KeyValuePair<uint, ActorItem> playerKeyValue = aResult.CurrentPCs.ToList().Find(x => x.Value.Name == currentPlayer.Name);
                    ActorItem playerItem = playerKeyValue.Value;

                    player = playerItem;
                }

                if (player != null)
                {
                    presence.playerName = player.Name;
                    presence.lvl = player.Level.ToString();
                    presence.job = player.Job.ToString();

                    uint mapID = player.MapTerritory;
                    MapItem zone = ZoneLookup.GetZoneInfo(mapID);
                    presence.place = zone.Name.English;

                    presence.UpdatePresence();
                }
                Thread.Sleep(5000);
            } while (!shutdown);

            presence.Deinitialize();
        }
    }
}
