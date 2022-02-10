using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpinerScript : MonoBehaviour
{
    [SerializeField] Transform CircleWraper;
    [SerializeField] Transform HandleWraper;
    [SerializeField] Transform HolderWraper;

    [SerializeField] UnityEvent[] Events;
    [SerializeField] UnityEvent ClearEvent;
    [SerializeField] float usableAngel;
    private float newAngle;
    void ChangeAAngle()
    {
        newAngle = (HolderWraper.eulerAngles - transform.localEulerAngles).z;
        CircleWraper.localEulerAngles = Vector3.forward * newAngle;
        HandleWraper.localEulerAngles = Vector3.forward * newAngle;
    }

    public void ReturnHandele()
    {
        HolderWraper.position = HandleWraper.position;
    }

    private void Update()
    {
        ChangeAAngle();
        AceptRotation();
    }

    private void AceptRotation()
    {
        ClearEvent.Invoke();
        Debug.Log(Events.Length);
        float SectorSize = 360 / Events.Length;
        usableAngel = newAngle - SectorSize / 2;
    }
}
