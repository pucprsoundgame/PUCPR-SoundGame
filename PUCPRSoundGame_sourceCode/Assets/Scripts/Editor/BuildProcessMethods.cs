#if UNITY_EDITOR

using UnityEditor.Build.Reporting;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEngine;

namespace PSG {
	public class BuildProcessMethods : IPreprocessBuildWithReport {

		public int callbackOrder { get; }
		public void OnPreprocessBuild(BuildReport report) {
			// var sourceDirName = Application.dataPath + "/" + PsgSettings.ROOT_SOUNDS_FOLDER_NAME;
			// var destDirName = Application.streamingAssetsPath + "/" + PsgSettings.ROOT_SOUNDS_FOLDER_NAME;
			// Directory.Move(sourceDirName, destDirName);
		}
	
		[PostProcessBuild(1)]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltExe) {
			// get root build path
			var rootProjectBuildPath = pathToBuiltExe.Replace($"{Application.productName}.exe", string.Empty);

			// delete unity crash handler
			var unityCrashHandlerPath = rootProjectBuildPath + "/UnityCrashHandler32.exe";
			Debug.Log($"Deleting 'UnityCrashHandler32' {unityCrashHandlerPath}");
			File.Delete(unityCrashHandlerPath);
			
			// raise build number
			var splitVersion = PlayerSettings.bundleVersion.Split('.');
			var lastIndex = splitVersion.Length - 1;
			var intVersion = int.Parse(splitVersion[lastIndex]);
			intVersion += 1;
			splitVersion[lastIndex] = intVersion.ToString();
			PlayerSettings.bundleVersion = string.Join(".", splitVersion);
			
			// PSG only
			// move streaming assets and rename
			// Directory.Move(
			// 	$"{rootProjectBuildPath}{Application.productName}_Data/StreamingAssets/{PsgSettings.ROOT_SOUNDS_FOLDER_NAME}",
			// 	$"{rootProjectBuildPath}/{PsgSettings.ROOT_SOUNDS_FOLDER_NAME}");
			// Directory.Move(
			// 	$"{Application.streamingAssetsPath}/{PsgSettings.ROOT_SOUNDS_FOLDER_NAME}", 
			// 	$"{Application.dataPath}/{PsgSettings.ROOT_SOUNDS_FOLDER_NAME}");
			AssetDatabase.Refresh();
		}

		
	}
}
#endif