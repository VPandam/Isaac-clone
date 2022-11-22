using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] float lerpDuration = 0.2f;


    Vector3 LookAt = Vector3.zero;
    Camera camara;
    Vector3 offset = new Vector3(0, 0, -1);
    Vector3 positionToMove;
    // Start is called before the first frame update
    void Start()
    {
        camara = Camera.main;


    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != LookAt)
        {
            MoveCameraSmooth(LookAt);
        }
    }

    public void MoveCameraSmooth(Vector3 position)
    {
        position.z = -9;
        float elapsedTime = 0;
        elapsedTime = elapsedTime += Time.deltaTime;
        positionToMove = position + offset;

        float percentageComplete = elapsedTime / lerpDuration;

        this.transform.position = Vector3.Lerp(this.transform.position, positionToMove, percentageComplete);
    }
    public void MoveCameraTo(Vector3 position)
    {
        LookAt = position;
    }
}