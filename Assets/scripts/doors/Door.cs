using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private GameObject Light;

    [SerializeField] private float DoorSpeed;

    [SerializeField] private Vector3 OpenPos;
    [SerializeField] private Vector3 ClosePos;

    [SerializeField] private PowerTimer Power;

    public bool isOpen;
    public bool isOn;

    void Start()
    {
        Light.SetActive(false);
        transform.position = OpenPos;
        isOpen = true;
    }


    void Update()
    {

        if(Power.Power > 0) {

            if(isOpen) {

                if(transform.position != OpenPos)
                    if(Vector3.Distance(transform.position, OpenPos) <= 0.5f)
                        transform.position = OpenPos;
                    else
                        transform.position = Vector3.Lerp(transform.position, OpenPos, DoorSpeed * Time.deltaTime);
            }
            else {

                if(transform.position != ClosePos)
                    if(Vector3.Distance(transform.position, ClosePos) <= 0.5f)
                        transform.position = ClosePos;
                    else
                        transform.position = Vector3.Lerp(transform.position, ClosePos, DoorSpeed * Time.deltaTime);
            }
        }
        else {

            if(transform.position != OpenPos)
                if(Vector3.Distance(transform.position, OpenPos) <= 0.5f)
                    transform.position = OpenPos;
                else
                    transform.position = Vector3.Lerp(transform.position, OpenPos, DoorSpeed * Time.deltaTime);

            isOn = false;
            ChangeLights();
        }
    }

    public void ChangeLights() {
        
        isOn = !isOn;
        
        if(isOn) {
            Light.SetActive(true);
            Power.SystemsOn += 1;
        }
        else {
            Light.SetActive(false);
            Power.SystemsOn -= 1;
        }
    }
}