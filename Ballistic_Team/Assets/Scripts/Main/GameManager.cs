using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button gameStartButton;

    private void Start()
    {
        UpdateButtonState("");
        inputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
    }

    private void OnInputFieldValueChanged(string currentText)
    {
        UpdateButtonState(currentText);
    }

    private void UpdateButtonState(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            gameStartButton.interactable = false;
        }
        else
        {
            gameStartButton.interactable = true;
        }
    }

    private void OnGameStartButtonClicked()
    {
        string playerName = inputField.text;

        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();

        Debug.Log("플레이어 이름 저장 됨: " + playerName);

        SceneManager.LoadScene("Test");
    }
}