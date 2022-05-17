/****************************************************
    文件：Log.cs
	作者：夜猫工作室(Catson)
    邮箱: 419731519@qq.com
    日期：2021/3/4 10:37:55
	功能：日志
*****************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace TestLeeFramework
{
    public class Log
    {
        public static bool isWriteLog = false;
        /// <summary>
        /// 整理全部log
        /// </summary>
        public static string strAllLog = string.Empty;


        private static Action<string> _LogCb = null;
        private static Action<string, UnityEngine.Object> _LogObjCb = null;
        private static Action<string> _LogWCb = null;
        private static Action<string, UnityEngine.Object> _LogWObjCb = null;
        private static Action<string> _LogECb = null;
        private static Action<string, UnityEngine.Object> _LogEObjCb = null;

        private static string _LogMode1 = "<size=13><b><color=white>";
        private static string _LogMode2 = "<size=13><b><color=yellow>";
        private static string _LogMode3 = "<size=13><b><color=red>";
        private static string _LogEnd = "</color></b></size>";
        private static string _LogPath;



        public static void D(System.Object msg)
        {
            string log = _LogMode1 + "<<---  " + msg + "  --->>  " + DateTime.Now.ToLongTimeString().ToString() + _LogEnd;
#if UNITY_EDITOR
            Debug.Log(log);
#else
           _LogCb?.Invoke(log);
#endif
        }
        
    }
}