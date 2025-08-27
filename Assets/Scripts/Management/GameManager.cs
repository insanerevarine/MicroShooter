using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Management
{
	public class GameManager : MonoBehaviour
	{
		public void GameOver()
		{
			StartCoroutine(GameOverRoutine());
		}

		void StartNewGame()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}

		IEnumerator GameOverRoutine()
		{
			yield return new WaitForSeconds(3);
			StartNewGame();
		}
	}
}