using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

	public void ReturnToMenu()
	{
		SceneManager.LoadScene("Menu");

	}

	public void TryAgain()
	{
		SceneManager.LoadScene("Game");
	}
}
