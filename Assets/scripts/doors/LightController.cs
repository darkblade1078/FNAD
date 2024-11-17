using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private Door Door;

    private void OnMouseDown() {
        Door.ChangeLights();
    }
}
