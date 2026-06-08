using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UiExit : MonoBehaviour
{
    public void LoadExit()
    {
        SceneManager.LoadScene("UiScene");
    }
}
