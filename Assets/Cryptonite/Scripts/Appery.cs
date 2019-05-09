// Appery Script for Cryptonite™
// copyright 2019 by Envisible, Inc., All rights reserved.

using System;
using System.Text;
using BestHTTP;
using UnityEngine;

namespace Cryptonite
{
	public class Appery
	{
		public bool logged_in = false;

		string api_key = "590e07f40f0d31248df3176f";
		string id = "app";

		[HideInInspector]
		public string session_token;

		HTTPRequest request;
		DateTime time_stamp;

		public void LogIn(string i, string p, BestHTTP.OnRequestFinishedDelegate callback)
		{
			time_stamp = DateTime.UtcNow;

			request = new HTTPRequest(new Uri("https://api.appery.io/rest/1/db/login?username=" + i + "&password=" + p), HTTPMethods.Get, callback);
			request.SetHeader("X-Appery-Database-Id", api_key);
			request.Send();
		}

		public void SystemLogin()
		{
			LogIn(id, Cryptonite.secret, OnLogIn);
		}

		void OnLogIn(HTTPRequest rq, HTTPResponse rs)
		{
			try
			{
				JSONObject json = new JSONObject(rs.DataAsText.ToString());
				session_token = Cleanse(json.GetField("sessionToken").ToString());
				logged_in = session_token.Length > 0;
			}
			catch (Exception e)
			{
				Debug.Log(e.ToString());
			}
			Debug.Log(session_token);
		}

		public void LogOut()
		{
			request = new HTTPRequest(new Uri("https://api.appery.io/rest/1/db/logout"), HTTPMethods.Get, OnLogOut);
			request.SetHeader("X-Appery-Database-Id", api_key);
			request.SetHeader("X-Appery-Session-Token", session_token);
			request.Send();
		}

		void OnLogOut(HTTPRequest rq, HTTPResponse rs)
		{
			logged_in = false;
		}

		public void Query(string uri, string data, HTTPMethods http_method, OnRequestFinishedDelegate call_back)
		{
			request = new HTTPRequest(new Uri(uri), http_method, call_back);
			request.SetHeader("Content-Type", "application/json");
			request.SetHeader("X-Appery-Database-Id", api_key);
			request.SetHeader("X-Appery-Session-Token", session_token);

			if (http_method == HTTPMethods.Post && data.Length > 0)
				request.RawData = Encoding.UTF8.GetBytes(data);

			request.Send();
		}

		void OnApplicationQuit()
		{
			LogOut();
		}

		string Cleanse(string s)
		{
			return s.Replace("\"", "");
		}
	}
}