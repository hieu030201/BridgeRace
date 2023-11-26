using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public int level;
    public void RestartButton()
    {
        SceneManager.LoadScene("SampleScene");

    }
    public void NextLevel()
    {
        SceneManager.LoadScene("Level" + level.ToString());
    }
}
