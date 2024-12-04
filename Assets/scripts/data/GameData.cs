using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public bool JumpScareActive = false;
    public PowerTimer PowerTimer;
    public ShiftTimer ShiftTimer;
    public GameOver GameOver;
    public CameraScript CameraScript;
    public bool enabledSignal = false;
    public bool nightCompleted = false;

    public Dictionary<string, int[]> hashmap = new Dictionary<string, int[]>
    {
        { "Devin Neal", new int[] { 0, 0, 1, 2, 3, 4 } },
        { "Keenum", new int[] { 0, 3, 0, 2, 5, 10 } },
        { "Rahimi", new int[] { 0, 1, 5, 4, 7, 12 } },
        { "Colin", new int[] { 0, 1, 2, 6, 5, 16 } }
    };

    private void nightCompletedFunction() {
        CameraScript.CameraUI.SetActive(false);
        PowerTimer.PowerUI.SetActive(false);
        nightCompleted = true;
    }

    private void OnEnable() {
        ShiftTimer.nightCompletedTrigger += nightCompletedFunction;
        return;
    }

    private void OnDisable() {
        ShiftTimer.nightCompletedTrigger -= nightCompletedFunction;
    }

    void Start() {
        PowerTimer = gameObject.GetComponent<PowerTimer>();
        ShiftTimer = gameObject.GetComponent<ShiftTimer>();
        GameOver = gameObject.GetComponent<GameOver>();
        CameraScript = gameObject.GetComponent<CameraScript>();
    }
}