using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class inimigo : MonoBehaviour
{
    [SerializeField] public virtual int vida { get; protected set; } = 10000000;

    public virtual void levaDano(int dano)
    {
       
        vida -= dano;
        
    }

    public void Die()
    {
        Destroy(gameObject);
    }


}
