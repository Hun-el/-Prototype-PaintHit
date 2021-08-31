using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColorChanger : MonoBehaviour
{
    bool hasEntered = false;

    private void OnCollisionEnter(Collision other) 
    {
        if(!hasEntered)
        {
            hasEntered = true;
            if(other.gameObject.tag != "Target")
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;

                Camera.main.DOShakePosition(0.5f,1f);

                GameManager gameManager = FindObjectOfType<GameManager>();
                gameManager.decreaseHealth();

                gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 50,ForceMode.Impulse);
                this.gameObject.transform.DOScale(0,2f).OnComplete(()=>{Destroy(this.gameObject);});
            }
            else
            {
                gameObject.GetComponent<SphereCollider>().enabled = false;

                gameObject.transform.parent = other.gameObject.transform;
                other.gameObject.tag = "Untagged";

                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
                other.gameObject.GetComponent<MeshRenderer>().material.DOColor(BallHandler.ballColor,0.5f);

                Destroy(this.gameObject);
            }
        }
    }
}
