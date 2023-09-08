using UnityEngine;
using UnityEngine.SceneManagement;

public class winMessage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene("MenuVictoria");
        }
    }
}
