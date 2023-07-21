using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider != null)
        {
            StringTag stringTagComponent = collider.GetComponent<StringTag>();

            if (stringTagComponent != null)
            {
                if (stringTagComponent.stringTag.Equals("Ball"))
                {
                    gameManager.currentBall = collider.gameObject;
                    StartCoroutine(gameManager.OnBallThrown(5f));
                }
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

}
