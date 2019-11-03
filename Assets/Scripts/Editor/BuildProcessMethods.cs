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
			Directory.Move(Application.dataPath + "/Sounds",  Application.streamingAssetsPath + "/" + PsgSettings.ROOT_SOUNDS_FOLDER_NAME );
		}
	
		[PostProcessBuild(1)]
		public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
			// delete unity crash handler
			var path = pathToBuiltProject + "UnityCrashHandler32.exe";
			path = path.Replace($"{Application.productName}.exe", string.Empty);
			Debug.Log($"Deleting 'UnityCrashHandler32' {path}");
			File.Delete(path);
			
			// raise build number
			var splitVersion = PlayerSettings.bundleVersion.Split('.');
			var lastIndex = splitVersion.Length - 1;
			var intVersion = splitVersion[lastIndex];
			intVersion += 1;
			splitVersion[lastIndex] = intVersion.ToString();
			PlayerSettings.bundleVersion = splitVersion.ToString();
			
			// PSG only
			// move streaming assets and rename
			Directory.Move(Application.streamingAssetsPath + "/" + PsgSettings.ROOT_SOUNDS_FOLDER_NAME, Application.dataPath + "/Sounds");
			AssetDatabase.Refresh();
		}

		
	}
}
#endif