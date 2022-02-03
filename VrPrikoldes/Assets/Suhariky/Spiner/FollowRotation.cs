using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    [SerializeField] Transform other;
    [SerializeField] float scale = 1;

    enum cords
    {
        x,
        y,
        z
    }

    [SerializeField] cords selfCord;
    [SerializeField] cords otherCord;


    Vector3 newRot;

    private void Update()
    {
        newRot = transform.rotation.eulerAngles;
        newRot[(int)selfCord] = other.rotation.eulerAngles[(int)otherCord] * scale;
        transform.rotation = Quaternion.Euler(newRot);        
    }
}
