using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTile : MonoBehaviour
{
    public float speedMove;


    private Vector3 objectPoint;
    private Vector3 targetPoint;

    private void Start()
    {
        objectPoint = gameObject.transform.position;
        targetPoint = gameObject.transform.position;
        PlayerPrefs.SetFloat("objectPointX", gameObject.transform.position.x);
        PlayerPrefs.SetFloat("objectPointZ", gameObject.transform.position.z);
        PlayerPrefs.SetFloat("targetPointX", gameObject.transform.position.x);
        PlayerPrefs.SetFloat("targetPointZ", gameObject.transform.position.z);

    }

    void FixedUpdate()
    {
        if (PlayerPrefs.HasKey("objectPointX") && PlayerPrefs.HasKey("objectPointZ")) 
        {
            objectPoint = new Vector3(PlayerPrefs.GetFloat("objectPointX"), gameObject.transform.position.y, PlayerPrefs.GetFloat("objectPointZ"));
            
        }
        if (PlayerPrefs.HasKey("targetPointX") && PlayerPrefs.HasKey("targetPointZ")) 
        {
            targetPoint = new Vector3(PlayerPrefs.GetFloat("targetPointX"), gameObject.transform.position.y, PlayerPrefs.GetFloat("targetPointZ"));
            
        }
        if (gameObject.transform.position != targetPoint)
        {
            gameObject.transform.LookAt(targetPoint);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPoint, speedMove * Time.deltaTime);
        }  
    }


}
