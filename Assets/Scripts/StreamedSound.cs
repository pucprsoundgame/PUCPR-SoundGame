using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;

namespace PSG {
	[RequireComponent(typeof(AudioSource))]
	public class StreamedSound : MonoBehaviour {
		[FormerlySerializedAs("_soundName")] public string _soundPath;
		private AudioSource _audioSource;

		private void Start() {
			#if UNITY_WEBGL
			enabled = false;
			return;
			#endif
			this._audioSource = this.GetComponent<AudioSource>();
			this._audioSource.clip = null;
			string dir = Path.Combine(PsgSettings.GetRootSoundsFolder(), this._soundPath);
			this.StartCoroutine(this.LoadSound(dir));
		}

		private IEnumerator LoadSound(string path) {
			string filePath = null;

			filePath = "file://" + path + ".ogg";

			// Stop if file doesn't exists.
			if (!File.Exists(path + ".ogg")) {
				Debug.Log("File at path: " + path + " does not exist.");
				yield break;
			}

			var www = new WWW(filePath);

			AudioClip myAudioClip = null;

			do {
				myAudioClip = www.GetAudioClip();
				while (myAudioClip.loadState != AudioDataLoadState.Loaded) {
					while (myAudioClip.loadState == AudioDataLoadState.Loading) {
						yield return www;
					}
					myAudioClip = www.GetAudioClip();
					yield return null;
				}
				yield return null;
			} while (myAudioClip == null || myAudioClip.length == 0);

			this._audioSource.clip = myAudioClip;

			if (this._audioSource.playOnAwake && this._audioSource.enabled) {
				this._audioSource.Play();
			}
		}
	}
}
