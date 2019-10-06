using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    List<Text> Dialogue = new List<Text>();

    int index = 0;
    Text currDialogue = null;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Text txt = transform.GetChild(i).GetComponent<Text>();
            if (!Equals(txt, null))
            {
                Dialogue.Add(txt);
                txt.gameObject.SetActive(false);
                txt.GetComponent<DialogueTextLogic>().setDialogueController(this);
            }
        }
        if (Dialogue.Count > 0)
        {
            currDialogue = Dialogue[index];
            currDialogue.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void NextDialogue()
    {
        if (Dialogue.Count > index+1)
        {
            index = index + 1;
            currDialogue = Dialogue[index];
            currDialogue.gameObject.SetActive(true);
        }
    }
}
