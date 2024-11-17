using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Door Door;
    [SerializeField] private PowerTimer Power;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    
    private void OnMouseDown() {
        Door.isOpen = !Door.isOpen;

        if(Power.Power > 0)
            source.PlayOneShot(clip);

        if(Door.isOpen)
            Power.SystemsOn -= 1;
        else
            Power.SystemsOn += 1;
    }
}
