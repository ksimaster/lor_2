using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionObject: MonoBehaviour
{
    public float speedMove;
    public float timeToDeath;
    public Animator anim;
    public float coordinateAccuracy;
    private bool isMove;
    private Vector3 unitPoint;
    private Vector3 targetPoint;

    private void Start()
    {
        unitPoint = gameObject.transform.position;
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
            unitPoint = new Vector3(PlayerPrefs.GetFloat("objectPointX"), gameObject.transform.position.y, PlayerPrefs.GetFloat("objectPointZ"));
            
        }
        if (PlayerPrefs.HasKey("targetPointX") && PlayerPrefs.HasKey("targetPointZ")) 
        {
            targetPoint = new Vector3(PlayerPrefs.GetFloat("targetPointX"), gameObject.transform.position.y, PlayerPrefs.GetFloat("targetPointZ"));
            
        }
        if (gameObject.transform.position != targetPoint && Mathf.Abs(gameObject.transform.position.x - targetPoint.x) > coordinateAccuracy && Mathf.Abs(gameObject.transform.position.z - targetPoint.z) > coordinateAccuracy)
        {
            StartMove();
        }
        if (gameObject.transform.position == targetPoint || Mathf.Abs(gameObject.transform.position.x - targetPoint.x) <= coordinateAccuracy || Mathf.Abs(gameObject.transform.position.z - targetPoint.z) <= coordinateAccuracy)
        {
            StopMove();
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

    public void StartMove()
    {
        gameObject.transform.LookAt(targetPoint);
        isMove = true;
        anim.SetBool("Move", isMove);
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPoint, speedMove * Time.deltaTime);

    }
    public void StopMove()
    {
        isMove = false;
        anim.SetBool("Move", isMove);

    }

    public void ChooseTarget()
    {

    }







}
