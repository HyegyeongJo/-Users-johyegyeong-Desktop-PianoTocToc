using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ToryUX
{
    public class ToryConsoleSetup : MonoBehaviour
    {
        public ToryConsole uiConsoleElement;
        public int maxLogItemCount = 20;

        [Space]
        public bool showStackTraceForRegular = false;
        public bool showStackTraceForWarning = false;
        public bool showStackTraceForError = false;
        public bool showStackTraceForException = true;
        public bool showStackTraceForAssert = true;

        public static bool ShowStackTraceForRegular
        {
            get
            {
                return Instance.showStackTraceForRegular;
            }
        }

        public static bool ShowStackTraceForWarning
        {
            get
            {
                return Instance.showStackTraceForWarning;
            }
        }

        public static bool ShowStackTraceForError
        {
            get
            {
                return Instance.showStackTraceForError;
            }
        }

        public static bool ShowStackTraceForException
        {
            get
            {
                return Instance.showStackTraceForException;
            }
        }

        public static bool ShowStackTraceForAssert
        {
            get
            {
                return Instance.showStackTraceForAssert;
            }
        }

        public const int DefaultFontSize = 30;
        public const int SmallerFontSize = 20;

        public static readonly Color RegularColor = Color.clear;
        public static readonly Color WarningColor = new Color(232f / 255f, 222f / 255f, 77f / 255f, 100f / 255f);
        public static readonly Color ErrorColor = new Color(200f / 255f, 100f / 255f, 100f / 255f, 100f / 255f);
        public static readonly Color ExceptionColor = new Color(200f / 255f, 100f / 255f, 100f / 255f, 200f / 255f);
        public static readonly Color AssertColor = new Color(100f / 255f, 200f / 255f, 154f / 255f, 100f / 255f);

        public static ToryConsoleSetup Instance
        {
            get
            {
                return instance;
            }
        }
        private static ToryConsoleSetup instance;

        void Awake()
        {
            // Do things only when uiConsoleElement is assigned.
            if (uiConsoleElement != null)
            {
                if (instance != null && instance != this)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    instance = this;
                }

                uiConsoleElement.Initialize(maxLogItemCount);
                Application.logMessageReceived += uiConsoleElement.HandleLog;
            }
        }

        void OnDestroy()
        {
            if (uiConsoleElement != null)
            {
                Application.logMessageReceived -= uiConsoleElement.HandleLog;

                if (instance == this)
                {
                    instance = null;
                }
            }
        }
    }
}