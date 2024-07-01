using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int playerHealth = 1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else Instance = this;
    }

    public void TakeDamage(int damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            YouLose();
        }
    }

    public void YouWin()
    {
        SceneManager.LoadScene("WinScreen");
        Debug.Log("WINNER");
    }

    public void YouLose()
    {
        SceneManager.LoadScene("LoseScreen");
    }

   
}
