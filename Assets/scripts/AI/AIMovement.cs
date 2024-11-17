using UnityEngine;
using System.Linq;
using System.Collections;

public class AIMovemovement : MonoBehaviour
{
    [SerializeField] private int Level;
    [SerializeField] private FSMNode Node;
    [SerializeField] private float StartCoolDown = 5.0f;
    private float CoolDown;
    [SerializeField] private AudioSource source;
    [SerializeField] private GameObject GameManager;
    private GameData GameData;
    private SpriteRenderer SpriteRenderer;

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

        if(CoolDown > 0) {
            CoolDown -= Time.deltaTime;
            return;
        }

        //check if the character is moving or not
        if(!MoveCharacterRNG(Level))
            return;

        //check if the character is at a door or not
        if(Node.IsDoor) {
            //check if the door is open
            if(Node.Door.isOpen && !Node.Office.IsOccupied) {
                //movement function but with office
                Node = MoveCharacterIntoOffice();
            }
            else
                //movement function
                Node = MoveCharacter();
        }
        //if the current destiantion is in the office
        else if(Node.IsOffice && !GameData.JumpScareActive) {
            GameData.JumpScareActive = true;
            StartCoroutine(JumpScare());
        }
        else
            //movement function
            Node = MoveCharacter();
        //reset
        CoolDown = StartCoolDown;
    }

    //function that handles the RNG of character movement
    bool MoveCharacterRNG(int Level) {
        return Random.Range(1, 20) <= Level;
    }

    //function that handles getting a random index in an array of destinations that accept the character and is not occupied
    FSMNode getRandomNode() {

        //add code to remove index that don't accept character or are occupied
        FSMNode[] FilteredNodes = Node.Nodes.Where(n => !n.IsOccupied && n.Accepts.Contains(gameObject)).ToArray();

        //add code to return current destination if there are no index left
        if(FilteredNodes.Length == 0)
            return Node;
        
        int RNG = Random.Range(0, FilteredNodes.Length);

        return FilteredNodes[RNG];
    }

    //function that handles movement of the characters
    FSMNode MoveCharacter() {

        Node.IsOccupied = false;

        Node = getRandomNode();

        gameObject.transform.position = Node.transform.position;
        gameObject.transform.rotation = Node.transform.rotation;

        Node.IsOccupied = true;

        return Node;
    }

    //function that handles the movement of a character into the office
    FSMNode MoveCharacterIntoOffice() {

        if(Node.Office.IsOccupied)
            return Node;

        Node.IsOccupied = false;

        Node = Node.Office;

        SpriteRenderer.enabled = false;
        gameObject.transform.position = Node.transform.position;
        gameObject.transform.rotation = Node.transform.rotation;

        Node.IsOccupied = true;

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

    IEnumerator JumpScare()
    {
        float WaitJumpScareTime = 20f;

        if(GameData.CameraScript.CamerasOpen)
            while(WaitJumpScareTime > 0) {

                if(!GameData.CameraScript.CamerasOpen) {
                    GameData.CameraScript.Cameras[GameData.CameraScript.CurrentCamera].SetActive(false);
                    GameData.CameraScript.CameraUI.SetActive(false);
                    GameData.CameraScript.MainCamera.SetActive(true);
                    break;
                }

                WaitJumpScareTime -= Time.deltaTime;

                yield return null;
            }

        Time.timeScale = 0;

        if(GameData.CameraScript.CamerasOpen) {
            GameData.CameraScript.Cameras[GameData.CameraScript.CurrentCamera].SetActive(false);
            GameData.CameraScript.CameraUI.SetActive(false);
            GameData.CameraScript.MainCamera.SetActive(true);
        }

        GameData.PowerTimer.PowerUI.SetActive(false);
        GameData.ShiftTimer.clockText.enabled = false;
        SpriteRenderer.enabled = true;
        source.Play();
        yield return new WaitWhile(() => source.isPlaying);
        GameData.GameOver.StartGameOver();
    }
}
