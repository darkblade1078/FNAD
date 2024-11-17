using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class GameOver : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject screen;
    [SerializeField] private GameObject gameOverScreen;

    private void Awake()
    {
        screen.SetActive(false);
        gameOverScreen.SetActive(false);
    }

    public void StartGameOver()
    {
        screen.SetActive(true);
        Time.timeScale = 1;
        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OnVideoFinished;
        screen.SetActive(false);
        gameOverScreen.SetActive(true);
        StartCoroutine(LoadNextLevelAfterDelay());
    }

    private IEnumerator LoadNextLevelAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
