using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SpinerScript : MonoBehaviour
{
    [SerializeField] Transform CircleWraper;
    [SerializeField] Transform HandleWraper;
    [SerializeField] Transform HolderWraper;
    Interactable HolderInteract;

    [Header("Spiner Events")]
    [SerializeField] UnityEvent[] Events;//= new UnityEvent[3];
    [SerializeField] UnityEvent OnChangeEvent;

    [SerializeField] int CurrentIndex = -1;
    [SerializeField] float handleDist = 0.2f;

    private void Awake()
    {
        HolderInteract = HolderWraper.gameObject.GetComponent<Interactable>();
    }

    private void Update()
    {
        ChangeAAngle();
        CheckRotation();
    }

    private float newAngle;
    void ChangeAAngle()
    {
        newAngle = (HolderWraper.eulerAngles - transform.localEulerAngles).z;
        //Debug.Log(newAngle);

        CircleWraper.localEulerAngles = Vector3.forward * newAngle;
        HandleWraper.localEulerAngles = Vector3.forward * newAngle;
    }

    public void ReturnHandele()
    {
        HolderWraper.position = HandleWraper.position;
        HolderWraper.rotation = HandleWraper.rotation;
    }



    public void ChekDist(Hand hand)
    {
        if (Vector3.Distance(HolderWraper.position, HandleWraper.position) > handleDist)
        {
            DropSmth(hand);
        }
    }

    void DropSmth(Hand hand)
    {
        Debug.Log(HolderInteract);
        if (!HolderInteract || !HolderInteract.attachedToHand)
        {
            ReturnHandele();
            return;
        }

        hand.DetachObject(HolderWraper.gameObject);
        hand.HoverUnlock(HolderInteract);
        ReturnHandele();
    }

    private void CheckRotation()
    {
        //OnChangeEvent.Invoke();
        float SectorSize = 360 / Events.Length;
        
        float Angle = (360 - newAngle) + SectorSize/2;
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
