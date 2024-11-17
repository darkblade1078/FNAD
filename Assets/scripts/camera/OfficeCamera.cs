using UnityEngine;

public class OfficeCamera : MonoBehaviour
{

    [SerializeField] private float CameraSensitivity = 100f;

    [SerializeField] private float MinLookDist;
    [SerializeField] private float MaxLookDist;

    float camLookDistance;

    void Start()
    {
        camLookDistance = transform.localRotation.y;
    }

    void Update()
    {
        camLookDistance = Mathf.Clamp(camLookDistance + Input.GetAxis("Mouse X") * CameraSensitivity, MinLookDist, MaxLookDist);
        transform.localRotation = Quaternion.Euler(0f, camLookDistance, 0f);
    }
}
