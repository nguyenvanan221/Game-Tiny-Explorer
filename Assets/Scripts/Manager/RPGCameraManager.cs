using Cinemachine;
using UnityEngine;

public class RPGCameraManager : MonoBehaviour
{
    public static RPGCameraManager sharedInstance = null;
    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    void Awake()
    {
        if (sharedInstance != null && sharedInstance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            sharedInstance = this;
        }

        GameObject vCameraObject = GameObject.FindWithTag("VirtualCamera");
        virtualCamera = vCameraObject.GetComponent<CinemachineVirtualCamera>();
    }
}
