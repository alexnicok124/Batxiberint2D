using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManagerScript : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pausedMenuUI;
    public GameObject Player;

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausedMenuUI.activeInHierarchy)
            {
                Continue();
            }
            else if (!pausedMenuUI.activeInHierarchy)
            {
                Pause();
            }
        }
    }

    public void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Sortir()
    {
        Application.Quit();
    }

    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {

        pausedMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Player.GetComponent<MovementPlayerScript>().enabled = false;
        Player.GetComponent<Attack>().enabled = false;
        Player.GetComponent<Pointing>().enabled = false;
        Player.GetComponent<Stamina>().enabled = false;
        Player.GetComponent<Health>().enabled = false;
        Player.GetComponent<Dashing>().enabled = false;
        Player.GetComponent<WeaponManager>().enabled = false;
        Player.GetComponent<Shooting>().enabled = false;
        Player.GetComponent<UpdateBullets>().enabled = false;
    }

    public void Continue()
    {
        pausedMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        Player.GetComponent<MovementPlayerScript>().enabled = true;
        Player.GetComponent<Attack>().enabled = true;
        Player.GetComponent<Pointing>().enabled = true;
        Player.GetComponent<Stamina>().enabled = true;
        Player.GetComponent<Health>().enabled = true;
        Player.GetComponent<Dashing>().enabled = true;
        Player.GetComponent<WeaponManager>().enabled = true;
        Player.GetComponent<Shooting>().enabled = true;
        Player.GetComponent<UpdateBullets>().enabled = true;
    }
}
