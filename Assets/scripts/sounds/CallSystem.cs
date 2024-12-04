using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CallSystem : MonoBehaviour
{
    public AudioSource source;
    [SerializeField] private AudioClip[] Calls;
    private AudioClip CurrentCall;

    public void Start() {
        if(SaveDataManager.Instance.sharedData.night < 5) {
            Debug.Log("test");
            CurrentCall = Calls[SaveDataManager.Instance.sharedData.night - 1];
            StartCoroutine(PlayAudioWhenReady());
        }
    }

    private IEnumerator PlayAudioWhenReady() {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().isLoaded);
        source.PlayOneShot(CurrentCall);
    }
}
