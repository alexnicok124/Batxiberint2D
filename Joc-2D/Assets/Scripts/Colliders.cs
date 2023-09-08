using UnityEngine;
using UnityEngine.SceneManagement;

public class winMessage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            SceneManager.LoadScene("MenuVictoria");
        }       
    }
}
