using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigator : MonoBehaviour
{

    // Start is called before the first frame update
    public static void LoadCreateAccount()
    {
        SceneManager.LoadScene("CreateAccount");
    }

    public static void LoadMyWorlds()
    {
        SceneManager.LoadScene("MyWorlds");
        Debug.Log("Loading MyWorlds");
    }

    public static void LoadGame()
    {
        SceneManager.LoadScene("MainMap");
    }
    public static void LoadChat()
    {
        SceneManager.LoadScene("Chat");
    }

    public static void LoadCharacterCreator()
    {
        SceneManager.LoadScene("CharacterCreator");
    }

    public static void LoadOnboarding()
    {
        SceneManager.LoadScene("Onboarding");
    }
}
