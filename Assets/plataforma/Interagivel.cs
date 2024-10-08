using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interagivel : MonoBehaviour
{
    public UnityEvent OnInteract;
    [SerializeField]GameObject PressEInterFace;
    [SerializeField] float DistanceInterface = 1.0f;

    private void Awake()
    {
        if (PressEInterFace == null)
        {
            PressEInterFace = Resources.Load<GameObject>("Press E");
            PressEInterFace = Instantiate(PressEInterFace);
            PressEInterFace.transform.SetParent(this.transform);
            PressEInterFace.transform.localPosition = Vector3.up * DistanceInterface;
        }
        PressEInterFace.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<player>() != null)
        {
            PressEInterFace.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<player>() != null)
        {
            PressEInterFace.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<player>() != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OnInteract.Invoke();
            }
        }
    }
}
