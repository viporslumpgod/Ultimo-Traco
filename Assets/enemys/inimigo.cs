using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class inimigo : MonoBehaviour
{
    [SerializeField] public int vida { get; protected set; } = 10000000;

    public void levaDano(int dano)
    {
       
        vida -= dano;
        Debug.Log(vida);
    }

}
