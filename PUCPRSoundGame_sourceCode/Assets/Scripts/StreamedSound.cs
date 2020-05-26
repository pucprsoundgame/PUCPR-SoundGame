using System.Collections;
using System.IO;
using UnityEngine;

namespace PSG {
	[RequireComponent(typeof(AudioSource))]
	public class StreamedSound : MonoBehaviour {
		public string _soundPath;
		private AudioSource _audioSource;

		private void Start() {
			#if UNITY_WEBGL
			return;
			#endif
			this._audioSource = this.GetComponent<AudioSource>();
			string dir = Path.Combine(PsgSettings.GetRootSoundsFolder(), this._soundPath);

			this.StartCoroutine(this.LoadSound(dir));
		}

		private IEnumerator LoadSound(string path) {
			string filePath = "file://" + path + ".ogg";

			// Stop if file doesn't exists.
			if (!File.Exists(path + ".ogg")) {
				Debug.Log("File at path: " + filePath + " does not exist.");
				yield break;
			}

			WWW www = new WWW(filePath);

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
			} while (myAudioClip == null || myAudioClip.length <= 0);

			_audioSource.clip = myAudioClip;

			if (_audioSource.playOnAwake && _audioSource.enabled) {
				_audioSource.Play();
			}
		}
	}
}
