using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    GameManager gameManager;
    
    float timer = 6f;
    [SerializeField]float rotateSpeed;

    private void Start() {
        gameManager = FindObjectOfType<GameManager>();
        rotateSpeed = rotateSpeed + (gameManager.level - 2) * 5;
    }
    
    void Update()
    {
        Rotate();
        if(gameManager.level > 3)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                rotateSpeed *= -1;
                timer = 6;
            }
        }
    }

    void Rotate()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    }
}
