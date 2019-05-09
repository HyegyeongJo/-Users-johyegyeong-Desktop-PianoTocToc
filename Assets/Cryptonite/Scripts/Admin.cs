// Cryptonite™ Adm// Cryptonite™
// copyright 2019 by Envisible, Inc., All rights reserved.

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BestHTTP;


namespace Cryptonite 
{
    public class Admin : MonoBehaviour
    {
        [HideInInspector]
        public Appery db;

        InputField status;
        Transform register;

        string this_device_code;
        void Start()
        {
            this_device_code = UUID.Get().ToUpper();
            transform.Find("Device Info").GetComponent<Text>().text = "본 기기의 코드 : " + this_device_code + "\n현재 앱 이름 : " + Application.productName;
            register = transform.Find("Register Input");
            register.Find("Device Code").GetComponent<InputField>().text = this_device_code;
            register.Find("App Name").GetComponent<InputField>().text = Application.productName;
            transform.Find("App Check").Find("App Name").GetComponent<InputField>().text = Application.productName;
            transform.Find("Code Check").Find("Device Code").GetComponent<InputField>().text = this_device_code;
            status = transform.Find("Status").Find("Status").GetComponent<InputField>();
            //db = new Appery();
            //db.SystemLogin();
        }

        public void CheckCode(InputField s) { CheckCode(s.text); }
        public void CheckProduct(InputField s) { CheckProduct(s.text); }

        string Condition()
        {
            string code = register.Find("Device Code").GetComponent<InputField>().text;
            string product = register.Find("App Name").GetComponent<InputField>().text;
            return "{\"$and\":[{\"device\":\"" + code + "\"},{\"product\":\"" + product + "\"}]}"; 
        }

        void CheckCode(string s)
        {
            ShowStatus("코드 : " + s);
            db.Query("https://api.appery.io/rest/1/db/collections/licenses?where={\"device\":\"" + s + "\"}", "", HTTPMethods.Get, OnCheck);
            status.text = "querying...";
        }

        void CheckProduct(string s)
        {
            ShowStatus("앱 이름 : " + s);
            db.Query("https://api.appery.io/rest/1/db/collections/licenses?where={\"product\":\"" + s + "\"}", "", HTTPMethods.Get, OnCheck);
            status.text = "querying...";
        }

        void OnCheck(HTTPRequest rq, HTTPResponse rs) 
		{
			try 
			{
				JSONObject log = new JSONObject (rs.DataAsText.ToString());
				if(log.IsArray && log.Count > 0)
				{
					status.text = "";
					for(int i=0; i<log.Count; i++)
					{
						status.text += "번호 " + i + " : ";
                        if (log[i].HasField("device")) status.text += Cleanse(log[i].GetField("device").ToString()) + " / ";
                        if (log[i].HasField("product")) status.text += Cleanse(log[i].GetField("product").ToString()) + " / ";
                        if (log[i].HasField("info")) status.text += Cleanse(log[i].GetField("info").ToString());
						DateTime dt = Convert.ToDateTime(Cleanse(log[i].GetField("_createdAt").ToString()));
                        if(log[i].HasField("_createdAt")) status.text += " @ " + dt.Year.ToString() + "년" + dt.Month + "월" + dt.Day + "일" + dt.ToLongTimeString() + "\n";
					}
				}
				else
					status.text = "no data";
			} 
			catch (Exception e) 
			{ 
				print (e.ToString ());
			}
		}

		public void Register()
		{
            ShowStatus("신규 등록");
            db.Query ("https://api.appery.io/rest/1/db/collections/licenses?where=" + Condition(), "", HTTPMethods.Get, OnRegister); 
			status.text = "querying...";
		}

		void OnRegister(HTTPRequest rq, HTTPResponse rs) 
		{
			try 
			{
                JSONObject log = new JSONObject (rs.DataAsText.ToString());
				if(log.IsArray && log.Count > 0)
				{
					string info = "";
					if(log[0].HasField("info"))
						info = Cleanse(log[0].GetField("info").ToString());
					status.text = "Already registered" + (info != "" ? "to [" + info + "]" : ".");
				}
				else
				{
                    string code = transform.Find ("Register Input").Find("Device Code").GetComponent<InputField>().text;
                    string info = transform.Find ("Register Input").Find("Device Info").GetComponent<InputField>().text;
                    string product = transform.Find ("Register Input").Find("App Name").GetComponent<InputField> ().text;

					db.Query("https://api.appery.io/rest/1/db/collections/licenses", 
						"{\"device\":\"" + code + 
						"\",\"info\":\"" + info + 
						"\",\"product\":\"" + product + 
                        "\"}", HTTPMethods.Post, OnSubmit);
					
                    status.text = code + " / " + product + " is registered.";
				}
			} 
			catch (Exception e) 
			{ 
				print (e.ToString ());
			}
		}

        public void OnSubmit(HTTPRequest rq, HTTPResponse rs) 
        {
            print(rs.DataAsText.ToString());
        }

		public void Deregister()
		{
            ShowStatus("등록 해제");
            db.Query ("https://api.appery.io/rest/1/db/collections/licenses?where=" + Condition(), "", HTTPMethods.Get, DeregisterCheck); 
			status.text = "querying...";
		}
			
		void DeregisterCheck(HTTPRequest rq, HTTPResponse rs)
		{
            print(rs.DataAsText.ToString());
			try 
			{
				JSONObject log = new JSONObject (rs.DataAsText.ToString());
				if(log.IsArray && log.Count > 0)
				{
					string _id = Cleanse(log[0].GetField("_id").ToString());
					db.Query ("https://api.appery.io/rest/1/db/collections/licenses/" + _id, "", HTTPMethods.Delete, OnDeregister); 
				}
				else
					status.text = "Delete failed.";
			} 
			catch (Exception e) 
			{ 
				print (e.ToString ());
			}
		}

		void OnDeregister(HTTPRequest rq, HTTPResponse rs)
		{
			if (rs.DataAsText.ToString () == "{}")
                status.text = 
                    transform.Find ("Register Input").Find("Device Code").GetComponent<InputField>().text + " / " +
                    transform.Find ("Register Input").Find("App Name").GetComponent<InputField>().text + " is deregistered.";
		}

		void ShowStatus(string s){
			transform.Find ("Status").gameObject.SetActive(true);
			transform.Find ("Status").Find("Device Code").GetComponent<Text>().text = s;
		}

		public void Hide (GameObject go)
		{
			go.SetActive (false);
		}

		string Cleanse(string s) 
		{ 
			return s.Replace("\"",""); 
		}

	}
}
