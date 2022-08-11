using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObject: MonoBehaviour
{
    public float speedMove;
    public Animator anim;
    public float coordinateAccuracy;
    private bool isMove;
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
        if (gameObject.transform.position != targetPoint && Mathf.Abs(gameObject.transform.position.x - targetPoint.x) > coordinateAccuracy && Mathf.Abs(gameObject.transform.position.z - targetPoint.z) > coordinateAccuracy)
        {

            gameObject.transform.LookAt(targetPoint);
            isMove = true;
            anim.SetBool("Move", isMove);
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPoint, speedMove * Time.deltaTime);
        }
        if (gameObject.transform.position == targetPoint || Mathf.Abs(gameObject.transform.position.x - targetPoint.x) <= coordinateAccuracy || Mathf.Abs(gameObject.transform.position.z - targetPoint.z) <= coordinateAccuracy)
        {
            isMove = false;
            anim.SetBool("Move", isMove);
        }
        
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    public void Damage()
    {
        anim.SetTrigger("Damage");
    }
    public void Die()
    {
        anim.SetTrigger("Die");
        Destroy(gameObject, 1.5f);
    }


}
