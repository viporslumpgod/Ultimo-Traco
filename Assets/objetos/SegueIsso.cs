using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegueIsso : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        Camerasegueessaporraaqui();
    }

    public void Camerasegueessaporraaqui()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, Camera.main.transform.position.z);
    }
}
