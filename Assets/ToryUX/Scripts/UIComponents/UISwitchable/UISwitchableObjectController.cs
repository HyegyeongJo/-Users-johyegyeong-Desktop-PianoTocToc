using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ToryUX
{
    public class UISwitchableObjectController : MonoBehaviour
    {
        public static List<UISwitchableObjectController> allSwitchableControllers;

        [Header("Switch objects")]
        public List<ShowAndHideAnimationPlayer> uiObjects;
        public bool showFirstObjectOnAwake = true;

        List<ShowAndHideAnimationPlayer> prevObjectStack;

        [Header("Loop objects")]
        public List<ShowAndHideAnimationPlayer> loopObjects;

        ShowAndHideAnimationPlayer currentObject;

        public float loopDuration = 3f;
        public bool loopObjectOnAwake = false;

        void Awake()
        {
            if (allSwitchableControllers == null)
            {
                allSwitchableControllers = new List<UISwitchableObjectController>();
            }
            if (!allSwitchableControllers.Contains(this))
            {
                allSwitchableControllers.Add(this);
            }

            if (uiObjects == null || uiObjects.Count < 1)
            {
                uiObjects = new List<ShowAndHideAnimationPlayer>();
                foreach (TagClasses.UISwitchableObject o in GetComponentsInChildren<TagClasses.UISwitchableObject>(true))
                {
                    if (o.transform.parent == this.transform)
                    {
                        uiObjects.Add(o.GetComponent<ShowAndHideAnimationPlayer>());
                    }
                }
            }

            prevObjectStack = new List<ShowAndHideAnimationPlayer>();
            loopObjects = new List<ShowAndHideAnimationPlayer>();
        }

        void Start()
        {
            Reset();

            if (loopObjectOnAwake && loopObjects.Count > 0)
            {
                UpdateLoopContext();
            }
        }

        public void Show(ShowAndHideAnimationPlayer switchable)
        {
            ClearCoroutine(showObjectCoroutine);
            ClearCoroutine(rollbackObjectCoroutine);
            ClearCoroutine(loopCoroutine);

            if (gameObject.activeInHierarchy && enabled)
            {
                showObjectCoroutine = StartCoroutine(ShowCoroutine(switchable));
            }
        }

        public void Show(int index)
        {
            if (index < uiObjects.Count)
            {
                Show(uiObjects[index]);
            }
        }

        public void Rollback(ShowAndHideAnimationPlayer switchable, float rollbackDuration = 3f)
        {
            ClearCoroutine(rollbackObjectCoroutine);
            ClearCoroutine(loopCoroutine);
            if (gameObject.activeInHierarchy && enabled)
            {
                rollbackObjectCoroutine = StartCoroutine(RollbackCoroutine(switchable, rollbackDuration));
            }
        }

        public void Rollback(int index, float rollbackDuration = 3f)
        {
            if (index < uiObjects.Count)
            {
                Rollback(uiObjects[index], rollbackDuration);
            }
        }

        public void ShowAndRollback(int index, int rollbackIndex, float rollbackDuration = 3f)
        {
            Show(index);
            Rollback(rollbackIndex, rollbackDuration);
        }

        public void ShowAndRollback(ShowAndHideAnimationPlayer switchable, ShowAndHideAnimationPlayer rollbackSwitchable, float rollbackDuration = 3f)
        {
            Show(switchable);
            Rollback(rollbackSwitchable, rollbackDuration);
        }

        public void ShowPrevObject()
        {
            if (currentObject != null && prevObjectStack.Count > 0)
            {
                ClearCoroutine(showObjectCoroutine);
                ClearCoroutine(rollbackObjectCoroutine);
                ClearCoroutine(loopCoroutine);

                if (gameObject.activeInHierarchy && enabled)
                {
                    showObjectCoroutine = StartCoroutine(ShowCoroutine(prevObjectStack.Last(), true));
                }
            }
        }

        public void ShowNextObject()
        {
            if (currentObject != null)
            {
                int index = uiObjects.IndexOf(currentObject);

                if (index < uiObjects.Count - 1)
                {
                    Show(index + 1);
                }
            }
        }

        #region Show and Rollback Coroutines

        Coroutine showObjectCoroutine;
        IEnumerator ShowCoroutine(ShowAndHideAnimationPlayer switchable, bool isShowPrev = false)
        {
            foreach (ShowAndHideAnimationPlayer p in uiObjects)
            {
                if (p.gameObject.activeInHierarchy)
                {
                    p.PlayHideAnimation();
                }
            }
            yield return new WaitForSeconds(.1f);

            if (isShowPrev)
            {
                prevObjectStack.RemoveAt(prevObjectStack.Count - 1);
            }
            else
            {
                prevObjectStack.Add(currentObject);
            }

            switchable.gameObject.SetActive(true);
            switchable.PlayShowAnimation();
            currentObject = switchable;

            showObjectCoroutine = null;
        }

        public Coroutine rollbackObjectCoroutine;
        IEnumerator RollbackCoroutine(ShowAndHideAnimationPlayer switchable, float rollbackDuration)
        {
            yield return new WaitForSeconds(rollbackDuration);
            Show(switchable);
            rollbackObjectCoroutine = null;
        }

        public void HideAll()
        {
            foreach (ShowAndHideAnimationPlayer p in uiObjects)
            {
                if (p.gameObject.activeInHierarchy)
                {
                    p.PlayHideAnimation();
                }
            }
            currentObject = null;
        }

        #endregion

        /// <summary>
        /// Clear existing looping switchables, then register new switchable object for loop manually.
        /// </summary>
        /// <param name="switchables"></param>
        public void Loop(ShowAndHideAnimationPlayer firstSwitchable, params ShowAndHideAnimationPlayer[] switchables)
        {
            loopObjects.Clear();
            loopObjects = switchables.ToList();
        }

        public void AddLoopObject(ShowAndHideAnimationPlayer loopObject)
        {
            loopObjects.Add(loopObject);
            UpdateLoopContext();
        }

        public void AddLoopObjectAtFirst(ShowAndHideAnimationPlayer loopObject)
        {
            loopObjects.Insert(0, loopObject);
            UpdateLoopContext();
        }

        public void RemoveLoopObject(ShowAndHideAnimationPlayer loopObject)
        {
            if (loopObjects.Contains(loopObject))
            {
                loopObjects.Remove(loopObject);
                UpdateLoopContext();
            }
        }

        public void ShowLoopObject(ShowAndHideAnimationPlayer switchable)
        {
            ClearCoroutine(showObjectCoroutine);
            ClearCoroutine(rollbackObjectCoroutine);

            if (gameObject.activeInHierarchy && enabled)
            {
                showObjectCoroutine = StartCoroutine(ShowCoroutine(switchable));
            }
        }

        public void UpdateLoopContext()
        {
            ClearCoroutine(showObjectCoroutine);
            ClearCoroutine(rollbackObjectCoroutine);

            if (gameObject.activeInHierarchy && enabled && loopCoroutine == null)
            {
                loopCoroutine = StartCoroutine(LoopCoroutine());
            }
        }

        Coroutine loopCoroutine;
        IEnumerator LoopCoroutine()
        {
            int index = -1;
            while (true)
            {
                if (loopObjects.Count < 1)
                {
                    Reset();
                    loopCoroutine = null;
                    yield break;
                }
                else if (loopObjects.Count == 1)
                {
                    Show(loopObjects[0]);
                    loopCoroutine = null;
                    yield break;
                }
                else
                {
                    index = (index + 1) % loopObjects.Count;
                    ShowLoopObject(loopObjects[index]);
                    yield return new WaitForSeconds(Mathf.Max(.5f, loopDuration));
                }
            }
        }

        void ClearCoroutine(Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        void OnEnable()
        {
            if (loopObjectOnAwake && loopObjects.Count > 0)
            {
                UpdateLoopContext();
            }
        }

        void OnDisable()
        {
            ClearCoroutine(showObjectCoroutine);
            ClearCoroutine(rollbackObjectCoroutine);
            ClearCoroutine(loopCoroutine);
        }

        public void Reset()
        {
            ClearCoroutine(showObjectCoroutine);
            ClearCoroutine(rollbackObjectCoroutine);
            ClearCoroutine(loopCoroutine);

            prevObjectStack.Clear();

            foreach (ShowAndHideAnimationPlayer p in uiObjects)
            {
                p.gameObject.SetActive(false);
            }
            currentObject = null;

            if (showFirstObjectOnAwake && uiObjects.Count > 0)
            {
                uiObjects[0].gameObject.SetActive(true);
                currentObject = uiObjects[0];
            }
        }
    }
}