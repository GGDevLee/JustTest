/****************************************************
    文件：Log.cs
	作者：夜猫工作室(Catson)
    邮箱: 419731519@qq.com
    日期：2021/3/4 10:37:55
	功能：日志
*****************************************************/
using DG.Tweening;
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



        public static void Init()
        {
            DOTween.Init();

            if (IOUtils.HasFile(Application.persistentDataPath + "/Debug"))
            {
                GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Reporter"));

                isWriteLog = true;
                Debug.unityLogger.logEnabled = true;
                _LogCb = Debug.Log;
                _LogObjCb = Debug.Log;
                _LogWCb = Debug.LogWarning;
                _LogWObjCb = Debug.LogWarning;
                _LogECb = Debug.LogError;
                _LogEObjCb = Debug.LogError;

                _LogPath = Application.persistentDataPath + "/Log" + string.Format("_{0:yyyy_MM_dd_HH_mm}.txt", DateTime.Now);
                IOUtils.HasDirectory(_LogPath);
                IOUtils.WriteFile(_LogPath, Encoding.UTF8.GetBytes("日志开始...\n"));
                Application.logMessageReceived += LogCallback;
            }
            else
            {
                isWriteLog = false;
                Debug.unityLogger.logEnabled = false;
            }

#if UNITY_EDITOR
            //编辑器模式，默认开启日志
            Debug.unityLogger.logEnabled = true;
            _LogCb = Debug.Log;
            _LogObjCb = Debug.Log;
            _LogWCb = Debug.LogWarning;
            _LogWObjCb = Debug.LogWarning;
            _LogECb = Debug.LogError;
            _LogEObjCb = Debug.LogError;
#endif
        }


        #region Tools
        public static void D(System.Object msg)
        {
            string log = _LogMode1 + "<<---  " + msg + "  --->>  " + DateTime.Now.ToLongTimeString().ToString() + _LogEnd;
#if UNITY_EDITOR
            Debug.Log(log);
#else
           _LogCb?.Invoke(log);
#endif
        }

        public static void D(System.Object msg, UnityEngine.Object obj)
        {
            string log = _LogMode1 + "<<---  " + msg + "  --->>  " + DateTime.Now.ToLongTimeString().ToString() + _LogEnd;
#if UNITY_EDITOR
            Debug.Log(log, obj);
#else
            _LogObjCb?.Invoke(log, obj);
#endif
        }

        public static void DFat(string msg, params object[] value)
        {
            msg = string.Format(msg, value);
            D(msg);
        }


        public static void W(System.Object msg)
        {
            string log = _LogMode2 + "<<---  " + msg + "  --->>  " + DateTime.Now.ToLongTimeString().ToString() + _LogEnd;
#if UNITY_EDITOR
            Debug.LogWarning(log);
#else
            _LogWCb?.Invoke(log);
#endif
        }

        public static void W(System.Object msg, UnityEngine.Object obj)
        {
            string log = _LogMode2 + "<<---  " + msg + "  --->>  " + DateTime.Now.ToLongTimeString().ToString() + _LogEnd;
#if UNITY_EDITOR
            Debug.LogWarning(log, obj);
#else
            _LogWObjCb?.Invoke(log, obj);
#endif
        }

        public static void WFat(string msg, params object[] value)
        {
            msg = string.Format(msg, value);
            W(msg);
        }


        public static void E(System.Object msg)
        {
            string log = _LogMode3 + "<<---  " + msg + "  --->>  " + DateTime.Now.ToLongTimeString().ToString() + _LogEnd;
#if UNITY_EDITOR
            Debug.LogError(log);
#else
            _LogECb?.Invoke(log);
#endif
        }

        public static void E(System.Object msg, UnityEngine.Object obj)
        {
            string log = _LogMode3 + "<<---  " + msg + "  --->>  " + DateTime.Now.ToLongTimeString().ToString() + _LogEnd;
#if UNITY_EDITOR
            Debug.LogError(log, obj);
#else
            _LogEObjCb?.Invoke(log, obj);
#endif
        }

        public static void EFat(string msg, params object[] value)
        {
            msg = string.Format(msg, value);
            E(msg);
        }
        #endregion


        /// <summary>
        /// 清空所有日志
        /// </summary>
        public static void ClearAllLog()
        {
            strAllLog = string.Empty;
        }

        /// <summary>
        /// 日志监听
        /// </summary>
        public static void LogCallback(string condition, string stackTrace, LogType type)
        {
            string tmpStr = condition;
            switch (type)
            {
                case LogType.Log:
                    {
                        strAllLog += tmpStr + "\n";
                    }
                    break;
                case LogType.Error:
                    {
                        strAllLog += tmpStr + "\n";
                    }
                    break;
                case LogType.Assert:
                    {
                        strAllLog += "断言：" + tmpStr + "\n";
                    }
                    break;
                case LogType.Warning:
                    {
                        strAllLog += tmpStr + "\n";
                    }
                    break;
                case LogType.Exception:
                    {
                        strAllLog += tmpStr + "  :  " + stackTrace + "\n";
                    }
                    break;
            }
            if (isWriteLog)
            {
                condition = condition.Replace(_LogMode1, "");
                condition = condition.Replace(_LogMode2, "");
                condition = condition.Replace(_LogMode3, "");
                condition = condition.Replace(_LogEnd, "");
                File.AppendAllText(_LogPath, condition + "\r\n", Encoding.UTF8);
            }
        }

        public static void CreateDeubgFile()
        {
            IOUtils.CreateFile(Application.persistentDataPath + "/Debug", null);
        }
    }
}