using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetController : MonoBehaviour
{
    public bool isMine;
    

    private void OnDestroy()
    {
        if (isMine)
        {
            MainController.instance.CreateSmoke(transform.position);
        }
    }
}
