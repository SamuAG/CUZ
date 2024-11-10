using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenScene : MonoBehaviour
{
    public void OpenSceneByIndex(int index)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
    }

    public void OpenSceneByName(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }
}
