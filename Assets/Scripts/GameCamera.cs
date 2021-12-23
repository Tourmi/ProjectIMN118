using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GameCamera : MonoBehaviour
{
    [SerializeField]
    private Stage stage;
    [SerializeField]
    private Transform character1;
    [SerializeField]
    private Transform character2;
    [SerializeField]
    private Transform leftCameraWall;
    [SerializeField]
    private Transform rightCameraWall;


    private Camera gameCamera;

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        float resolutionRatio = gameCamera.orthographicSize * Screen.width / Screen.height;

        float horizontalPos = (character1.position.x + character2.position.x) / 2;
        float minX = stage.transform.position.x - (stage.Bounds.x / 2) + resolutionRatio;
        float maxX = stage.transform.position.x + (stage.Bounds.x / 2) - resolutionRatio;
        horizontalPos = Mathf.Max(Mathf.Min(maxX, horizontalPos), minX);

        gameCamera.transform.position = new Vector3(horizontalPos, gameCamera.transform.position.y, gameCamera.transform.position.z);


        leftCameraWall.position = gameCamera.transform.position - new Vector3(resolutionRatio, 0, 0);
        rightCameraWall.position = gameCamera.transform.position + new Vector3(resolutionRatio, 0, 0);
    }
}
