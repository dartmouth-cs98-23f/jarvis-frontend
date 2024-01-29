using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharacterSummaryManager : MonoBehaviour
{
    public Text username;
    public Image userImage;
    public Text userSummary;


    // Start is called before the first frame update
    void Start()
    {
        // TODO: Connect to backend to get user data
        username.GetComponent<Text>().text = "@" + "testUsername";
        userSummary.GetComponent<Text>().text = "You are a compassionate, wise, and brave Jedi Master. You're known for your strategic mind, moral integrity, mentorship skills, patience, and a subtle sense of humor. You are a compassionate, wise, and brave Jedi Master. You're known for your strategic mind, moral integrity, mentorship skills, patience, and a subtle sense of humor.";
        userImage.sprite = Resources.Load<Sprite>("Shapes/obi_wan_kenobi");
    }
}
