using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public float speedCam; 
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            gameObject.transform.position += new Vector3(0,0,speedCam);
        }
        if (Input.GetKey(KeyCode.S))
        {
            gameObject.transform.position += new Vector3(0, 0, -speedCam);
        }
        if (Input.GetKey(KeyCode.D))
        {
            gameObject.transform.position += new Vector3(speedCam, 0, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            gameObject.transform.position += new Vector3(-speedCam, 0, 0);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            gameObject.transform.position += new Vector3(0, -speedCam, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            gameObject.transform.position += new Vector3(0, speedCam, 0);
        }
    }
}
