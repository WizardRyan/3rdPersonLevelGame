using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{

    public static bool isPaused = false;
    [SerializeField] GameObject pausePanel;
    [SerializeField] CinemachineFreeLook cam;
    [SerializeField] Slider horizontalSlider;

    // Start is called before the first frame update
    void Start()
    {
        //isPaused = !pausePanel.activeInHierarchy;
        horizontalSlider.value = cam.m_XAxis.m_MaxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            if (isPaused)
            {
                UpdateHorizontalSensitivity();
                Unpause();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
    }

    public void UpdateHorizontalSensitivity()
    {
        cam.m_XAxis.m_MaxSpeed = horizontalSlider.value;
    }
}
