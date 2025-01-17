using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CustomerScript : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public string[] dialogue;
    public Text objectName;
    [SerializeField] private Slider timerSlider;
    private int index = 0;
    public float wordSpeed = 0.1f;
    public bool isTalking = false;
    public bool isFinishDialogue = false;
    private bool isOrderReceived = false;
    private bool isFoodReceived = false;
    private PlayerScript player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        StartCoroutine(TimerDelay());
    }

    private IEnumerator TimerDelay()
    {
        yield return new WaitForSeconds(3);
        timerSlider.gameObject.SetActive(true);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void NextLine()
    {
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            ResetText();
        }
    }

    private void ResetText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
        isTalking = false;
    }

    private void Update()
    {
        if (dialogueText.text == dialogue[index])
        {
            isTalking = false;
        }
    }

    public void OnInteract()
    {
        if (!isOrderReceived)
        {
            isOrderReceived = true;
            timerSlider.gameObject.SetActive(false);
            StopCoroutine(TimerDelay());
            StartCoroutine(TimerDelay());
            player.AddOrder("burger");
        }
        else
        {
            if (!isTalking)
            {
                isTalking = true;
                InteractAction();
            }
        }
    }

    private void InteractAction()
    {
        // todo: fix this bug: name not show on dialog name
        // objectName.text = gameObject.name;
        if (dialoguePanel.activeInHierarchy)
        {
            NextLine();
        }
        else
        {
            dialoguePanel.SetActive(true);
            StartCoroutine(Typing());
        }
    }
}
