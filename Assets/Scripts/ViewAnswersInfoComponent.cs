using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewAnswersInfoComponent : MonoBehaviour
{
    public TextMeshProUGUI usernameTMP;
    public TextMeshProUGUI answerTMP;

    public void SetDetails(string username, string answer){
        answerTMP.text = answer;
        usernameTMP.text = username;
    }
}
