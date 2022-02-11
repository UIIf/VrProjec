using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLedScript : MonoBehaviour
{
    MeshRenderer Rend;
    private void Awake()
    {
        Rend = GetComponent<MeshRenderer>();
    }
    public void ChangeColorRED()
    {
        Rend.material.color = Color.red;
    }

    public void ChangeColorGREEN()
    {
        Rend.material.color = Color.green;
    }

    public void ChangeColorBLUE()
    {
        Rend.material.color = Color.blue;
    }

    public void ChangeColorYELLOW()
    {
        Rend.material.color = Color.yellow;
    }
}
