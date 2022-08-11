using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLookAtCam : MonoBehaviour
{

    void Update()
    {
        if(gameObject.transform.rotation != Camera.main.transform.rotation)
        {
            gameObject.transform.rotation = Camera.main.transform.rotation;
        }
    }
}
