using System;
using UnityEngine;

namespace PSG {
	public class Collectable : MonoBehaviour {

		public AudioSource SoundOnCollet;

		private void OnCollisionEnter2D(Collision2D other) {
			var player = other.transform.GetComponent<PlayerController2D>();
			if (player == null) return;
			this.Get();
		}

		private void OnTriggerEnter2D(Collider2D other) {
			var player = other.transform.GetComponent<PlayerController2D>();
			if (player == null) return;
			this.Get();
		}

		private void Get() {
			if (this.SoundOnCollet) {
				this.SoundOnCollet.transform.parent = null;
				this.SoundOnCollet.Play();
				if(this.SoundOnCollet.clip) Destroy(this.SoundOnCollet.gameObject, this.SoundOnCollet.clip.length);
			}
			Destroy(this.gameObject);
		}
	}
}
