using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    [SerializeField] Vector3 circlePos;

    public GameObject[] Circles;
    public GameObject healthPrefab,gameoverPrefab;
    GameObject beforeCircle;

    Transform healthLayout;

    Text levelText;

    public int level = 1;

    List<GameObject> healthObject = new List<GameObject>();
    [SerializeField] int health;

    private void Awake() {
        levelText = GameObject.FindWithTag("LevelText").GetComponent<Text>();
        healthLayout = GameObject.FindWithTag("Health").transform;
    }

    private void Start() 
    {
        beforeCircle = null;
        level = 1;

        SetHealth();
    }

    public void SpawnCircle()
    {
        GameObject[] circles = GameObject.FindGameObjectsWithTag("Circle");
        foreach(var circle in circles)
        {
            if(circle.transform.position.y <= -10)
            {
                circle.transform.DOScale(0,1f).OnComplete(()=>{Destroy(circle);});
            }
            else
            {
                circle.transform.DOMoveY(circle.transform.position.y - 2f,1f).SetEase(Ease.OutBounce);
            }
        }

        if(beforeCircle != null)
        {
            foreach(Transform circleChild in beforeCircle.transform)
            {
                circleChild.gameObject.GetComponent<MeshRenderer>().enabled = true;
                circleChild.gameObject.GetComponent<MeshRenderer>().material.DOColor(BallHandler.ballColor,0.5f);
            }
        }

        GameObject cloneCircle = Instantiate(Circles[Random.Range(0,Circles.Length)],circlePos,Quaternion.identity);
        cloneCircle.transform.DOMoveY(0,1f).SetEase(Ease.OutBounce);

        SetCircleTargets(cloneCircle);

        beforeCircle = cloneCircle;
    }

    void SetCircleTargets(GameObject clone)
    {
        List<Transform> circleChildList = new List<Transform>();
        foreach(Transform circleChild in clone.transform)
        {
            circleChildList.Add(circleChild);
        }

        for(int i = 0; i < level; i++)
        {
            int randomNumber = Random.Range(0,circleChildList.Count);
            if(!circleChildList[randomNumber].gameObject.GetComponent<MeshRenderer>().enabled)
            {
                circleChildList[randomNumber].gameObject.tag = "Target";
                circleChildList[randomNumber].gameObject.GetComponent<MeshRenderer>().enabled = true;
                circleChildList[randomNumber].gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.black,0.5f);
            }
            else
            {
                i--;
            }
        }

        BallHandler ballHandler = FindObjectOfType<BallHandler>();
        ballHandler.ballCount = level;
        levelText.text = "LEVEL "+level.ToString();
        level++;
    }

    void SetHealth()
    {
        for(int i = 0; i < health; i++)
        {
            GameObject clone = Instantiate(healthPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            clone.transform.SetParent(healthLayout, false);

            healthObject.Add(clone);
        }
    }

    public void decreaseHealth()
    {
        healthObject[0].transform.DOScale(0,0.25f).OnComplete(()=>{Destroy(healthObject[0]); healthObject.RemoveAt(0);});
        BallHandler ballHandler = FindObjectOfType<BallHandler>();
        ballHandler.ballCount++;
        health--;

        if(health <= 0)
        {
            Destroy(ballHandler);
            Instantiate(gameoverPrefab);
            StartCoroutine(WaitForSceneLoad());
        }
    }
    private IEnumerator WaitForSceneLoad() {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
