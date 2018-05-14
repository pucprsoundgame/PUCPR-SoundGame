using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantGameOver : MonoBehaviour {


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			GameOver();
		}
	}

	public void GameOver()
	{
		SceneManager.LoadScene("GameOver");
	}

}
