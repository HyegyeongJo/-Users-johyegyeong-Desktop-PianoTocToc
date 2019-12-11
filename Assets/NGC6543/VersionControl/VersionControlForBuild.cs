using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO.Compression;
#endif

namespace NGC6543
{
	/// <summary>
	/// Version Control asset for building a project.
	/// </summary>
	public class VersionControlForBuild : VersionControlBase 
	#if UNITY_EDITOR
	, IPreprocessBuildWithReport ,IPostprocessBuildWithReport
	#endif
	
	{
        #region  FIELDS
        
        const string ASSET_PATH = "Assets/_VersionControlForBuild.asset";
        
        const string LOG_HEADER = "[VersionControlForBuild] ";
        
        public enum AdditionalTagPosition {NEXT_TO_PRODUCTNAME, NEXT_TO_BUILDNUMBER}
        
		/// <summary>
        /// The current build number.
        /// </summary>
        /// <returns></returns>
        [SerializeField] int _buildNumber;
		#if UNITY_EDITOR
		
		[Tooltip("Switch build target")]
		[SerializeField] bool _overrideActiveBuildTarget = false;
		
		[SerializeField] BuildTarget _buildTarget = BuildTarget.StandaloneOSX;
		
		#endif
		
        /// <summary>
        /// Destination directory for a built project.
        /// </summary>
        /// <returns></returns>
		[SerializeField] string _builtProjectPath;

        /// <summary>
        /// Additional tag that will be included to the built project name.
        /// </summary>
        /// <returns></returns>
        [SerializeField] string _additionalTag;

        /// <summary>
        /// Where should the additional tag be placed?
        /// </summary>
        /// <returns></returns>
        [SerializeField] AdditionalTagPosition _additionalTagPosition;
        
        /// <summary>
        /// Indicates wether this is a 'Debug' build or 'Release' build.
        /// </summary>
        /// <returns></returns>
        [SerializeField] bool _isDevBuild;

        /// <summary>
        /// If set to true, Build Number won't be increased after a successful build.
        /// </summary>
        /// <returns></returns>
        [SerializeField] bool _doNotIncreaseBuildNumber;
		
		/// <summary>
		/// Release Note. Both File and Directory is allowed.
		/// </summary>
		/// <returns></returns>
		[SerializeField] Object _releaseNote;
		
		/// <summary>
		/// These objects will be copied to the output folder after a successful build.
		/// </summary>
		[SerializeField] Object[] _additionalObjectsToBeCopied;
		
		/// <summary>
		/// For Standalone or Android(internal) BuildTarget for 'Release' build, 
		/// if a release note is assigned, both built project and the release note will be compressed into a *.zip file.
		/// </summary>
		/// <returns></returns>
		[Tooltip("For Standalone or Android(internal) BuildTarget for 'Release' build,\nif a release note is assigned, both built project and the release note will be compressed ")]
        [SerializeField] bool _exportToZip;

        #endregion


        #region  PROPERTIES

        public int BuildNumber{ get { return _buildNumber; } }

#if UNITY_EDITOR
		/// <summary>
		/// [EDITOR_ONLY] IPreprocessBuildWithReport, IPostprocessBuildWithReport implementation
		/// </summary>
		/// <returns></returns>
        public int callbackOrder{get; set;}
#endif

		#endregion


		#region  UNITY_METHODS

#if UNITY_EDITOR
		void Reset()
		{
			_buildNumber = 1;
			_builtProjectPath =
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
        		"~/Desktop";
#elif UNITY_EDITOR_WIN
				System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
#endif
        }
#endif

		#endregion


		#region  STATIC_METHODS

#if UNITY_EDITOR
        
        [MenuItem("Version Control/Build/New Version Control asset for Build")]
        public static void CreateAsset()
        {
            // If there is an existing asset, do not create another one.
            VersionControlForBuild existingAsset = AssetDatabase.LoadAssetAtPath<VersionControlForBuild>(ASSET_PATH);
            if (AssetDatabase.ReferenceEquals(existingAsset, null))
            {
                Debug.Log(LOG_HEADER + "Creating a version control asset for build...");
                existingAsset = ScriptableObject.CreateInstance<VersionControlForBuild>();
                string path = AssetDatabase.GenerateUniqueAssetPath(ASSET_PATH);
                AssetDatabase.CreateAsset(existingAsset, path);
                
                existingAsset.UpdateVersion();
                existingAsset.IncrementBuildNumber(0);
                
                EditorUtility.SetDirty(existingAsset);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = existingAsset;
            }
            else
            {
                Debug.LogWarning(LOG_HEADER + "Existing asset was found! If you want to create a new version control asset for Build, manually delete this asset and try again.");
                
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = existingAsset;
            }
        }
        
#endif

		#endregion


		#region  PUBLIC_METHODS

		// Building a Project / editing the build number are only allowed in the Unity Editor.
#if UNITY_EDITOR
		/// <summary>
		/// [EDITOR_ONLY] Changes Built Project Folder path.
		/// </summary>
        public void ChangeBuiltProjectFolderPath()
        {
            string tempBuiltProjectPath = EditorUtility.SaveFolderPanel("The built project will be located here.", _builtProjectPath, "");
            if (tempBuiltProjectPath != "")
            {
                _builtProjectPath = tempBuiltProjectPath;
            }
        }
        
		/// <summary>
		/// [EDITOR_ONLY] Returns the would-be-name for the would-be-built project.
		/// AppName[AdditionalTag]_v[CurrentVersion]-b[BuildNumber][d/r]_YYMMDD
		/// </summary>
		/// <returns></returns>
		public string GetBuiltProjectName(bool isDevBuild)
		{
            string[] nameElements = Application.productName.Split(' ');
            return System.String.Concat(nameElements) +
                (_additionalTagPosition == AdditionalTagPosition.NEXT_TO_PRODUCTNAME ? _additionalTag : "") 
                + "_v" + CurrentVersion + "-b" + BuildNumber + (isDevBuild ? "d" : "r") 
                + (_additionalTagPosition == AdditionalTagPosition.NEXT_TO_BUILDNUMBER ? _additionalTag + "_" : "")
                + GetCurrentDateYYMMDD();
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
		/// [EDITOR_ONLY] Explicitly build this project for Development.
		/// </summary>
		public void BuildDebug()
		{
			Build(true);
		}
		
		/// <summary>
		/// [EDITOR_ONLY] Explicitly build this project for Release.
		/// </summary>
        public void BuildRelease()
		{
			Build(false);
		}
		
		/// <summary>
		/// [EDITOR_ONLY] Build this project using Development/Release specified in the asset.
		/// </summary>
		public void Build()
		{
			Build(_isDevBuild);
		}
		
        /// <summary>
        /// [EDITOR_ONLY] Build this project.
        /// </summary>
        /// <param name="isDevBuild"></param>
		public void Build(bool isDevBuild)
		{
            BuildPlayerOptions buildOptions = new BuildPlayerOptions();

            // location for built project
            string path = _builtProjectPath + "/" + GetBuiltProjectName(isDevBuild);
			
			// Build Target
			buildOptions.target = _overrideActiveBuildTarget ? _buildTarget : EditorUserBuildSettings.activeBuildTarget;
			
			// Change built file name with respect to build target.
			switch (buildOptions.target)
			{
				case BuildTarget.StandaloneWindows:
				case BuildTarget.StandaloneWindows64:
					buildOptions.locationPathName = path + "/" + GetBuiltProjectName(isDevBuild) + ".exe";
					Debug.Log(buildOptions.locationPathName);
					//throw new System.NotImplementedException();
					break;
				case BuildTarget.StandaloneOSX:
					buildOptions.locationPathName
						= path + ".app";
					break;
				case BuildTarget.Android:
					if (EditorUserBuildSettings.exportAsGoogleAndroidProject)
					{
						Debug.Log("Android : Export Project is enabled. Not adding .apk file name extension.");
						buildOptions.locationPathName = path;
					}
					else
					{
						buildOptions.locationPathName = path + ".apk";
					}
					break;
				case BuildTarget.iOS:
					buildOptions.locationPathName
						= path;
					break;
				default :
					Debug.LogWarning("Unusual build target " + buildOptions.target.ToString() + "! Consult with developer.");
					buildOptions.locationPathName = path;
					break;
			}
			
            // Scenes
            List<string> scenePaths = new List<string>();
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].enabled)
                {
                    Debug.Log("Adding active scene " + EditorBuildSettings.scenes[i].path);
                    scenePaths.Add(EditorBuildSettings.scenes[i].path);
                }
            }
            if (scenePaths.Count < 1)
            {
                Debug.LogError("At least 1 scene has to be active in build! Check the active scenes in Build Settings window!");
                return;
            }
            buildOptions.scenes = scenePaths.ToArray();


            // Is Development Build?
            if (isDevBuild)
            {
                buildOptions.options = BuildOptions.ShowBuiltPlayer | BuildOptions.Development | BuildOptions.ConnectWithProfiler;
            }
            else
            {
                buildOptions.options = BuildOptions.ShowBuiltPlayer;
            }

            Debug.Log("Build project to " + buildOptions.locationPathName);
            var buildReport = BuildPipeline.BuildPlayer(buildOptions);
		}

        /// <summary>
        /// [EDITOR_ONLY] Increment Build Number(+1).
        /// </summary>
        public void IncrementBuildNumber(int increment = 1)
		{
			if (_doNotIncreaseBuildNumber)
			{
				increment = 0;
			}
			_buildNumber += increment;

            SetBuildNumber(_buildNumber);
            
            EditorUtility.SetDirty(this);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
        
        /// <summary>
        /// [EDITOR_ONLY] Sets build Number.
        /// </summary>
        /// <param name="buildNumber"></param>
        public void SetBuildNumber(int buildNumber)
        {
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.StandaloneWindows:
                    break;
                case BuildTarget.StandaloneOSX:
                    PlayerSettings.macOS.buildNumber = BuildNumber.ToString();
                    break;
                case BuildTarget.Android:
                    PlayerSettings.Android.bundleVersionCode = BuildNumber;
                    break;
                case BuildTarget.iOS:
                    PlayerSettings.iOS.buildNumber = BuildNumber.ToString();
                    break;
            }
        }
		
		/// <summary>
		/// [EDITOR_ONLY] Called by Unity Editor on preprocess build.
		/// </summary>
		/// <param name="report"></param>
        public void OnPreprocessBuild(BuildReport report)
        {
            // throw new System.NotImplementedException();
        }
		
		/// <summary>
		/// [EDITOR_ONLY] Called by Unity Editor on postprocess build.
		/// </summary>
		/// <param name="report"></param>
        public void OnPostprocessBuild(BuildReport report)
        {
			Debug.Log("VersionAndControlForBuild::OnPostprocessBuild");

            /*
                Due to the fact that all the loaded object instances are destroyed before building a project, on postprocess build, it has to load an asset from a 'known(hard-coded)' asset path. This is the reason why the asset name and path should not be changed.
             */
            VersionControlForBuild target = AssetDatabase.LoadAssetAtPath<VersionControlForBuild>(ASSET_PATH);
            if (target == null)
            {
                Debug.LogWarning("[VersionControlForBuild] Version control asset for build was not found! Incrementing build number is disabled. Have you moved the Assets/_VersionControlForBuild.asset file?");
            }
            else
            {
                target.IncrementBuildNumber();
                
                string outputPath = report.summary.outputPath;
                // Debug.Log("outputPath : " + outputPath);
                
				// pathToBuiltProject : path/to/built/project/BuiltFileName.extension (standalone)
				//TODO check path for iOS
				string pathToBuiltProjectFolder = outputPath.Substring(0, outputPath.LastIndexOf('/'));
				
                // Release Note
                if (target._releaseNote != null)
                {
                    // Debug.Log("Build complete. builtProject folder : " + pathToBuiltProjectFolder);

                    // Application.dataPath : ~/Assets
                    // AssetDatabase.GetAssetPath(object) : Assets/path/to/file/FileName.extension
                    string releaseNoteFullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/')) + "/" + AssetDatabase.GetAssetPath(target._releaseNote);
                    
                    string releaseNoteExtension = Path.GetExtension(releaseNoteFullPath);
                    
                    string releaseNoteDestination = pathToBuiltProjectFolder + "/" + target._releaseNote.name + (target._additionalTagPosition == AdditionalTagPosition.NEXT_TO_PRODUCTNAME ? target._additionalTag : "")
                    + "_v" + target.CurrentVersion + (target._additionalTagPosition == AdditionalTagPosition.NEXT_TO_BUILDNUMBER ? target._additionalTag : "")
                    + "_" + target.GetCurrentDateYYMMDD() + releaseNoteExtension;

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
                
				// Additional objects to be copied to output folder.
				for (int i = 0; i < target._additionalObjectsToBeCopied.Length ; i++)
				{
					if (target._additionalObjectsToBeCopied[i] != null)
					{
						string objFullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'))
											+ "/" + AssetDatabase.GetAssetPath(target._additionalObjectsToBeCopied[i]);
						string objExtension = Path.GetExtension(objFullPath);
						string objDestination = pathToBuiltProjectFolder + "/" + target._additionalObjectsToBeCopied[i].name + objExtension;
						
						FileAttributes attr = File.GetAttributes(objFullPath);
						if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
						{
							// Directory
							if (Directory.Exists(objDestination))
							{
								Debug.Log("Replacing " + objFullPath + " to " + objDestination);
								FileUtil.ReplaceDirectory(objFullPath, objDestination);
							}
							else
							{
								Debug.Log("Copying " + objFullPath + " to " + objDestination);
								FileUtil.CopyFileOrDirectory(objFullPath, objDestination);
							}
						}
						else
						{
							// File
							if (File.Exists(objDestination))
							{
								Debug.Log("Replacing " + objFullPath + " to " + objDestination);
								FileUtil.ReplaceFile(objFullPath, objDestination);
							}
							else
							{
								Debug.Log("Copying " + objFullPath + " to " + objDestination);
								FileUtil.CopyFileOrDirectory(objFullPath, objDestination);
							}
						}
					}
				}
				
				
                // Export to Zip
                if (_exportToZip)
                {
                    //NOT IMPLEMENTED YET
                }
            }
            
        }


#endif

		#endregion
	}
}
