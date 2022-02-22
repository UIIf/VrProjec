using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpinerScript : MonoBehaviour
{
    [SerializeField] Transform handle;
    [SerializeField] Transform handleEmulator;
    [SerializeField] Transform[] coRotation;
    Interactable handleInteract;

    [Range(0, 360)]
    [SerializeField] float angleOfset = 0;

    [Header("Spiner Events")]
    [SerializeField] UnityEvent[] Events;
    [SerializeField] UnityEvent OnChangeEvent;

    [SerializeField] int CurrentIndex = -1;
    [SerializeField] float handleDist = 0.2f;

    [Header("Gizmos efect")]
    [SerializeField] float gizmoRadius = 0.25f;
    [SerializeField] Color gizmoColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Vector3 temp = Vector3.ProjectOnPlane(new Vector3(Mathf.Cos((angleOfset + 90) * Mathf.Deg2Rad), Mathf.Sin((angleOfset + 90) * Mathf.Deg2Rad), 0).normalized * gizmoRadius, transform.forward);
        Gizmos.DrawLine(transform.position, temp + transform.position);
    }

    private void Awake()
    {
        handleInteract = handle.gameObject.GetComponent<Interactable>();
    }

    private void Update()
    {
        ChangeAAngle();
        CheckRotation();
    }

    private float newAngle;
    void ChangeAAngle()
    {
        newAngle = (handle.eulerAngles - transform.localEulerAngles).z;
        handleEmulator.localEulerAngles = Vector3.forward * newAngle;
        for (int i = 0; i < coRotation.Length; i++)
        {
            coRotation[i].localEulerAngles = Vector3.forward * newAngle;
        }
    }

    public void ReturnHandele()
    {
        handle.position = handleEmulator.position;
        handle.rotation = handleEmulator.rotation;
    }



    public void ChekDist(Hand hand)
    {
        if (Vector3.Distance(handle.position, handleEmulator.position) > handleDist)
        {
            DropSmth(hand);
        }
    }

    void DropSmth(Hand hand)
    {
        Debug.Log(handleInteract);
        if (!handleInteract || !handleInteract.attachedToHand)
        {
            ReturnHandele();
            return;
        }

        hand.DetachObject(handle.gameObject);
        hand.HoverUnlock(handleInteract);
        ReturnHandele();
    }

    private void CheckRotation()
    {
        //OnChangeEvent.Invoke();
        float SectorSize = 360 / Events.Length;
        
        float Angle = (360 - newAngle) + SectorSize/2 + angleOfset;
        if(Angle > 360)
        {
            Angle -= 360;
        }

        int Index = (int)(Angle / SectorSize);


        if (Index != CurrentIndex)
        {
            CurrentIndex = Index;
            Debug.Log("Changed " + Index.ToString());
            Events[Index].Invoke();
        }


    }
}
