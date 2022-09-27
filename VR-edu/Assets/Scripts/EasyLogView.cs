using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
namespace EasyLogView
{
    [Serializable]
    public class LogModel
    {
        public string logInfo;
        public string logStackTrace;
        public LogType logType;
        public DateTime logTime;
        public LogModel(string logInfo, string logStackTrace, LogType logType, DateTime logTime)
        {
            this.logInfo = logInfo;
            this.logStackTrace = logStackTrace;
            this.logType = logType;
            this.logTime = logTime;
        }
        public string GetTime()
        {
            return logTime.ToString("hh:mm:ss");
        }
    }
    public class EasyLogView : MonoBehaviour
    {
        public List<LogModel> allLog = new List<LogModel>();
        public Transform content;
        public GameObject infoItem;
        private void Start()
        {
            Application.logMessageReceived += Log;
        }
        private void OnDestroy()
        {
            Application.logMessageReceived -= Log;
        }
        private void Log(string msg, string stackTrace, LogType type)
        {
            LogModel log = new LogModel(msg, stackTrace, type, DateTime.Now);
            allLog.Add(log);
            RefreshUI();
        }
        public void RefreshUI()
        {
            foreach (Transform t in content)
            {
                t.gameObject.SetActive(true);
            }
            if (content.childCount < allLog.Count)
            {
                var createCount = allLog.Count - content.childCount;
                for (int i = 0; i < createCount; i++)
                {
                    var item = GameObject.Instantiate(infoItem);
                    item.transform.SetParent(content);
                    item.transform.localPosition = Vector3.zero;
                }
            }
            else if (content.childCount > allLog.Count)
            {
                for (int i = allLog.Count; i < content.childCount; i++)
                {
                    content.GetChild(i).gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < allLog.Count; i++)
            {
                var itemInfo = content.GetChild(i).GetComponent<EasyLogInfoItem>();
                itemInfo.FlushUI(allLog[i]);
            }
        }
        public void ClearLog()
        {
            allLog.Clear();
            foreach (Transform t in content)
            {
                t.gameObject.SetActive(false);
            }
        }
    }
    public class EasyLogInfoItem : MonoBehaviour
    {
        public Text textLogType;
        public Text textLogInfo;
        public Text textLogStack;
        private LogModel model;
        public void FlushUI(LogModel model)
        {
            this.model = model;
            textLogType.text = model.logType.ToString();
            textLogType.color = GetColor();
            textLogInfo.text = "[" + model.GetTime() + "] " + model.logInfo;
            textLogStack.text = model.logStackTrace;
        }
        private Color GetColor()
        {
            if (model.logType == LogType.Log)
            {
                return Color.white;
            }
            else if (model.logType == LogType.Warning)
            {
                return Color.yellow;
            }
            else if (model.logType == LogType.Assert)
            {
                return Color.yellow;
            }
            else if (model.logType == LogType.Error)
            {
                return Color.red;
            }
            else if (model.logType == LogType.Exception)
            {
                return Color.red;
            }
            return Color.white;
        }
    }
}
