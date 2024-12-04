using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public GameObject[] Cameras;
    
    public int CurrentCamera;

    [SerializeField] private KeyCode OpenCameras;

    public bool CamerasOpen;

    public GameObject MainCamera;

    public GameObject CameraUI;
    public ShiftTimer ShiftTimer;
    private float CoolDownTimer;
    private bool isNightCompleted = false;
    [SerializeField] private float CoolDownTime = 0.5f;
    [SerializeField] private PowerTimer Power;

    private void nightCompletedFunction() {
        isNightCompleted = true;
    }

    private void OnEnable() {
        ShiftTimer.nightCompletedTrigger += nightCompletedFunction;
        return;
    }

    private void OnDisable() {
        ShiftTimer.nightCompletedTrigger -= nightCompletedFunction;
    }

    void Start() {
        ShiftTimer = gameObject.GetComponent<ShiftTimer>();
        for(int i = 0; i < Cameras.Length; i++) {
            Cameras[i].SetActive(false);
        }

        CameraUI.SetActive(false);
        MainCamera.SetActive(true);
    }

    void Update() {

        if(Power.Power > 0f && !isNightCompleted) {

            if(Input.GetKeyDown(OpenCameras)) {
                CamerasOpen = !CamerasOpen;

                if(CamerasOpen)
                    Power.SystemsOn += 1;
                else
                    Power.SystemsOn -= 1;

                ShowCamera();
            }

            if(CoolDownTimer <= 0) {

                if(Input.GetAxis("Horizontal") > 0) {

                    Cameras[CurrentCamera].SetActive(false);
                    CurrentCamera += 1;

                    if(CurrentCamera >= Cameras.Length)
                        CurrentCamera = 0;

                    GoToCamera(CurrentCamera);
                    CoolDownTimer = CoolDownTime;
                }
                else if(Input.GetAxis("Horizontal") < 0) {
                
                    Cameras[CurrentCamera].SetActive(false);
                    CurrentCamera -= 1;

                    if(CurrentCamera < 0)
                        CurrentCamera = Cameras.Length - 1;

                    GoToCamera(CurrentCamera);
                    CoolDownTimer = CoolDownTime;
                }
            }
            else
                CoolDownTimer -= Time.deltaTime;
        }
        else {
            CamerasOpen = false;
            ShowCamera();
        }
    }

    private void ShowCamera() {
        if(CamerasOpen) {
            Cameras[CurrentCamera].SetActive(true);
            MainCamera.SetActive(false);
            CameraUI.SetActive(true);
        }
        else {

            Cameras[CurrentCamera].SetActive(false);
            MainCamera.SetActive(true);
            CameraUI.SetActive(false);
        }
    }

    public void GoToCamera(int Progression) {
        Cameras[CurrentCamera].SetActive(false);
        CurrentCamera = Progression;
        ShowCamera();
    }
}
