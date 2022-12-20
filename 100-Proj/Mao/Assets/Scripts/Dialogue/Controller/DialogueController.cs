using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;

    bool canTalk = false;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //DialogueUI.setAction(false);
            //
            canTalk = false;
        }
    }

    private void Update()
    {
        if (canTalk && Input.GetMouseButtonDown(1))
        {
            OpenDialogue();    
        }
    }

    void OpenDialogue()
    {

    }
}
