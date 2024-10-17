using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item : MonoBehaviour
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private Sprite sprite;

    private MenuManager menuManager;

    void Start()
    {
        menuManager = GameObject.Find("InventoryCanvas").GetComponent<MenuManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("AAAAAAAAAAAA");
            menuManager.AddItem(itemName, quantity, sprite);
            Destroy(gameObject);
        }
    }

}
