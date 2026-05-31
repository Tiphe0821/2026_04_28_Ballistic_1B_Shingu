using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    public GameObject Panel;
    public Button startButton;

    public Button settingsButton;

    [Header("딜레이 시간")]
    public float sceneDelay = 1.0f;
    public float panelDelay = 0.5f;
    void Start()
    {
        if (startButton != null)
        {
            startButton.Select();
        }
    }
    public void GameButton()
    {
        Invoke("LoadSceneDelayed", sceneDelay);
    }
    private void LoadSceneDelayed()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void OpenPanel()
    {
        Invoke("OpenPanelDelayed", panelDelay);
    }
    private void OpenPanelDelayed()
    {
        Panel.SetActive(true);
    }
    public void ClosePanel()
    {
        Panel.SetActive(false);

        if (settingsButton != null)
        {
            settingsButton.Select();
        }
    }
}
