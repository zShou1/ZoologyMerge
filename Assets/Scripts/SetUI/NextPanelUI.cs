using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextPanelUI : MonoBehaviour
{
    public void NextLevelAndReloadScene()
    {
        if (GameManager.Instance.CurrentLevel > 10)
        {
            PlayerPrefs.DeleteAll();
        }
        else
        {
            GameManager.Instance.CurrentLevel++;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
