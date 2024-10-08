using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigobarril : inimigo
{

    // Start is called before the first frame update
    void Start()
    {
        vida = 10;
       
    }

    // Update is called once per frame
    void Update()
    {
        Morre();
    }


    public void Morre()
    {
        if (vida < 1.5) 
        {
        Destroy(gameObject);
        
        
        }
    }
}
