using System.Collections;
using System.Linq;
using UnityEngine;

public class ColinMovement : MonoBehaviour
{
    [SerializeField] private int Level;
    [SerializeField] private FSMNode Node;
    [SerializeField] private FSMNode ResetNode;
    [SerializeField] private float StartCoolDown = 5.0f;
    [SerializeField] private AudioSource source;
    [SerializeField] private GameObject GameManager;
    private float CoolDown;
    private GameData GameData;
    private bool IsMoving = false;
    [SerializeField] private float Speed;
    [SerializeField] private Door Door;
    private SpriteRenderer SpriteRenderer;
    private Vector3 StartPosition = new Vector3(-6.5f, 1.5f, -72.9f);
    private Vector3 DoorPosition = new Vector3(-5.4f, 1.5f, -88.35f);
    private bool IsWaitingForCamera = false;
    private bool IsLocked = false;
    private bool FirstAttack = false;
    private float AttackWaitTime = 25.0f;

    void Start() {
        GameData = GameManager.GetComponent<GameData>();
        Level = GameData.hashmap[gameObject.name][SaveDataManager.Instance.sharedData.night - 1];
        CoolDown = StartCoolDown;
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {

        if(Level == 0)
            return;

        if(GameData.PowerTimer.Power <= 0) {

            SpriteRenderer.enabled = false;
            enabled = false;
            return;
        }

        if(CoolDown > 0 && !IsMoving && !IsWaitingForCamera) {
            CoolDown -= Time.deltaTime;
            return;
        }

        if(GameData.CameraScript.CamerasOpen && GameData.CameraScript.CurrentCamera == 2)
            StartCoroutine(CollinLockState());

        if(IsLocked || !MoveCharacterRNG(Level))
            return;

        if(IsMoving)
            Node = MoveTowardsTarget();

        else if(!IsWaitingForCamera)
            Node = MoveCharacter();

        CoolDown = StartCoolDown;
    }

    //function that handles the RNG of character movement
    bool MoveCharacterRNG(int Level) {
        return Random.Range(1, 20) <= Level;
    }

    FSMNode MoveCharacter() {

        Node.IsOccupied = false;

        if (Node.IsColinRunNode) {
            
            SpriteRenderer.enabled = false;
            transform.position = StartPosition;
            transform.eulerAngles = Vector3.zero;

            StartCoroutine(WaitForCameraCheck());

            return Node;

        } else {

            Node = Node.Nodes[0];
            transform.position = Node.transform.position;
            transform.rotation = Node.transform.rotation;
            Node.IsOccupied = true;
        }

        return Node;
    }

    FSMNode MoveTowardsTarget() {
        transform.position = Vector3.MoveTowards(transform.position, DoorPosition, Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, DoorPosition) < 0.1f) {
            IsMoving = false;

            if(Door.isOpen && !GameData.JumpScareActive) {
                GameData.JumpScareActive = true;
                StartCoroutine(JumpScare());
            }
            else if(!GameData.JumpScareActive) {
                gameObject.transform.position = ResetNode.transform.position;
                gameObject.transform.rotation = ResetNode.transform.rotation;

                if(FirstAttack) {
                    GameData.PowerTimer.Power -= 1;
                    FirstAttack = true;
                }
                else
                    GameData.PowerTimer.Power -= 6;

                return ResetNode;
            }

        }

        return Node;
    }

    private void OnEnable() {
        ShiftTimer.LevelTrigger += LevelUpSignal;
        return;
    }

    private void OnDisable() {
        ShiftTimer.LevelTrigger -= LevelUpSignal;
    }

    private void LevelUpSignal(string[] data) {
        if(data.Contains(gameObject.name))
            Level++;
    }

    IEnumerator CollinLockState() {
        IsLocked = true;

        float random = Random.Range(0.83f, 16.67f);

        while(GameData.CameraScript.CamerasOpen && GameData.CameraScript.CurrentCamera == 2) {
            yield return null;
        }

        while(random > 0) {

            random -= Time.deltaTime;

            yield return null;
        }

        IsLocked = false;
    }

    IEnumerator WaitForCameraCheck() {

        IsWaitingForCamera = true;

        while(AttackWaitTime > 0) {

            if(GameData.CameraScript.CurrentCamera == 3 && GameData.CameraScript.CamerasOpen)
                break;
        
            AttackWaitTime -= Time.deltaTime;

            yield return null;
        }

        AttackWaitTime = 25.0f;
        IsWaitingForCamera = false;
        IsMoving = true;
        SpriteRenderer.enabled = true;
    }

    IEnumerator JumpScare() {
        gameObject.transform.position = new Vector3(-1.4f, 1.75f, -88.35f);
        Time.timeScale = 0;

        if(GameData.CameraScript.CamerasOpen) {
            GameData.CameraScript.Cameras[GameData.CameraScript.CurrentCamera].SetActive(false);
            GameData.CameraScript.CameraUI.SetActive(false);
            GameData.CameraScript.MainCamera.SetActive(true);
        }

        GameData.PowerTimer.PowerUI.SetActive(false);
        GameData.ShiftTimer.clockText.enabled = false;
        source.Play();
        yield return new WaitWhile(() => source.isPlaying);
        GameData.GameOver.StartGameOver();
    }
}
