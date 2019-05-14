using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*	
	By NGC6543(ngc6543@me.com)

	유의적 버전 2.0.0-k0 (https://semver.org)
	
	Semantic Versioning Summary

	Given a version number MAJOR.MINOR.PATCH, increment the:

		1. MAJOR version when you make incompatible API changes,
		2. MINOR version when you add functionality in a backwards-compatible manner, and
		3. PATCH version when you make backwards-compatible bug fixes.

	Additional labels for pre-release and build metadata are available as extensions to the MAJOR.MINOR.PATCH format.
*/

namespace NGC6543
{
	/// <summary>
	/// Base script for Semantic Versioning.
	/// </summary>
	public class VersionControlBase : ScriptableObject 
	{
		//FIXME Update here on API update!!
		public static string API_VERSION = "1.2.0";
		
		#region  FIELDS
		
		// Semantic Version
		[SerializeField] int _major = 0;
		[SerializeField] int _minor = 1;
		[SerializeField] int _patch = 0;
		
		#endregion
		
		
		#region  PROPERTIES
		
		/// <summary>
		/// Major.Minor.Patch.
		/// </summary>
		/// <returns></returns>
		public string CurrentVersion
		{
			get
			{
				return _major + "." + _minor + "." + _patch;
			}
		}

        #endregion
		
		
		// #region UNITY_METHODS
		
		// void Reset()
		// {
		// 	_major = 0;
		// 	_minor = 1;
		// 	_patch = 0;
		// }
		
		// #endregion
		

        #region  PUBLIC_METHODS

        // Editing versions are only allowed in the Unity Editor.
		#if UNITY_EDITOR

        /// <summary>
        /// [EDITOR_ONLY] Increment Major version. The Minor and Patch versions will be reset to 0.
        /// </summary>
        public void IncrementMajor()
		{
			_major++;
			_minor = 0;
			_patch = 0;
            UpdateVersion();
		}

        /// <summary>
        /// [EDITOR_ONLY] Increment Minor version. The Patch version will be reset to 0.
        /// </summary>
        public void IncrementMinor()
		{
			_minor++;
			_patch = 0;
            UpdateVersion();
		}

        /// <summary>
        /// [EDITOR_ONLY] Increment Patch version.
        /// </summary>
        public void IncrementPatch()
		{
			_patch++;
			UpdateVersion();
		}
		
		/// <summary>
		/// [EDITOR_ONLY] Updates Bundle Version on PlayerSettings to this version.
		/// </summary>
		public void UpdateVersion()
		{
			Debug.Log("Updated to " + CurrentVersion);
			
			PlayerSettings.bundleVersion = CurrentVersion;
			
			EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
		
		#endif
		
		#endregion
	}
}
