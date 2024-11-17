using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public AudioSource MainMenuMusic;
    [SerializeField] private GameObject[] Stars;
    [SerializeField] private GameObject QuitGameButton;
    [SerializeField] private GameObject Night6Button;
    [SerializeField] private GameObject CustomNightButton;

    void Start() {
        StartCoroutine(PlayAudioWhenReady());

        if(!SaveDataManager.Instance.sharedData.star1) {

            Stars[0].SetActive(false);
            QuitGameButton.transform.position = Night6Button.transform.position;
            Night6Button.SetActive(false);
        }


        if(!SaveDataManager.Instance.sharedData.star2) {

            Stars[1].SetActive(false);
            CustomNightButton.SetActive(false);
        }

        if(!SaveDataManager.Instance.sharedData.star2)
            Stars[2].SetActive(false);
    }

    private IEnumerator PlayAudioWhenReady() {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        MainMenuMusic.Play();
    }

    //Load into the next scene which is Game
    public void NewGame() {
        MainMenuMusic.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame() {
        MainMenuMusic.Stop();

        if(SaveDataManager.Instance.sharedData.night > 5)
            SaveDataManager.Instance.sharedData.night = 5;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Night6() {
        MainMenuMusic.Stop();
        SaveDataManager.Instance.sharedData.night = 6;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Quit the game
    public void QuitGame() {
        Application.Quit();
    }
}
