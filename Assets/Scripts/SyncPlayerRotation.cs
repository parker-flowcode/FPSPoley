using UnityEngine;

public class SyncPlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (cameraTransform != null)
        {
            float targetRotationY = cameraTransform.eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, targetRotationY, 0f);
        }
    }
}