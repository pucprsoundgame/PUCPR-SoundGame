using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Jukebox : MonoBehaviour
{

	private AudioSource _audioSource;

	private List<string> _allAudiosPaths = new List<string>();

	private string _dir;

	private float _healthMax = 80f;
	private float _healthCurrent;

	private void Awake()
	{
#if UNITY_WEBGL
		enabled = false;
#endif

		_audioSource = GetComponent<AudioSource>();
		_healthCurrent = _healthMax;

		_dir = Path.Combine(Application.streamingAssetsPath, "Sounds\\Musics\\JukeboxMusics");

		if (!Directory.Exists(_dir))
		{
			Directory.CreateDirectory(_dir);
			return;
		}

		DirectoryInfo info = new DirectoryInfo(_dir);
		FileInfo[] fileInfoList = info.GetFiles("*", SearchOption.AllDirectories);

		foreach (FileInfo file in fileInfoList)
		{
			if (!file.Name.EndsWith(".meta"))
			{
				_allAudiosPaths.Add(file.FullName);

			}
		}
		if (_allAudiosPaths == null || _allAudiosPaths.Count <= 0)
		{
			return;
		}
		StartCoroutine(LoadAndPlayMusic());
	}


	private IEnumerator LoadAndPlayMusic()
	{
		yield return null;
		_audioSource.Stop();

		if (_allAudiosPaths.Count <= 0)
		{
			yield break;
		}
		WWW www = new WWW("file://" + _allAudiosPaths[UnityEngine.Random.Range(0, _allAudiosPaths.Count)]);

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
		yield return null;
		_audioSource.Play();
		_audioSource.loop = true;
	}



	public void TakeDamage(float damage)
	{
		_healthCurrent -= damage;
		if (_healthCurrent <= 0)
		{
			_healthCurrent = _healthMax;
			StartCoroutine(LoadAndPlayMusic());
		}
	}

}
