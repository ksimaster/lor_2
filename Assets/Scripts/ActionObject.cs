using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Actions;

public class ActionObject: MonoBehaviour
{
    public float speedMove;
    public float timeToDeath;
    public Animator anim;
    public float coordinateAccuracy;
    private bool isMove;
    private Vector3 unitPoint;
    private Vector3 targetPoint;
    private int idSelectObject;
    public Material selectMaterial;
    public Material targetMaterial;

    private void Start()
    {
        unitPoint = gameObject.transform.position;
        targetPoint = gameObject.transform.position;
        Debug.Log(gameObject.transform.GetChild(2).gameObject.GetInstanceID());
    }

    void FixedUpdate()
    {
        ActionManager.AddListener<SelectObjectActionData>((data) => {
            unitPoint.x = data.x;
            unitPoint.z = data.z;
            idSelectObject = data.id;
        });
        if (gameObject.transform.GetChild(2).gameObject.GetInstanceID() == idSelectObject)
        {
            ActionManager.AddListener<SelectTargetActionData>((data) => {
                targetPoint.x = data.x;
                targetPoint.z = data.z;
            });
        }

        //Debug.Log(gameObject.transform.GetChild(2).gameObject.GetInstanceID() == idSelectObject);

        if (gameObject.transform.GetChild(2).gameObject.GetInstanceID() == idSelectObject
          && gameObject.transform.position != targetPoint) 
        {
            StartMove();
        }
        if (
            gameObject.transform.position == targetPoint 
            || Mathf.Abs(gameObject.transform.position.x - targetPoint.x) <= coordinateAccuracy 
            || Mathf.Abs(gameObject.transform.position.z - targetPoint.z) <= coordinateAccuracy)
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
        //ActionManager.TriggerEvent(new SelectTargetActionData(0, 0));

    }

    public void ChooseTarget()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0].color = targetMaterial.color;

    }

    public void SelectObject()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0].color = selectMaterial.color;
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
    }






}
