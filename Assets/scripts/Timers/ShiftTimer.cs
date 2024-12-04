using TMPro;
using UnityEngine;
using System;

public class ShiftTimer : MonoBehaviour
{
    public float timer;
    [SerializeField] private int endTime = 6;
    [SerializeField] private string digitalClock;
    public TextMeshProUGUI clockText;
    [SerializeField] private GameObject Winscreen;
    private float[] TimeTriggers = { 120.0f, 180.0f, 240.0f };
    private int iteration = 0;
    private bool finalStage = false;
    public static event Action<string[]> LevelTrigger;
    public static event Action nightCompletedTrigger;
    [SerializeField] private int SecondsPerHour;

    void Start() {

        digitalClock = "";
    }

    void Update() {

        timer += Time.deltaTime;

        if(!finalStage) {

            if(iteration == 0) 
                TriggerDataSignal(new string[] { "Keenum" });
            else
                TriggerDataSignal(new string[] { "Keenum", "Colin", "Rahimi" });

            if(iteration == 2)
                finalStage = true;

            iteration++;
        }

        var hours = Mathf.FloorToInt(timer / SecondsPerHour);

        if(hours == 0)
            hours = 12;

        digitalClock = string.Format("{0} AM", hours);
        clockText.text = digitalClock;

        if(hours == endTime) {
            TriggerNightCompletedSignal();
            Winscreen.SetActive(true);
        }
    }

    public static void TriggerNightCompletedSignal() {
        nightCompletedTrigger?.Invoke();
    }

    public static void TriggerDataSignal(string[] data) {
        LevelTrigger?.Invoke(data);
    }
}
