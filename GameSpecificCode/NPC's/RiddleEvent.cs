using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiddleEvent : MonoBehaviour {

    /* Reference everything in the code
     * Code the logic
     * Test
     */

    //Some Refernces
    public RectTransform Panel;

    public Text Question_Text;

    public Text[] Answers_Text = new Text[6];

    public GameObject[] Buttons_Btn = new GameObject[6];

    public Dropdown _AnswerOptions;

    public int choosenAnswer;

    private int _question;

    //Questions and Possible answers
    private string[,] riddle = new string[,] 
    {
        {"A man is standing in front of a painting of a man, and he tells us the following: 'Brothers and sisters have I none, but this mans father is my fathers son'. Who is on the painting?",
            "His father",
            "He himself",
            "His son",
            "NULL",
            "NULL",
            "NULL",
        },
        {"It is dark in my bedroom and I want to get two socks of the same color from my drawer, which contains 24 red and 24 blue socks. How many socks do I have to take from the drawer to get at least two socks of the same color?",
            "2",
            "3",
            "24",
            "47",
            "48",
            "NULL",
        },
        {"A hunter leaves his cabin early in the morning and walks one mile due south. Here he sees a bear and starts chasing it for one mile due east before he is able to shoot the bear. After shooting the bear, he drags it one mile due north back to his cabin where he started that morning. What color is the bear?",
            "Black",
            "Brown",
            "White",
            "Grey",
            "NULL",
            "NULL",
        },
        {"A certain street contains 100 buildings. They are numbered from 1 to 100. How many times does the digit 9 occur in these numbers?",
            "10",
            "11",
            "19",
            "20",
            "NULL",
            "NULL",
        },
        {"A Cretan named Epimenides once said to a fellow Cretan: 'All Cretans are liars'. Assuming that a liar always lies and someone who is not a liar always tells the truth, which of the following statements is true?",
            "Epimenides is a liar.",
            "Epimenides is not a liar.",
            "It is indeterminable whether Epimenides is a liar or not.",
            "NULL",
            "NULL",
            "NULL",
        },

    };

    //Correct Answers here
    private int[] correctAnswer = new int[5] 
    {
        3, 2, 3, 4, 1
    };

	// Use this for initialization
	void Start ()
    {
        //Debug.Log(riddle.Length); //Is basically row * column or every item in the array
        //Debug.Log(riddle.GetLength(0)); //Gets row
        //Debug.Log(riddle.GetLength(1)); //Gets column
    }

    //Handles choosing answer from the UI
    public void ChooseAnswer(int answer)
    {
        choosenAnswer = answer;
    }

    //This is called when we want to create/recreate the same data in UI elements
    public void GenerateRiddle(int value)
    {
        _question = value;

        int nullCount = 0;

        Question_Text.text = riddle[value, 0];

        _AnswerOptions.options.Clear();

        _AnswerOptions.options.Add(new Dropdown.OptionData() { text = "Choose Answer" });

        int j = 1;

        for (int i = 0; i < Answers_Text.Length; i++)
        {
            Answers_Text[i].text = riddle[value, j];

            //We dome some clean up here
            if (Answers_Text[i].text == "NULL")
            { 
                Buttons_Btn[i].SetActive(false);
                nullCount++;
            }
            else
            {
                _AnswerOptions.options.Add(new Dropdown.OptionData() { text = riddle[value, j] });
            }

            j++;
        }

        //Do some UI resizing
        if (nullCount == 1)
        {
            Panel.sizeDelta = new Vector2(300, 370);
        }
        else if (nullCount == 2)
        {
            Panel.sizeDelta = new Vector2(300, 330);
        }
        else if (nullCount == 3)
        {
            Panel.sizeDelta = new Vector2(300, 290);
        }
        else if (nullCount == 4)
        {
            Panel.sizeDelta = new Vector2(300, 250);
        }
    }

    //Finally We check for answer
    public void CheckAnswer()
    {
        if (choosenAnswer != 0)
        {
            if (choosenAnswer == correctAnswer[_question])
            {
                Debug.Log("The answer is correct homie!"); //This needs to be more interesting after polish
            }
            else
            {
                Debug.Log("The answer is incorrect You did bihh!"); //As well as this
            }
        }
    }
}
