using System;
using System.Collections;
using UnityEngine;

public class OTPTest : MonoBehaviour
{
    Texture2D QR;
    OTPAuthenticator otp;
	
    string _identity, _newidentity, _secret, _newsecret, _oneTimePassword, _qrCodeUrl;
	string _secondsToGo, _timestamp, _hmac1, _hmac2, _hmac3;

    void Start()
    {
        otp = gameObject.AddComponent<OTPAuthenticator>();
        otp.OnPropertyChanged += new OTPAuthenticator.PropertyChanged(otp_OnPropertyChanged);
		
		Debug.LogWarning("Change Identity");
        otp.Identity = "useremail@somehost.com";	
    }
	
	void OnApplicationQuit()
	{
		Destroy(otp);
	}
	
    void otp_OnPropertyChanged(string PropertyName, string Value)
    {
		//basic stuff
        if (PropertyName == "Identity") { 
			_identity = Value; 
			Debug.Log("Identity changed to " + _identity);
			_secret = Md5Sum(_identity).Substring(1, 20);
			otp.Secret = otp.StringToBytes(_secret);
		}
        if (PropertyName == "Secret") { _secret = Value; Debug.Log("Secret changed to " + _secret); }
        if (PropertyName == "OneTimePassword") { _oneTimePassword = Value; Debug.Log("OneTimePassword changed to " + _oneTimePassword); }
        if (PropertyName == "QRCodeUrl") { _qrCodeUrl = Value; Debug.Log("QRCodeUrl changed to " + _qrCodeUrl); GetQR(); }
		
		//misc info stuff
		if (PropertyName == "SecondsToGo") { _secondsToGo = Value; /*Debug.Log("SecondsToGo changed to " + _secondsToGo);*/ }
        if (PropertyName == "Timestamp") { _timestamp = Value; Debug.Log("Timestamp changed to " + _timestamp); }
        if (PropertyName == "HmacPart1") { _hmac1 = Value; /*Debug.Log("HmacPart1 changed to " + _hmac1);*/ }
        if (PropertyName == "HmacPart2") { _hmac2 = Value; /*Debug.Log("HmacPart2 changed to " + _hmac2);*/ }
        if (PropertyName == "HmacPart3") { _hmac3 = Value; /*Debug.Log("HmacPart3 changed to " + _hmac3);*/ }
    }
	
	Rect window = new Rect(10, 10, 690, 250);
    void OnGUI()
    {
		window = GUI.Window(0, window, DrawOTPGUI, "OTP Authenticator " + DateTime.Now.ToString("HH:mm:ss")); 
	}
	
	private void DrawOTPGUI(int id)
	{
        GUI.Label(new Rect(20, 30, 110, 20), "Identity");
        _newidentity = GUI.TextField(new Rect(140, 30, 320, 20), _identity);
		if (_newidentity != _identity) otp.Identity = _newidentity;
		
        GUI.Label(new Rect(20, 60, 110, 20), "Secret");
        _newsecret = GUI.TextField(new Rect(140, 60, 320, 20), _secret);
		if (_newsecret != _secret) otp.Secret = otp.StringToBytes(_newsecret);
		
        GUI.Label(new Rect(20, 90, 110, 20), "OneTimePassword");
        GUI.TextField(new Rect(140, 90, 320, 20), _oneTimePassword);

        GUI.Label(new Rect(20, 120, 110, 20), "QRCodeUrl");
        GUI.TextField(new Rect(140, 120, 320, 20), _qrCodeUrl);

        GUI.Label(new Rect(20, 150, 110, 20), "ValidTime");
        GUI.TextField(new Rect(140, 150, 320, 20), _secondsToGo != null ? _secondsToGo : "{null}");

        GUI.Label(new Rect(20, 180, 110, 20), "Timestamp");
        GUI.TextField(new Rect(140, 180, 320, 20), _timestamp);

        GUI.Label(new Rect(20, 210, 110, 20), "Hmac");
        GUI.TextField(new Rect(140, 210, 320, 20), _hmac1 + "|" + _hmac2 + "|" + _hmac3);
		
		//if (QR == null) { Debug.LogWarning("GET QR"); GetQR(); }
		if (QR != null) GUI.DrawTexture(new Rect(470, 30, 200, 200), QR);
    }
	
	private string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		
		return hashString.PadLeft(32, '0');
	}
	
	private void GetQR()
    {
		try 
		{
			StartCoroutine(DownloadQR(_qrCodeUrl));
		}
        catch (Exception e) 
		{
			Debug.LogError("GET QR: " + e.Message);    
        }
    }
	
	private IEnumerator DownloadQR(string url)
	{
		WWW www = new WWW(url); 

		Debug.Log("DL Start");
		while(!www.isDone)
		{
			yield return www; 
		}

		Debug.Log("DL Done");
		if (www.isDone)
		{
			QR = www.texture;
			Debug.Log("DL OK - " + QR.ToString());
		}
		else Debug.LogError  ("DL FAIL");
		
		yield break; 
	}
}

