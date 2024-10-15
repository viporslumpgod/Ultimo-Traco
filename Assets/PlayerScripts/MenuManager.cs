using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject Menu;
    private bool menuActivaed;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && menuActivaed)
        {
           Menu.SetActive(false);
            menuActivaed = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && !menuActivaed)
        {
           Menu.SetActive(true);
            menuActivaed = true;
        }
          
      


    }
}
