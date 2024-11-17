using System.Collections;
using UnityEngine;

public class PowerOut : MonoBehaviour {
    [SerializeField] private GameObject GameManager;
    [SerializeField] private AudioSource source;
    [SerializeField] private FSMNode OfficeNode;
    [SerializeField] private AudioClip DyingSong; 
    [SerializeField] private AudioClip JumpScareClip; 
    private SpriteRenderer SpriteRenderer;
    private PowerTimer PowerTimer;
    private GameOver GameOver; 
    private bool isActive;
    private int phase = 1;
    private bool ActivePhase = false;
    private bool isCoroutineRunning = false;

    void Start() {
        PowerTimer = GameManager.GetComponent<PowerTimer>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        GameOver = GameManager.GetComponent<GameOver>();
        source.clip = DyingSong;
    }

    void Update() {
        if (PowerTimer.Power > 0 && !isActive)
            return;
    
        if (!isCoroutineRunning) {
            if (phase == 1 && !ActivePhase)
                StartCoroutine(Timer1());
            else if (phase == 2 && !ActivePhase)
                StartCoroutine(Timer2());
            else if (phase == 3 && !ActivePhase)
                StartCoroutine(Timer3());
        }
    }

    IEnumerator Timer1() {
        isCoroutineRunning = true;
        isActive = true;
        ActivePhase = true;
        int timer = 5;

        yield return new WaitForSeconds(5);

        while (Random.Range(1, 100) > 20 && timer < 20) {
            timer += 5;
            yield return new WaitForSeconds(5);
        }

        phase = 2;
        ActivePhase = false;
        isActive = false;
        isCoroutineRunning = false;
    }

    IEnumerator Timer2() {
        isCoroutineRunning = true;
        SpriteRenderer.enabled = true;
        source.Play();
        ActivePhase = true;
        int timer = 5;

        yield return new WaitForSeconds(5);

        while (Random.Range(1, 100) > 20 && timer < 20) {
            yield return new WaitForSeconds(5);
            timer += 5;
        }

        SpriteRenderer.enabled = false;
        source.Stop();
        phase = 3;
        ActivePhase = false;
        isActive = false;
        isCoroutineRunning = false; // Allow next coroutine to start
    }

    IEnumerator Timer3() {
        isCoroutineRunning = true;
        SpriteRenderer.enabled = false;
        source.clip = JumpScareClip;
        ActivePhase = true;

        yield return new WaitForSeconds(2);

        while (Random.Range(1, 100) > 20) {
            yield return new WaitForSeconds(2);
        }

        phase = 4;
        ActivePhase = false;
        isActive = false;
        isCoroutineRunning = false;

        StartCoroutine(JumpScare());
    }

    IEnumerator JumpScare() {
        Time.timeScale = 0;
        transform.position = OfficeNode.transform.position;
        transform.rotation = OfficeNode.transform.rotation;
        SpriteRenderer.enabled = true;
        source.Play();

        yield return new WaitWhile(() => source.isPlaying);
        GameOver.StartGameOver();
    }
}