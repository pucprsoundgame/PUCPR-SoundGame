using System.IO;
using UnityEngine;

namespace PSG {
	public static class PsgSettings {
		
		public const string ROOT_SOUNDS_FOLDER_NAME = "Sons";
		
		
		
		
		public static string GetRootSoundsFolder() {
#if UNITY_EDITOR
			return Application.streamingAssetsPath;
#endif
			var dir = Path.Combine(Application.dataPath, "Sounds");
			Debug.Log($"Sounds path: {dir}");
			return dir;
		}
	}
}