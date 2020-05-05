using System.IO;
using UnityEngine;

namespace PSG {
	public static class PsgSettings {
		
		public const string ROOT_SOUNDS_FOLDER_NAME = "Sons";
		
		
		
		
		public static string GetRootSoundsFolder() {
			#if UNITY_EDITOR
			return Path.Combine(Application.streamingAssetsPath, ROOT_SOUNDS_FOLDER_NAME);
			#endif
			var dir = Path.Combine(Application.dataPath, "..", ROOT_SOUNDS_FOLDER_NAME);
			Debug.Log($"Sounds path: {dir}");
			return dir;
		}
	}
}