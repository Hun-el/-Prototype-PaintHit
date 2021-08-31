using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour
{
    public static Color ballColor;

    public GameObject ballPrefab;
    public GameObject ballDummy;

    public int ballCount;
    public float Ballspeed;
    bool freeShot = true;

    GameManager gameManager;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start() {
        ballColor = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
        );

        gameManager.SpawnCircle();
    }

    void Update()
    {
        if(!freeShot){ return; }
        #if UNITY_ANDROID && !UNITY_EDITOR
        if(Input.GetTouch(0).phase == TouchPhase.Began && ballCount > 0)
        {
            LaunchThrow();
        }
        #endif

        #if UNITY_STANDALONE || UNITY_EDITOR
        if(Input.GetMouseButtonDown(0) && ballCount > 0)
        {
            LaunchThrow();
        }
        #endif
    }

    void LaunchThrow()
    {
        GameObject ballClone = Instantiate(ballPrefab,ballDummy.transform.position,Quaternion.identity);
        ballClone.GetComponent<MeshRenderer>().material.color = ballColor;
        ballClone.GetComponent<Rigidbody>().AddForce(Vector3.forward * Ballspeed,ForceMode.Impulse);

        ballCount--;

        if(ballCount <= 0)
        {
            StartCoroutine(delaySpawnCircle());
        }
    }

    IEnumerator delaySpawnCircle()
    {
        freeShot = false;
        yield return new WaitForSeconds(0.5f);
        if(ballCount <= 0){
            gameManager.SpawnCircle();
            ballColor = new Color(
                Random.Range(0f, 1f), 
                Random.Range(0f, 1f), 
                Random.Range(0f, 1f)
            );
            yield return new WaitForSeconds(1f);
            freeShot = true;
        }
        else
        {
            freeShot = true;
        }
    }
}
