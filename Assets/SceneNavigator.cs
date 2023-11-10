using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadCreateAccount()
    {
        SceneManager.LoadScene("CreateAccount");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("MainMap");
    }
    public void LoadChat(){
        SceneManager.LoadScene("Chat");
    }
}
