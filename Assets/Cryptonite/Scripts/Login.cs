// Cryptonite™ Admin
// copyright 2017 by Envisible, Inc., All rights reserved.
// Updated on 11th of November, 2017

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BestHTTP;


namespace Cryptonite 
{
    public class Login : MonoBehaviour
    {
        Appery db;
        public GameObject Admin;

        public void LogIn(InputField pass)
        {
            db = new Appery();
            db.LogIn("user", pass.text, OnLogIn);
        }


        void OnLogIn(HTTPRequest rq, HTTPResponse rs)
        {
            try
            {
                JSONObject json = new JSONObject(rs.DataAsText.ToString());
                if (Cleanse(json.GetField("sessionToken").ToString()).Length > 0)
                {
                    Admin.SetActive(true);
                    db.session_token = Cleanse(json.GetField("sessionToken").ToString());
                    Admin.GetComponent<Admin>().db = db;
                    this.gameObject.SetActive(false);
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        string Cleanse(string s)
        {
            return s.Replace("\"", "");
        }
	}
}
