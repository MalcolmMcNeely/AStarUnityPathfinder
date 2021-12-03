using System;
using UnityEngine;
using System.Collections;

namespace Logging
{
    public class DebugLogger : MonoBehaviour
    {
        public int MaxMessages = 50;

        string _log;
        readonly Queue _logQueue = new Queue();

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            _log = logString;

            var newLog = $"{Environment.NewLine}[{DateTime.Now:HH:mm:ss}][{type}] : {_log}";

            Enqueue(newLog);

            if (type == LogType.Exception)
            {
                newLog = $"{Environment.NewLine}{stackTrace}";
                Enqueue(newLog);
            }

            _log = string.Empty;

            foreach (var log in _logQueue)
            {
                _log += log;
            }
        }

        void Enqueue(string newLog)
        {
            if (_logQueue.Count > MaxMessages)
            {
                _logQueue.Dequeue();
            }

            _logQueue.Enqueue(newLog);
        }

        void OnGUI()
        {
            GUILayout.Label(_log);
        }
    }
}