using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class AreaSound : MonoBehaviour {

	public GameObject InteriorSounds;
	public GameObject ExteriorSounds;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		InteriorSounds.SetActive(true);
		ExteriorSounds.SetActive(false);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		InteriorSounds.SetActive(false);
		ExteriorSounds.SetActive(true);
	}

}
