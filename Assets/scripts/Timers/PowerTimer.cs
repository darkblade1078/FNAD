using TMPro;
using UnityEngine;

public class PowerTimer : MonoBehaviour {

  public int SystemsOn;
  public float Power = 100;
  [SerializeField] private TextMeshProUGUI PowerText;
  [SerializeField] private GameObject Lights;
  public GameObject PowerUI;
  private float WaitTime;
  private float StartingWaitTime = 6;
  private bool IsNightOne = SaveDataManager.Instance.sharedData.night == 1;
  void Start() {

    if(!IsNightOne)
      SystemsOn = 1;

    WaitTime = StartingWaitTime;
  }
  
  void Update() {

    if(WaitTime <= 0) {
      Power -= 1 * SystemsOn;
      WaitTime = StartingWaitTime;
    }

    WaitTime -= Time.deltaTime;

    if(Power <= 0) {
      Lights.SetActive(false);
      PowerUI.SetActive(false);
      SystemsOn = 0;
    }
      
    var power = string.Format("{0:0}", Power);
    PowerText.text = $"{power}%";
  }
}
