using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuScript : MonoBehaviour
{

    public static bool isPaused = false;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] CinemachineFreeLook cam;
    [SerializeField] Slider horizontalSlider;
    [SerializeField] Slider verticalSlider;
    [SerializeField] Button resumeButton;

    // Start is called before the first frame update
    void Start()
    {
        //begin development relevant code
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
        }
        //end development relevant code

        horizontalSlider.value = cam.m_XAxis.m_MaxSpeed;
        verticalSlider.value = cam.m_YAxis.m_MaxSpeed * 180;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame))
        {
            if (isPaused)
            {
                UpdateControls();
                Unpause();
            }
            else
            {
                Pause();
            }
        }
        
        else if(Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            if (settingsPanel.activeSelf)
            {
                BackFromSettings();
            }
            else
            {
                Unpause();
            }
        }
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        isPaused = false;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        settingsPanel.SetActive(false);
        pausePanel.SetActive(true);
        resumeButton.Select();
        isPaused = true;
    }

    public void UpdateHorizontalSensitivity()
    {
        cam.m_XAxis.m_MaxSpeed = horizontalSlider.value;
    }

    public void UpdateVerticalSensitivity()
    {
        cam.m_YAxis.m_MaxSpeed = verticalSlider.value / 180;
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
        horizontalSlider.Select();
    }

    private void UpdateControls()
    {
        UpdateHorizontalSensitivity();
        UpdateVerticalSensitivity();
    }

    public void BackFromSettings()
    {
        Pause();
    }
}
