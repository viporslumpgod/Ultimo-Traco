using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class itemSlot : MonoBehaviour, IPointerClickHandler
{
    //======ITEM DATA=======//
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;

    //======ITEM SLOT=======//
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;


    public GameObject selectedShader;
    public bool thisItemSelected;

    private MenuManager menuManager;

   private void Start()
    {
        menuManager = GameObject.Find("IventoryCanvas").GetComponent<MenuManager>();
    }

    public void AddItem(string itemName, int quantity, Sprite itemSprite)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemSprite = itemSprite;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;
        itemImage.sprite = itemSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }

    }

    public void OnLeftClick()
    {
        menuManager.DeselectAllSlots();
        selectedShader.SetActive(true);
        thisItemSelected = true;
       
    }

    public void OnRightClick()
    {

    }
}
