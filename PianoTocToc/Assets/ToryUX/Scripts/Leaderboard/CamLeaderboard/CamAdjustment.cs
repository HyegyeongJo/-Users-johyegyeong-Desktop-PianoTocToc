using System.Collections;
using System.Collections.Generic;
using ToryValue;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToryUX
{
    public class CamAdjustment : MonoBehaviour
    {
        #region Singleton
        static volatile CamAdjustment instance;
        static readonly object syncRoot = new object();

        public static CamAdjustment Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = FindObjectOfType<CamAdjustment>();

                            if (instance == null)
                            {
                                foreach (CamAdjustment c in Resources.FindObjectsOfTypeAll<CamAdjustment>())
                                {
                                    #if UNITY_EDITOR
                                    if (EditorUtility.IsPersistent(c.transform.root.gameObject))
                                    {
                                        continue;
                                    }
                                    #endif

                                    instance = c;
                                    break;
                                }
                            }
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        void Awake()
        {
            #region Create Singleton Instance
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
            #endregion

            LoadSettings();
        }

        public void LoadSettings()
        {
            ZoomLevel.LoadSavedValue();
            OffsetX.LoadSavedValue();
            OffsetY.LoadSavedValue();
            RotateAxis.LoadSavedValue();
            IsMirrored.LoadSavedValue();
            CameraIndex.LoadSavedValue();
        }

        [Header("Camera Settings")]

        [SerializeField]
        ToryFloat zoomLevel;
        public ToryFloat ZoomLevel
        {
            get
            {
                return Instance.zoomLevel;
            }
            set
            {
                Instance.zoomLevel = value;
            }
        }

        [SerializeField]
        ToryFloat offsetX;
        public ToryFloat OffsetX
        {
            get
            {
                return Instance.offsetX;
            }
            set
            {
                Instance.offsetX = value;
            }
        }

        [SerializeField]
        ToryFloat offsetY;
        public ToryFloat OffsetY
        {
            get
            {
                return Instance.offsetY;
            }
            set
            {
                Instance.offsetY = value;
            }
        }

        [SerializeField]
        ToryInt rotateAxis;
        public ToryInt RotateAxis
        {
            get
            {
                return Instance.rotateAxis;
            }
            set
            {
                Instance.rotateAxis = value;
            }
        }

        [SerializeField]
        ToryBool isMirrored;
        public ToryBool IsMirrored
        {
            get
            {
                return Instance.isMirrored;
            }
            set
            {
                Instance.isMirrored = value;
            }
        }

        [SerializeField]
        ToryInt cameraIndex;
        public ToryInt CameraIndex
        {
            get
            {
                return Instance.cameraIndex;
            }
            set
            {
                Instance.cameraIndex = value;
            }
        }
    }
}