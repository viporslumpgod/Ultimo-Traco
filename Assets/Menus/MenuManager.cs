using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject Menu;
    private bool menuActivaed;
    
    public GameObject SlotPrefab;
    public Transform InventorySlots;
    public itemSlot[] itemSlot;

    private void Awake()
    {
       
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && menuActivaed)
        {
            Time.timeScale = 1;
            Menu.SetActive(false);
            menuActivaed = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && !menuActivaed)
        {
            Time.timeScale = 0;
           Menu.SetActive(true);
            menuActivaed = true;
        }
          
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {

            if (itemSlot[i].isFull == false)
            {
                itemSlot[i].AddItem(itemName, quantity, itemSprite);
                return;
            }

        }
    }

    public void DeselectAllSlots()
    {
        for(int i = 0;i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }

}
