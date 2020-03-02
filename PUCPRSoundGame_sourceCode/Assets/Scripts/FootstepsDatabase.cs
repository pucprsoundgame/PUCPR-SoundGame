using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

	[SerializeField] private GameObject _loadingSoundsContainer;
	[SerializeField] private Text _textloadingProgress;


	private void Awake() 
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}


		_audioSource = GetComponent<AudioSource>();

		if (_audioSource == null)
		{
			_audioSource = gameObject.AddComponent<AudioSource>();
			_audioSource.spatialBlend = 0f;
			_audioSource.playOnAwake = false;
		}

#if UNITY_WEBGL
		return;
#endif

		StartCoroutine(LoadFootstepsSounds());
	}



	/// <summary>
	/// Load all footsteps sounds from folder.
	/// </summary>
	private IEnumerator LoadFootstepsSounds()
	{
		
		_footstepsDir = Path.Combine(Application.streamingAssetsPath, "Footsteps");

		if (!Directory.Exists(_footstepsDir))
		{
			Directory.CreateDirectory(_footstepsDir);
			yield break; // coroutine equivalent of return
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
				string path = Path.Combine(file.DirectoryName, file.Name);
				WWW www = new WWW("file://" + path);

				AudioClip myAudioClip = null;
				do
				{
					yield return null;
					myAudioClip = www.GetAudioClip();
					while (myAudioClip.loadState == AudioDataLoadState.Loading)
					{
						_textloadingProgress.text = (www.progress * 100f).ToString("000");
						yield return null;
					}
					yield return www;

				} while (myAudioClip == null || myAudioClip.length == 0);
	
				Debug.Log("Loaded audioClip length: " + myAudioClip.length);
				soundList.FirstOrDefault(s => s._soundTypeName == trimmedName)._sounds.Add(myAudioClip);
			}
		}

		_loadingSoundsContainer.SetActive(false);
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
	
	public string RemovePrefixes(string text)
	{
		// gambiarra da porra
		return text.Replace("1", string.Empty)
					.Replace("2", string.Empty)
					.Replace("3", string.Empty)
					.Replace("4", string.Empty)
					.Replace("5", string.Empty)
					.Replace("6", string.Empty)
					.Replace("7", string.Empty)
					.Replace("8", string.Empty)
					.Replace("9", string.Empty)
					.Replace("0", string.Empty)
					.Replace("tile_", string.Empty)
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
		Debug.Log("tileName: " + tileName);
		foreach (SoundInfo soundInfo in soundList)
		{
			if (soundInfo._soundTypeName.Split('.')[0] == tileName)
			{
				_audioSource.Stop();
				_audioSource.clip = soundInfo._sounds[UnityEngine.Random.Range(0, soundInfo._sounds.Count())];
				_audioSource.Play();
			}
		}

	}

}
