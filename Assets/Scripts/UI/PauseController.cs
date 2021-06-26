using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject pauseMenu;
    public static bool Paused { get; private set; }
    private void Awake()
    {
        DisactivatePauseMenu();
    }
    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (Paused)
            {
                DisactivatePauseMenu();
            }
            else
            {
                ActivatePauseMenu();
            }
        }
    }
    public void ActivatePauseMenu()
    {
        if (HUD != null)
        {
            HUD.SetActive(false);
        }
        pauseMenu.SetActive(true);
        Paused = true;
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void DisactivatePauseMenu()
    {
        pauseMenu.SetActive(false);
        Paused = false;
        if (HUD != null)
        {
            HUD.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        Time.timeScale = 1f;
    }
}
