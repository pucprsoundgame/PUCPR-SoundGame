using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FootstepsDatabase : MonoBehaviour {

	#region Singleton
	private static FootstepsDatabase instance;
	public static FootstepsDatabase Instance
	{
		get{
			if (instance == null)
			{
				instance = FindObjectOfType<FootstepsDatabase>();
			}
			if (instance == null)
			{
				instance = new GameObject("FootstepsDatabase", typeof(FootstepsDatabase)).GetComponent<FootstepsDatabase>();
			}
			return instance;
		}
	}
	#endregion

	/// <summary>
	/// List with all footstep sounds.
	/// </summary>
	[System.Serializable]
	public class SoundInfo
	{
		public string _soundTypeName;									// example: wood, grass.
		public List<AudioClip> _sounds = new List<AudioClip>();			// list with all found sounds.
	}
	public List<SoundInfo> soundList;

	private string _footstepsDir;
	private AudioSource _audioSource;

	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}

		_audioSource = gameObject.AddComponent<AudioSource>();
		_audioSource.spatialBlend = 0f;
		_audioSource.playOnAwake = false;

		GetFootstepsSounds();

		DontDestroyOnLoad(gameObject);
	}

	/// <summary>
	/// Get all files and organize then on lists.
	/// </summary>
	private void GetFootstepsSounds()
	{
		_footstepsDir = Path.Combine(Application.streamingAssetsPath, "Footsteps");

		if (!Directory.Exists(_footstepsDir))
		{
			Directory.CreateDirectory(_footstepsDir);
			return;
		}
		soundList = new List<SoundInfo>();

		DirectoryInfo info = new DirectoryInfo(_footstepsDir);
		FileInfo[] fileInfoList = info.GetFiles("*", SearchOption.AllDirectories);

		// Foreach file in folder... 
		foreach (FileInfo file in fileInfoList)
		{
			if (!file.Name.EndsWith(".meta"))
			{
				string trimmedName = RemovePrefixes(file.Name);

				if (!IsSoundOnList(trimmedName))
				{
					// Not in the list, create an entry.
					var soundToAdd = new SoundInfo();
					soundToAdd._soundTypeName = trimmedName;
					soundList.Add(soundToAdd);

				}

				// Load a sound from folder and add it on the list.
				StartCoroutine(LoadSound(Path.Combine(file.DirectoryName, file.Name), soundList.FirstOrDefault(s => s._soundTypeName == trimmedName)));
			}
			
		}

	}

	private bool IsSoundOnList(string trimmedName)
	{
		// ...check if sound name its already on the list
		foreach (SoundInfo soundInfo in soundList)
		{
			// ...check if the file name already have a list.
			if (soundInfo._soundTypeName == trimmedName)
			{
				return true;
			}
		}
		return false;
	}

	private IEnumerator LoadSound(string path, SoundInfo listToAdd)
	{
		WWW www = new WWW("file://" + path);

		AudioClip myAudioClip = www.GetAudioClip();
		while (myAudioClip.loadState == AudioDataLoadState.Loading)
			yield return www;

		listToAdd._sounds.Add(myAudioClip);
	}
	
	public string RemovePrefixes(string text)
	{
		// gambiarra da porra
		text = text.Replace("1", "");
		text = text.Replace("2", "");
		text = text.Replace("3", "");
		text = text.Replace("4", "");
		text = text.Replace("5", "");
		text = text.Replace("6", "");
		text = text.Replace("7", "");
		text = text.Replace("8", "");
		text = text.Replace("9", "");
		text = text.Replace("0", "");
		return text.Replace("tile_", string.Empty)
			.Replace("sfx_", string.Empty)
			.Replace(@"[\d-]", string.Empty)
			.ToLower()
			.Trim();
	}
		

	/// <summary>
	/// Play the sound of a footstep to the tile.
	/// </summary>
	public void PlayFootstepOfTile(string tileName)
	{
		tileName = RemovePrefixes(tileName).Split('.')[0];
		//Debug.Log("tileName: " + tileName);
		foreach (SoundInfo soundInfo in soundList)
		{
			if (soundInfo._soundTypeName.Split('.')[0] == tileName)
			{
				_audioSource.clip = soundInfo._sounds[UnityEngine.Random.Range(0, soundInfo._sounds.Count())];
				_audioSource.Play();
			}
		}

	}

}
