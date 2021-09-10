using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    float desiredDuration = 0.2f;


    Vector3 LookAt = Vector3.zero;
    Camera camara;
    Vector3 offset = new Vector3(0, 0, -1);
    // Start is called before the first frame update
    void Start()
    {

        camara = Camera.main;

        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCameraSmooth(LookAt);
    }

    public void MoveCameraSmooth(Vector3 position)
    {
        position.z = -9;
        float elapsedTime = 0;
        elapsedTime = elapsedTime += Time.deltaTime;

        float percentageComplete = elapsedTime / desiredDuration;

         this.transform.position = Vector3.Lerp( this.transform.position, position + offset, percentageComplete);
    }
    public void MoveCameraTo(Vector3 position)
    {
        LookAt = position;
    }
}
