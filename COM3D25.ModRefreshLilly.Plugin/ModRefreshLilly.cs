using BepInEx;
using BepInEx.Configuration;
using COM3D2.LillyUtill;
using COM3D2API;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace COM3D2.ModRefreshLilly.Plugin
{
    class MyAttribute
    {
        public const string PLAGIN_NAME = "ModRefreshLilly";
        public const string PLAGIN_VERSION = "22.2.23";
        public const string PLAGIN_FULL_NAME = "COM3D2.ModRefreshLilly.Plugin";
    }

    [BepInPlugin(MyAttribute.PLAGIN_FULL_NAME, MyAttribute.PLAGIN_NAME, MyAttribute.PLAGIN_VERSION)]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    //[BepInPlugin("COM3D2.Sample.Plugin", "COM3D2.Sample.Plugin", "21.6.6")]// 버전 규칙 잇음. 반드시 2~4개의 숫자구성으로 해야함. 미준수시 못읽어들임
    [BepInProcess("COM3D2x64.exe")]
    public class ModRefreshLilly : BaseUnityPlugin
    {
        public static MyLog myLog;

        public void Start()
        {
            myLog = new MyLog(MyAttribute.PLAGIN_NAME);
            myLog.LogMessage("Start");

            //SampleGUI.Install(gameObject, Config);

            SystemShortcutAPI.AddButton(MyAttribute.PLAGIN_FULL_NAME, new Action(ModRefresh0), MyAttribute.PLAGIN_NAME, MyUtill.ExtractResource(COM3D2.ModRefreshLilly.Plugin.Properties.Resources.icon));
        }

        static bool isRunModreflash = false;

        private void ModRefresh0()
        {
            if (isRunModreflash)
                return;

            isRunModreflash = true;
            Task.Factory.StartNew(() =>
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                myLog.LogDarkBlue("modreflash0. start ", string.Format("{0:0.000} ", stopwatch.Elapsed.ToString()));

                bool flag = Directory.Exists(UTY.gameProjectPath + "\\Mod\\");
                if (flag)
                {
                    GameUty.UpdateFileSystemPath();
                    GameUty.UpdateFileSystemPathOld();
                }
                bool flag3 = GameUty.FileSystemMod != null;
                if (flag3)
                {
                    typeof(GameUty).GetField("m_aryModOnlysMenuFiles").SetValue(null, Array.FindAll<string>(GameUty.FileSystemMod.GetList(string.Empty, AFileSystemBase.ListType.AllFile), (string i) => new Regex(".*\\.menu$").IsMatch(i)));
                }

                myLog.LogDarkBlue("modreflash0. end ", string.Format("{0:0.000} ", stopwatch.Elapsed.ToString()));
                isRunModreflash = false;
            }
            );
        }

    }
}
