using System.Collections;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StreamedSound : MonoBehaviour
{

	[SerializeField] private string _soundName;
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
		WWW www = new WWW("file://" + path + ".ogg");

		AudioClip myAudioClip = www.GetAudioClip();
		while (myAudioClip.loadState != AudioDataLoadState.Loaded)
		{
			while (myAudioClip.loadState == AudioDataLoadState.Loading)
			{
				yield return www;
			}
			myAudioClip = www.GetAudioClip();
			yield return null;
		}

		_audioSource.clip = myAudioClip;
		_audioSource.Play();
	}

}
