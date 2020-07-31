using UnityEngine;

namespace PSG {
	public class Damageable : MonoBehaviour {

		public AudioSource playOnDie;
		
		public void TakeDamage() {
			this.playOnDie.transform.parent = null;
			this.playOnDie.Play();
			if(playOnDie.clip) Destroy(this.playOnDie.gameObject, this.playOnDie.clip.length);
			Destroy(this.gameObject);
		}
	}
}
