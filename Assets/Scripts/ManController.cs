using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ManController : MonoBehaviour
{
    public bool isCanRaise;
    public Animator animator;

    public float speed;

    private void Start()
    {
        if(isCanRaise)
        {
            animator.Play("Dance");
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (MainController.instance && !MainController.instance.isPlay)
        {
            if (transform.position.x <= MainController.instance.leftUpStartZone.transform.position.x
                        || transform.position.x >= MainController.instance.rightDownStartZone.transform.position.x
                        || transform.position.z >= MainController.instance.leftUpStartZone.transform.position.z
                        || transform.position.z <= MainController.instance.rightDownStartZone.transform.position.z)
            {
                MainController.instance.RemoveMan(this);
            }
        }    
    }

    public void MoveToNewPos(Vector3 pos)
    {
        StartCoroutine(Move(pos));
    }

    private IEnumerator Move(Vector3 pos)
    {
        while (Vector3.Distance(transform.localPosition, pos) > 0.01)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, pos, speed * Time.deltaTime);

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isCanRaise)
        {
            if (other.gameObject.GetComponent<ManController>().isCanRaise)
            {
                Destroy(other.gameObject);
                MainController.instance.AddMan(other.gameObject.transform.position, true);
            }
            /*
            else if( !MainController.instance.isPlay)
            {
                try
                {
                    MainController.instance.manControllers.Remove(GetComponent<ManController>());
                }
                catch { }

                Destroy(gameObject);
            }
            */
        }
        else if (other.gameObject.CompareTag("Let"))
        {
            if (other.gameObject.TryGetComponent<LetController>( out _))
            {
                Destroy(other.gameObject);
            }
            MainController.instance.RemoveMan(this);
        }
    }

    public void PlayAnimation(string animationName)
    {
        try
        {
            animator.Play(animationName);
        }
        catch { }
    }
}
