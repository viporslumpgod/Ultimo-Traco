using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image TotalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private void Start()
    {
        TotalHealthBar.fillAmount = playerHealth.currentHealth / 500;
    }

    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 500;
    }

}
