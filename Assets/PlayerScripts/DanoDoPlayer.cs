using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoDoPlayer : MonoBehaviour
{
   public Espatula espatula;
    
    void Start()
    {
        espatula = GetComponentInParent<Espatula>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<inimigo>() != null)
        {
            collision.GetComponent<inimigo>().levaDano(espatula.damage);
        }
    }
}
