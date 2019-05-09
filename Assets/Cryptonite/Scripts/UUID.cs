// Cryptonite™
// copyright 2019 by Envisible, Inc., All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using UnityEngine;

namespace Cryptonite
{
    public class UUID : MonoBehaviour
    {
        static PhysicalAddress wlan0Address;
        static PhysicalAddress eth0Address;

        static UUID()
        {
            try
            {
                List<NetworkInterface> networkInterfaces = new List<NetworkInterface>(NetworkInterface.GetAllNetworkInterfaces());
                foreach (NetworkInterface i in networkInterfaces)
                {
                    switch (i.Name.ToLower())
                    {
                        case "eth0":
                        case "en0":
                            eth0Address = i.GetPhysicalAddress();
                            break;

                        case "wlan0":
                        case "en1":
                            wlan0Address = i.GetPhysicalAddress();
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log("Failed to get network interface information. " + e.ToString());
            }
        }

        public static string Get()
        {
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();

            byte[] bytes0 = md5provider.ComputeHash(new System.Text.UTF8Encoding().GetBytes(SystemInfo.deviceUniqueIdentifier));
            byte[] bytes1 = md5provider.ComputeHash(new System.Text.UTF8Encoding().GetBytes(wlan0Address != null ? wlan0Address.ToString() : ""));
            byte[] bytes2 = md5provider.ComputeHash(new System.Text.UTF8Encoding().GetBytes(eth0Address != null ? eth0Address.ToString() : ""));

            /* fixformat ignore:start */
            char[] alphanumerics = { '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            /* fixformat ignore:end */

            // string result = bytes1[0].ToString("x2") + bytes1[1].ToString("x2") + bytes2[0].ToString("x2") + bytes2[1].ToString("x2");
            // string result = (bytes1[0] | bytes2[1]).ToString("x2") + (bytes1[1] | bytes2[0]).ToString("x2");

            string result = "";
            for (int i = 0; i < Mathf.Min(bytes0.Length, bytes1.Length, bytes2.Length); i++)
            {
                result += alphanumerics[(bytes0[i] + bytes1[i] + bytes2[i]) % alphanumerics.Length];
            }

            Debug.Log("Cryptonite uuid value : " + result);
            return result.Substring(0, 4).ToUpper();
        }
    }
}