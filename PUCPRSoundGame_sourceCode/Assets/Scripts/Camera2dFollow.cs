using UnityEngine;

public class Camera2dFollow : MonoBehaviour {

	/// <summary>
	/// Target to follow.
	/// </summary>
	[SerializeField] private Transform target;
	
	
	void Update () {
		transform.position = new Vector3(target.position.x, transform.position.y, transform.position.z);
	}
}
