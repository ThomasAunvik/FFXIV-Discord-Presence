using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Sharlayan;
using Sharlayan.Models;
using Sharlayan.Core;

namespace FFXIV_DiscordPresence.FFXIV
{
    public class FFProcess
    {
        public const string DX11_DEFAUL_NAME = "ffxiv_dx11";
        public const string DX9_DEFAUL_NAME = "ffxiv";

        public FFProcess Instance;

        private Process gameProcess;
        private ProcessModel gameProcessModel;

        private string processName;
        private string gameLanguage;
        private bool useLocalCache;
        private string patchVersion;

        public FFProcess(string processName = "ffxiv", bool isDX11 = false, string gameLanguage = "English", bool useLocalCache = true, string patchVersion = "latest")
        {
            Instance = this;

            Process[] processes = Process.GetProcessesByName(processName);
            if (processes.Length == 1)
            {
                Process process = processes[0];
                ProcessModel processModel = new ProcessModel()
                {
                    Process = process,
                    IsWin64 = isDX11
                };
                SetProcess(processModel, gameLanguage, patchVersion, useLocalCache);
                return;
            }

            Console.WriteLine("Missing " + ", Ending...");
            Environment.Exit(-1);

        }

        void SetProcess(ProcessModel processModel, string gameLanguage, string patchVersion, bool useLocalCache)
        {
            gameProcess = processModel.Process;
            gameProcessModel = processModel;

            this.processName = gameProcess.ProcessName;
            this.gameLanguage = gameLanguage;
            this.patchVersion = patchVersion;
            this.useLocalCache = useLocalCache;

            MemoryHandler.Instance.SetProcess(processModel, gameLanguage, patchVersion, useLocalCache);
        }
    }
}
