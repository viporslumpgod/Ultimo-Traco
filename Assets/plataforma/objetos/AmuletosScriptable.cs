using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Amuleto", menuName = "itens/Amuletos", order = 1)]
public class AmuletosScriptable : ScriptableObject
{
    public int duração;
    int vidaAdicional = 10;

    public void Efeito()
    {
      
    }
}
