using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_Update : MonoBehaviour
{
    private Text text;

    // Start is called before the first frame update
    private float highScore=0;
    void Start()
    {
        
    }
    private void Awake()
    {
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        ShowScore(text);
    }

    void ShowScore(Text text)
    {
        if (Game_Manager.Instance.gameScore > highScore)
        {
            highScore = Game_Manager.Instance.gameScore;
            text.text = string.Format($"Score: {highScore:f0}");
        }
    }
}
