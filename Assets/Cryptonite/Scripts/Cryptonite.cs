// Cryptonite™
// copyright 2019 by Envisible, Inc., All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

using BestHTTP;

namespace Cryptonite
{
    public enum Type
    {
        Cryptonite = 0,         // Cryptonite Component
        Crypton = 1,            // Crypton License Server
        Evaluation = 2          // Evaluation License. Limited time.
    }

    public class Cryptonite : MonoBehaviour
    {
        #region properties
        public static String secret = "Wkdehfapd2";
        public Type type = Type.Cryptonite;              // Default Type is Cypronite Component
        public Font font;

        Appery db;
        OTPAuthenticator OTP;
        OscIn OSC_IN;
        OscOut OSC_OUT;

        int port = 6914;
        string address = "/cryptonite";
        int otp;                                         // One Time Password
        int timestamp;

        string uuid;
        bool activated = false;
        TextMesh textMesh;
        string status;

        #endregion

        #region init
        void Awake()
        {
            GameObject go = new GameObject();
            textMesh = go.AddComponent<TextMesh>();

            if (font != null)
            {
                textMesh.font = font;
                MeshRenderer mr = textMesh.GetComponentInChildren<MeshRenderer>();
                mr.material = textMesh.font.material;
            } else
                textMesh.fontStyle = FontStyle.Bold;

            textMesh.color = new Color(1f, 0.75f, 0.25f);
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.fontSize = 30;

            go.transform.position = transform.position + Vector3.forward * textMesh.fontSize;
            go.transform.parent = transform;
        }

        void Start()
        {
            if(type == Type.Evaluation)
            {
                StartCoroutine(Evaluate());
            }
            else
            {
                // Race between Activation vs Expiration
                StartCoroutine(Activate());                 // Go!
                expiration = StartCoroutine(Expire());      // Go!

                // OTP Checksum
                OTP = gameObject.AddComponent<OTPAuthenticator>();
                OTP.OnPropertyChanged += new OTPAuthenticator.PropertyChanged(OTPCallBack);
                OTP.Identity = "admin@envisible.com";
                OTP.Secret = OTP.StringToBytes(secret);

                if (type == Type.Crypton)           // Source
                {
                    OSC_OUT = gameObject.AddComponent<OscOut>();
                    OSC_OUT.Open(port);
                }
                else if (type == Type.Cryptonite)   // Checker
                {
                    OSC_IN = gameObject.AddComponent<OscIn>();
                    OSC_IN.Open(port);
                    OSC_IN.Map(address, Receive);
                }
            }
        }
        #endregion

        #region Self Destruction
        IEnumerator Evaluate()
        {
            #if UNITY_EDITOR
            EditorApplication.Beep();
            #endif

			GetComponent<Camera>().enabled = false;
			textMesh.GetComponent<MeshRenderer>().enabled = false;

			textMesh.color = new Color(1f, 0.5f, 0.25f);
            status = "Evaluation License\n";

            yield return new WaitForSeconds(3);
            DontDestroyOnLoad(this.gameObject);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        
            // Quitting in 5 minutes
            int t = 5 * 60;
            while(t-- > 0)
            {
                yield return new WaitForSeconds(1);
                Debug.Log("Evaluation license ending in " + t + "seconds.");
            }
            Application.Quit();
        }
        #endregion

        #region OTP
        void OTPCallBack(string property, string value)
        {
            try
            {
                if (property == "Timestamp") int.TryParse(value, out timestamp);
                if (property == "OneTimePassword") int.TryParse(value, out otp);
                if (property == "SecondsToGo" && type == Type.Crypton && activated) OSC_OUT.Send(address, otp);     // Broadcast if this is a Licensed Crypton Server
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }

        void Receive(OscMessage message)
        {
            if (!activated)
            {
                int p;
                if (message.TryGet(0, out p))
                    if (p == otp) StartCoroutine(Proceed());         // Proceed if my OTP matches with the broadcasted one.
            }
        }
        #endregion


        #region update
        void Update()
        {
            string[] anim = {"", ".", "..", "...", "..", "." };
            textMesh.text = status + anim[Mathf.FloorToInt(Time.timeSinceLevelLoad * 3 % anim.Length)];
        }
        #endregion


        #region RESTful
        IEnumerator Activate()
        {
            db = new Appery();

            uuid = UUID.Get().ToUpper();

            status = "code = " + uuid;
            status += "\nproduct = " + Application.productName;
            status += "\n\nconnecting to\nlicense server\n";

            activated = false;

            float timer;

            while (!db.logged_in && !activated)   // Try logging into the license DB
            {
                db.SystemLogin();
                timer = Time.timeSinceLevelLoad;

                yield return new WaitUntil(() => db.logged_in || Time.timeSinceLevelLoad > timer + 5);
            }

            while (!activated)      // Try getting activation response from the DB
            {
                CheckActivation();
                timer = Time.timeSinceLevelLoad;

                yield return new WaitUntil(() => activated || Time.timeSinceLevelLoad > timer + 5);
            }

            StartCoroutine(Proceed());  // Proceed if all above are successful
        }

        void CheckActivation()
        {
            string condition = "{\"$and\":[{\"device\":\"" + uuid + "\"},{\"product\":\"" + Application.productName + "\"}]}";
            db.Query("https://api.appery.io/rest/1/db/collections/licenses?where=" + condition, "", HTTPMethods.Get, OnCheckActivation);
        }

        void OnCheckActivation(HTTPRequest rq, HTTPResponse rs)
        {
            try
            {
                // Resets offline activation once connected to license server
                PlayerPrefsElite.SetBoolean("Activated", false);

                JSONObject log = new JSONObject(rs.DataAsText.ToString());
                if (log.IsArray && log.Count > 0)
                {
                    // Activation wins the race.
                    // Kill expiration trials.
                    StopCoroutine(expiration);
                    expiration = null;

                    PlayerPrefsElite.SetBoolean("Activated", true, 0);
                    activated = true;
                }
            }
            catch (Exception e)
            {
                print(e.ToString());
            }
        }

        Coroutine expiration;
        IEnumerator Expire()
        {
            // Cryptonite fails after 15 secs of trials.
            yield return new WaitForSeconds(15);
            
            // Crypton Server survives if and only if there was a saved activation.
            if(type == Type.Crypton)
            {
                try
                {
                    if (PlayerPrefsElite.VerifyBoolean("Activated", 0)) // Piracy check
                        if (PlayerPrefsElite.GetBoolean("Activated"))
                            activated = true;
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                }

            }

            // OK. I give up.
            if (!activated)
            {
                int trialDuration = 60;
                for (int i = 0; i < trialDuration; i++)
                {
                    yield return new WaitForSeconds(1);
                    textMesh.color = new Color(1f, 0.5f, 0.5f);
                    status = "code = " + uuid;
                    status += "\nproduct = " + Application.productName;
                    status += "\ntimestamp = " + timestamp;
                    status += "\n\nPlease check network connection.\nApplication ending in " + (trialDuration - i) + " secs.\n";
                }
                #if UNITY_EDITOR
                EditorApplication.Beep();
                #endif
                Application.Quit();
            }
        }

        IEnumerator Proceed()
        {
            #if UNITY_EDITOR
            EditorApplication.Beep();
            #endif

            textMesh.color = new Color(0.5f, 0.75f, 1f);
            status = "License Active\n";

            if (db != null) db.LogOut();
            yield return new WaitForSeconds(3);

            Destroy(HTTPUpdateDelegator.Instance.gameObject);

            if (type == Type.Cryptonite)
            {
                Destroy(this.gameObject);
                SceneManager.LoadScene(1, LoadSceneMode.Single);
            }
            else
                status = "Crypton Server Running\n";
        }

        string Cleanse(string s)
        {
            return s.Replace("\"", "");
        }
        #endregion


        #region quit
        void OnApplicatioQuit()
        {
            db.Query("https://api.appery.io/rest/1/db/collections/activities", "{\"activity\":\"quit\",\"code\":\"" + uuid + "\"\"product\":\"" + Application.productName + "\"}", HTTPMethods.Post, null);
            db.LogOut();

            OTP.StopAllCoroutines();
            OTP.CancelInvoke();
            Destroy(OTP);
        }
        #endregion
    }
}