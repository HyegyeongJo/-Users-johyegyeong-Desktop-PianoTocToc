using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

namespace NGC6543
{
	/// <summary>
	/// Version Control asset for Unity Package.
	/// </summary>
    public class VersionControlForUnityPackage : VersionControlBase
	{
		#region  FIELDS
		const string ASSET_PATH = "Assets/New Version Control for UnityPackage.asset";
		const string LOG_HEADER = "[VersionControlForUnityPackage] ";
        
        public enum AdditionalTagPosition {NEXT_TO_PACKAGENAME, NEXT_TO_EXPORTNUMBER}

        /// <summary>
        /// Objects(Folders, Files) that will be exported into UnityPackage.
        /// </summary>
        /// <returns></returns>
        [SerializeField] Object[] _objectsToBeExported;		
		
		/// <summary>
		/// Objects(Folders, Files) that will be excluded from UnityPackage.
		/// </summary>
		// [SerializeField] Object[] _objectsToExclude;
		
		/// <summary>
		/// 
		/// </summary>
		[SerializeField] string _packageName = "PackageName";
		
		/// <summary>
		/// The current export number.
        /// </summary>
        /// <returns></returns>
		[SerializeField] int _exportNumber = 1;

        /// <summary>
        /// Destination directory for exported project
        /// </summary>
        /// <returns></returns>
        [SerializeField] string _exportedPackagePath
         
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
			= "~/Desktop";
#elif UNITY_EDITOR_WIN
            = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
#else
            ;
#endif

        /// <summary>
        /// Additional tag that will be included to the exported package name.
        /// </summary>
        /// <returns></returns>
        [SerializeField] string _additionalTag;
        
        
        [SerializeField] AdditionalTagPosition _additionalTagPosition;
        
        /// <summary>
		/// Release Note. Both File and Directory is allowed.
		/// </summary>
		/// <returns></returns>
        [SerializeField] Object _releaseNote;

        #endregion


        #region  PUBLIC_METHODS
        
        #if UNITY_EDITOR
        /// <summary>
        /// [EDITOR_ONLY] Creates a new Version Control asset for UnityPackage.
        /// </summary>
        [MenuItem("Version Control/UnityPackage/New Version Control asset for UnityPackage")]
        public static void CreateAsset()
        {
            string newAssetPath = ASSET_PATH;
            VersionControlForUnityPackage existingAsset = AssetDatabase.LoadAssetAtPath<VersionControlForUnityPackage>(newAssetPath);
            if (!AssetDatabase.ReferenceEquals(existingAsset, null))
            {
                int i = 1;
                while (true)
                {
                    newAssetPath = "Assets/New Version Control for UnityPackage" + i + ".asset";
                    existingAsset = AssetDatabase.LoadAssetAtPath<VersionControlForUnityPackage>(newAssetPath);
                    if (!AssetDatabase.ReferenceEquals(existingAsset, null))
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
            Debug.Log(LOG_HEADER + "Creating a version control asset for UnityPackage...");
            existingAsset = ScriptableObject.CreateInstance<VersionControlForUnityPackage>();
            string path = AssetDatabase.GenerateUniqueAssetPath(newAssetPath);
            AssetDatabase.CreateAsset(existingAsset, path);
            
            EditorUtility.SetDirty(existingAsset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = existingAsset;
            
        }
        
        /// <summary>
        /// [EDITOR_ONLY] Changes export folder path.
        /// </summary>
        public void ChangeExportFolderPath()
        {
            string tempExportPath = EditorUtility.SaveFolderPanel("Select export path", _exportedPackagePath, "");
            if (tempExportPath != "")
            {
                _exportedPackagePath = tempExportPath;
            }
            
            // _exportedPackagePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        }
        
        /// <summary>
        /// [EDITOR_ONLY] Returns the would-be-name for the would-be-exported package.
        /// PackageName[AdditionalTag]_v[CurrentVersion]-e[ExportNumber]_YYMMDD
        /// </summary>
        /// <returns></returns>
        public string GetExportedPackageName()
        {
            string[] nameElements =_packageName.Split(' ');
            return System.String.Concat(nameElements) + (_additionalTagPosition == AdditionalTagPosition.NEXT_TO_PACKAGENAME ? _additionalTag : "") + "_" + "v" + CurrentVersion
            + "-e" + _exportNumber + (_additionalTagPosition == AdditionalTagPosition.NEXT_TO_EXPORTNUMBER ? _additionalTag : "") + "_" + GetCurrentDateYYMMDD() + ".UnityPackage";
        }

        /// <summary>
        /// [EDITOR_ONLY] Returns toady's date in YYMMDD format.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentDateYYMMDD()
        {
            return System.DateTime.Today.Year.ToString().Substring(2) + System.DateTime.Today.Month.ToString("D2") + System.DateTime.Today.Day.ToString("D2");
        }
        
		/// <summary>
		/// [EDITOR_ONLY] Exports assets to UnityPackage.
		/// </summary>
		public void ExportUnityPackage()
		{
			// Export package. If there is no file or directory, stop
			if (_objectsToBeExported.Length != 0)
			{
				string[] assetPaths = new string[_objectsToBeExported.Length];
                string message = "Exporting :";
				for (int i = 0; i < assetPaths.Length; i++)
				{
					assetPaths[i] = AssetDatabase.GetAssetPath(_objectsToBeExported[i]);
                    message += "\n	" + assetPaths[i];
				}
				
                // Export the temporary package into the project folder first, then move it to the specified folder.
				string destinationPath = GetExportedPackageName();
                message += "\n Destination path : " + destinationPath;
				// AssetDatabase.ExportPackage(assetPaths, GetExportedPackageName(), ExportPackageOptions.)
                AssetDatabase.ExportPackage(assetPaths, destinationPath, ExportPackageOptions.Recurse);
				
                destinationPath = _exportedPackagePath + "/" + GetExportedPackageName();
                
                // Handle exception : Create directory. If it already exist, does nothing.
                
                string tempExportPath = _exportedPackagePath;
                if (tempExportPath.StartsWith("~/"))
                {
                    Debug.Log("The Exported Package Path is starting with ~. Replacing...");
                    tempExportPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + tempExportPath.Substring(1);
                    Debug.Log("tempExportPath : " + tempExportPath);
                }
                
                Directory.CreateDirectory(tempExportPath);
                FileUtil.MoveFileOrDirectory(Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/" + GetExportedPackageName(), destinationPath);
                
                #if UNITY_EDITOR_OSX
                EditorUtility.RevealInFinder(destinationPath);
                #elif UNITY_EDITOR_WIN
                // System.Diagnostics.Process.Start("Finder.app", "/select," + destinationPath);   
                Debug.LogWarning("TODO : Show exported package in Windows Explorer : https://answers.unity.com/questions/43422/how-to-implement-show-in-explorer.html");
                /*
                The command line you want is:

    explorer.exe /select,<path to file or folder>

You can run a command line such as this with System.Diagnostics.Process.Start, as follows (C#) ...

    public void ShowExplorer(string itemPath)
    {
        itemPath = itemPath.Replace(@"/", @"\");   // explorer doesn't like front slashes
        System.Diagnostics.Process.Start("explorer.exe", "/select,"+itemPath);
                */             
#endif
                Debug.Log(message);
                _exportNumber++;
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                // Release Note
                if (_releaseNote != null)
                {   
                    string releaseNoteFullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/" + AssetDatabase.GetAssetPath(_releaseNote);
                    
                    string releaseNoteExtension = Path.GetExtension(releaseNoteFullPath);
                    
                    string releaseNoteDestination = _exportedPackagePath + "/" + _releaseNote.name + (_additionalTagPosition == AdditionalTagPosition.NEXT_TO_PACKAGENAME ? _additionalTag : "") + "_v" + CurrentVersion +(_additionalTagPosition == AdditionalTagPosition.NEXT_TO_EXPORTNUMBER ? _additionalTag : "") + "_" + GetCurrentDateYYMMDD() + releaseNoteExtension;
                    
                    FileAttributes attr = File.GetAttributes(releaseNoteFullPath);
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        // Debug.Log("Directory!");
                        // Directory. check if the directory exists
                        if (Directory.Exists(releaseNoteDestination))
                        {
                            Debug.Log("Replacing Release Note Directory.");
                            FileUtil.ReplaceDirectory(releaseNoteFullPath, releaseNoteDestination);
                        }
                        else
                        {
                            Debug.Log("Copying Release Note directory.");
                            FileUtil.CopyFileOrDirectory(releaseNoteFullPath, releaseNoteDestination);
                        }
                    }
                    else
                    {
                        // Debug.Log("File!");
                        // File. Check if the file exists
                        if (File.Exists(releaseNoteDestination))
                        {
                            Debug.Log("Replacing Release Note file.");
                            FileUtil.ReplaceFile(releaseNoteFullPath, releaseNoteDestination);
                        }
                        else
                        {
                            Debug.Log("Copying Release Note file.");
                            FileUtil.CopyFileOrDirectory(releaseNoteFullPath, releaseNoteDestination);
                        }
                    }
                }
			}
			else
            {
                Debug.LogError(LOG_HEADER + "No assets are selected! Make sure that there is at least 1 asset to be exported!");
                return;
            }
            
			//TODO copy release note
		}
        
		#endif
        
		#endregion
	}
	
}
