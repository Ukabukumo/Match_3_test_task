using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBtnScr : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
