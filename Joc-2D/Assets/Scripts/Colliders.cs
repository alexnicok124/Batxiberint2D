using UnityEngine;
using UnityEngine.SceneManagement;

public class winMessage : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Hit");
        if (collision.tag == "Player") 
        {
            SceneManager.LoadScene("MenuVictoria");
        }       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit");
        if (collision.tag == "Player")
        {
            SceneManager.LoadScene("MenuVictoria");
        }
    }
}
