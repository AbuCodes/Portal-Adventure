using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreepyOldManNpc : MonoBehaviour {

    private int chooseRiddle = 1;

    private Transform _canvas;
    private GameObject _Riddle;

    public GameObject yesPanel;
    public Button yesBtn;
    public Text yesText;

    void Start()
    {
        _canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        yesPanel = _canvas.Find("YesNoPanel").gameObject;
        yesBtn = _canvas.Find("YesNoPanel").Find("Panel").Find("Yes_Button").GetComponent<Button>();
        yesText = yesPanel.transform.Find("Text").GetComponent<Text>();
        yesPanel.SetActive(false);
    }

    public void PromptPlayer()
    {
        yesPanel.SetActive(true);
        yesText.text = "Would You like to solve my riddle for a reward? \n \n Remember you will be punished if you fail.";
        yesBtn.onClick.AddListener(delegate { LaunchRiddle(); });
    }

    public void LaunchRiddle ()
    {
        if (!_Riddle)
        {
            _Riddle = (GameObject)Instantiate(Resources.Load("RiddlePanel"));
            _Riddle.transform.SetParent(_canvas, false);
            _Riddle.GetComponent<RiddleEvent>().GenerateRiddle(chooseRiddle);
        }
        else
        {
            _Riddle.SetActive(true);
            _Riddle.GetComponent<RiddleEvent>().GenerateRiddle(chooseRiddle);
        }
    }
}
