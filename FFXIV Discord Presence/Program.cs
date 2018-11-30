﻿using System;
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
            FFProcess process = new FFProcess(FFProcess.DX11_DEFAUL_NAME, true);
            DiscordPresence presence = new DiscordPresence(DISCORD_CLIENT_ID);

            Console.WriteLine("Press ESC to stop");
            do
            {
                while (!Console.KeyAvailable)
                {
                    while (Scanner.Instance.IsScanning)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine("Scanning...");
                    }

                    ActorItem player = ActorItem.CurrentUser;

                    presence.playerName = player.Name;
                    presence.lvl = player.Level.ToString();
                    presence.job = player.Job.ToString();

                    uint mapID = player.MapTerritory;
                    MapItem zone = ZoneLookup.GetZoneInfo(mapID);
                    presence.place = zone.Name.English;

                    presence.UpdatePresence();
                    Thread.Sleep(5000);
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            presence.Deinitialize();
        }
    }
}
