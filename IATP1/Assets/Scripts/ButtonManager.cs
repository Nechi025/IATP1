using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{

    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("BaseLevel");
    }

    public void OnLeaveGameButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnCreditGameButtonClicked()
    {
        SceneManager.LoadScene("Credits");
    }
    public void OnExitGameButtonClicked()
    {
        Application.Quit();

        Debug.Log("CerrandoJuego");
    }
}