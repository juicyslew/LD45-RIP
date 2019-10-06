using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public float grav = 15f;
    public string nextLevel;
    string thisLevel;
    string sceneToLoad;
    GameObject EndScreen;
    Button but;
    RawImage im;
    Text txt;

    int state = 0; //0 - fadein wait; 1 - fadein; 2 - normal; 3 - fadeout
    float prefadeInterval = .5f;
    float prefadeTimer = 0f;

    float fadeInterval = .5f;
    float fadeTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        thisLevel = SceneManager.GetActiveScene().name;
        sceneToLoad = nextLevel;
        prefadeTimer = 0f;
        fadeTimer = 0f;
        EndScreen = GameObject.FindGameObjectWithTag("UIScreen");
        but = EndScreen.GetComponentInChildren<Button>();
        im = EndScreen.GetComponentInChildren<RawImage>();
        txt = but.GetComponentInChildren<Text>();
        but.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case 0:
                Time.timeScale = 0f;
                prefadeTimer += Time.unscaledDeltaTime;
                if (prefadeTimer > prefadeInterval)
                {
                    state = 1;
                    prefadeTimer = 0f;
                }
                break;
            case 1:
                Time.timeScale = 1f;
                fadeTimer += Time.deltaTime;
                im.color = new Color(im.color.r, im.color.g, im.color.b, 1 - (fadeTimer / fadeInterval));
                if (fadeTimer > fadeInterval)
                {
                    state = 2;
                    fadeTimer = 0f;
                }
                break;
            case 2:
                break;
            case 3:
                Time.timeScale = 0f;
                fadeTimer += Time.unscaledDeltaTime;
                im.color = new Color(im.color.r, im.color.g, im.color.b, (fadeTimer / fadeInterval));
                if (fadeTimer > fadeInterval)
                {
                    LoadNextLevel();
                }
                break;
            default:
                break;
        }
    }

    public void Captured()
    {
        but.gameObject.SetActive(true);
        txt.text = "You were Captured!";
        but.onClick.AddListener(StartFadeOut);
        sceneToLoad = thisLevel;
    }

    public void EndLevel()
    {
        but.gameObject.SetActive(true);
        but.onClick.AddListener(StartFadeOut);
    }

    public void StartFadeOut()
    {
        state = 3;
        fadeTimer = 0f;
        but.gameObject.SetActive(false);
        
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
