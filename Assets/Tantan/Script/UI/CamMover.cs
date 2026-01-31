using Unity.Cinemachine;
using UnityEngine;

public class CamMover : MonoBehaviour
{
    [SerializeField] CanvasGroup foodSection;
    [SerializeField] GameObject upButton;
    [SerializeField] GameObject downButton;

    CinemachineCamera camera;
    public void MoveToCam(CinemachineCamera cam)
    {
        if (camera != null)
        {
            camera.Priority = 0;
        }

        camera = cam;

        camera.Priority = 10;
    }

    public void MoveToDefaultCam()
    {
        if (camera != null)
        {
            camera.Priority = 0;
        }

        camera = GameObject.FindGameObjectWithTag("MainCam").GetComponent<CinemachineCamera>();
        camera.Priority = 10;

        foodSection.alpha = 0;
        upButton.SetActive(false);
        downButton.SetActive(true);
    }
}
