using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
   public int index;
    public void LoadAsync()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadCustomScene()
    {
        SceneManager.LoadSceneAsync(index);
    }
}
