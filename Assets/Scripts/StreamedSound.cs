using System.Collections;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StreamedSound : MonoBehaviour
{

	public string _soundName;
	private AudioSource _audioSource;

	private void Start()
	{
		_audioSource = GetComponent<AudioSource>();
		string dir = Path.Combine(Application.streamingAssetsPath, "SFX");

		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}
		StartCoroutine(LoadSound(Path.Combine(dir, _soundName)));

	}


	private IEnumerator LoadSound(string path)
	{

		string filePath = "file://" + path + ".ogg";

		// Stop if file doesn't exists.
		if (!File.Exists(path + ".ogg"))
		{
			Debug.Log("File at path: " + path + " does not exist.");
			yield break;
		}

		WWW www = new WWW(filePath);

		AudioClip myAudioClip = null;

		do
		{
			myAudioClip = www.GetAudioClip();
			while (myAudioClip.loadState != AudioDataLoadState.Loaded)
			{
				while (myAudioClip.loadState == AudioDataLoadState.Loading)
				{
					yield return www;
				}
				myAudioClip = www.GetAudioClip();
				yield return null;
			}
			yield return null;

		} while (myAudioClip == null || myAudioClip.length == 0);

		_audioSource.clip = myAudioClip;

		if (_audioSource.playOnAwake && _audioSource.enabled)
		{
			_audioSource.Play();
		}
	}

}
