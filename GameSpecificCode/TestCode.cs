using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestCode : MonoBehaviour {
    public RectTransform hero;
    public RectTransform fill;
    public GameObject btn;

    void Start() {
    }

    public void RunLevelChange(){
        StartCoroutine(MoveObject(3f));
    }

    public IEnumerator MoveObject(float overTime)
    {
        float startTime = Time.time;
        LevelManager levelManager = GameObject.Find("_LevelManager(Clone)").GetComponent<LevelManager>();
        Vector3 startPosition = new Vector3(hero.localPosition.x + (80*levelManager.bossLevelCounter) , hero.localPosition.y, hero.localPosition.z);
        Vector3 endPosition = new Vector3(startPosition.x+80, hero.localPosition.y, hero.localPosition.z);
        Vector2 startFill = new Vector2(fill.sizeDelta.x + (80*levelManager.bossLevelCounter), fill.sizeDelta.y);
        Vector2 endFill = new Vector2(startFill.x+80, fill.sizeDelta.y);
        while(Time.time < startTime + overTime)
        {
            hero.localPosition = Vector3.Lerp(startPosition, endPosition, (Time.time - startTime)/(overTime));
            fill.sizeDelta = Vector2.Lerp(startFill,  endFill, (Time.time - startTime)/(overTime));
            yield return null;
        }
        hero.localPosition = endPosition;
        fill.sizeDelta = endFill;
        levelManager.bossLevelCounter++;
        btn.SetActive(true);
    }

    public void ChangeLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
