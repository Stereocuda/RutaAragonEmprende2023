using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    [SerializeField] private PersistentData_SO playerData;
    public void RestartGame()
    {
        ResetPlayerData();
        SceneManager.LoadScene(0);
    }

    private void ResetPlayerData()
    {
        playerData.energy = 100;
        playerData.level = 0;
        playerData.score = 0;

    }
}
