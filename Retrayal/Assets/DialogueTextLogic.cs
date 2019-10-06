using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextLogic : MonoBehaviour
{

    float fadeTimer = 0f;
    public float fadeInterval = 1f;

    float moveTimer = 0f;
    public float moveInterval = 1.5f;
    public float moveDist = 2f;
    Vector3 origPos;
    Text txt;
    Color origCol;
    Color finCol;
    DialogueController dc;
    int state = 0; //0 fading in; 1 existance; 2 fading out

    public float existInterval = 3f;
    float existTimer = 0f;

    public float dialogueInterval = 1.5f;
    float dialogueTimer = 0f;

    bool nextDialogue = false;

    // Start is called before the first frame update
    void Start()
    {
        origPos = transform.position;
        txt = GetComponent<Text>();
        origCol = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);
        finCol = txt.color;
        txt.color = origCol;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (moveTimer < moveInterval) moveTimer += Time.deltaTime;
        if (dialogueTimer < dialogueInterval) dialogueTimer += Time.deltaTime;
        

        switch (state)
        {
            case 0:
                if (fadeTimer < fadeInterval) { fadeTimer += Time.deltaTime; }
                else { state = 1; }
                txt.color = Color.Lerp(origCol, finCol, fadeTimer / fadeInterval);
                break;
            case 1:
                if (existTimer < existInterval) { existTimer += Time.deltaTime; }
                else { state = 2; fadeTimer = 0f; }
                break;
            case 2:
                if (fadeTimer < fadeInterval) { fadeTimer += Time.deltaTime; }
                else { Destroy(gameObject); }
                txt.color = Color.Lerp(finCol, origCol, fadeTimer / fadeInterval);
                break;
            default:
                break;
        }

        transform.position = origPos + (1 - Mathf.Pow(moveTimer / moveInterval, 1f/3f)) * moveDist * Vector3.down;
        
        if (!nextDialogue && dialogueTimer >= dialogueInterval)
        {
            dc.NextDialogue();
            nextDialogue = true;
        }

    }
    public void setDialogueController(DialogueController newdc)
    {
        dc = newdc;
    }
}
