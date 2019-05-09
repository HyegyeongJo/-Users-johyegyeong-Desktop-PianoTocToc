using System;
using System.Collections;
using UnityEngine;

public class OTPSample : MonoBehaviour
{
    Texture2D QR;
    OTPAuthenticator otp;
	
    string _identity = "", _newidentity = "", _secret = "", _newsecret = "", _oneTimePassword = "", _qrCodeUrl = "", _mypassword = "", _message = "";
		
	enum GUIState
	{
		Login, Register, Message
	}
	
	GUIState _guiState = GUIState.Login;
	
    void Start()
    {
        otp = gameObject.AddComponent<OTPAuthenticator>();
        otp.OnPropertyChanged += new OTPAuthenticator.PropertyChanged(otp_OnPropertyChanged);
			
		otp.Identity = _identity;
    }
	
	void OnApplicationQuit()
	{
		Destroy(otp);
	}
	
    void otp_OnPropertyChanged(string PropertyName, string Value)
    {
		//for login
        if (PropertyName == "Identity") 
		{ 
			_identity = Value;
			_secret = Md5Sum(_identity).Substring(1, 20);
			otp.Secret = otp.StringToBytes(_secret);
		}
		
		if (PropertyName == "OneTimePassword") { _oneTimePassword = Value; }
		
		//for registration
        if (PropertyName == "QRCodeUrl") { _qrCodeUrl = Value; GetQR(); }
    }
	
	Rect loginwindow = new Rect(10, 10, 280, 150);
	Rect registerwindow = new Rect(10, 10, 320, 450);
	Rect messagewindow = new Rect(10, 10, 280, 150);
    void OnGUI()
    {
		if (_guiState == GUIState.Login) loginwindow = GUI.Window(0, loginwindow, DrawOTPLogin, "Login " + DateTime.Now.ToString("HH:mm:ss")); 
		if (_guiState == GUIState.Register) registerwindow = GUI.Window(1, registerwindow, DrawOTPRegister, "Register " + DateTime.Now.ToString("HH:mm:ss")); 
		if (_guiState == GUIState.Message) messagewindow = GUI.Window(2, messagewindow, DrawOTPMessage, "Result " + DateTime.Now.ToString("HH:mm:ss")); 
	}
	
	private void DrawOTPLogin(int id)
	{
        GUI.Label(new Rect(20, 30, 100, 20), "User / e-mail");
        _newidentity = GUI.TextField(new Rect(100, 30, 160, 20), _identity);
		if (_newidentity != _identity) otp.Identity = _newidentity;
		
        GUI.Label(new Rect(20, 60, 100, 20), "Password");
        _mypassword = GUI.PasswordField(new Rect(100, 60, 160, 20), _mypassword, '*', 6);
		
		if (GUI.Button(new Rect(180, 90, 80, 40), "Login"))
		{
			ProcessLogin();
		}

		if (GUI.Button(new Rect(20, 90, 80, 40), "Register"))
		{
			_guiState = GUIState.Register;
		}
    }
	
	private void DrawOTPRegister(int id)
	{
        GUI.Label(new Rect(20, 30, 240, 60), "User / e-mail");
        _newidentity = GUI.TextField(new Rect(100, 30, 200, 20), _identity);
		if (_newidentity != _identity) otp.Identity = _newidentity;
		
		GUI.Label(new Rect(20, 60, 240, 60), "Your Secret");
		/*_newsecret =*/ GUI.TextField(new Rect(100, 60, 200, 20), _identity != "" ? _secret : "");
		//if (_newsecret != "" && _newsecret != _secret) otp.Secret = otp.StringToBytes(_newsecret);
		
		if (_identity != "") GUI.DrawTexture(new Rect(100, 90, 200, 200), QR);
		else GUI.TextArea(new Rect(100, 90, 200, 200), "", 0); //placeholder
		
		GUI.Label(new Rect(20, 300, 280, 80), "Enter your username or email, then scan the QR with any TOTP/MFA app (like. Google Authenticator) or manually enter code (secret) into app/device.");
		
		if (GUI.Button(new Rect(220, 390, 80, 40), "Done"))
		{
			_guiState = GUIState.Login;
		}
    }
	
	private void DrawOTPMessage(int id)
	{
        GUI.Label(new Rect(20, 30, 240, 60), _message);
		
		if (GUI.Button(new Rect(180, 90, 80, 40), "Ok"))
		{
			_guiState = GUIState.Login;
		}
    }
	
	private void ProcessLogin()
	{
		if (_identity != "" && _mypassword == _oneTimePassword)
			_message = "Authentication SUCCEDED";
		else
			_message = "Authentication FAILED";

		_mypassword = "";
		_guiState = GUIState.Message;
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
		StartCoroutine(DownloadQR(_qrCodeUrl));
    }
	
	private IEnumerator DownloadQR(string url)
	{
		WWW www = new WWW(url); 

		while(!www.isDone)
		{
			yield return www; 
		}

		QR = www.texture;
		yield break; 
	}
}

