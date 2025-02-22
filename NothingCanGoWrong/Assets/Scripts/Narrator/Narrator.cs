using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Narrator : MonoBehaviour
{

    #region Singleton
    public static Narrator Instance { get; private set; }
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            sentences = new Queue<string>();
            dialogueText.text = "";
            StartDialogue(currentDialogue, dialogues[currentDialogue].rootNode);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public Animator animator;
    public Dialogue[] dialogues;
    public int currentDialogue = 0;
    public TextMeshProUGUI dialogueText;
    public float typeSpeed = 5f;
    public GameObject responseButtonPrefab;
    public Transform responseButtonContainer;
    public GameObject cutToBlack;

    private string fullText;
    private Queue<string> sentences = new Queue<string>();
    private bool isTyping = false;
    private bool dialogue2 = false;
    private bool dialogue3 = false;
    private bool dialogue4 = false;
    private bool dialogue5 = false;

    private void Update()
    {
        if (isTyping && Input.anyKeyDown)
        {
            // Show full text
            dialogueText.text = fullText;
            isTyping = false;
        }
        if (PlayerStats.instance.technologyLevel > 1 && !dialogue2)
        {
            currentDialogue = 1;
            StartDialogue(currentDialogue, dialogues[currentDialogue].rootNode);
            dialogue2 = true;
        }
        if (StarSystem.instance.planetsDestroyed == 1 && !dialogue3)
        {
            currentDialogue = 2;
            StartDialogue(currentDialogue, dialogues[currentDialogue].rootNode);
            dialogue3 = true;
        }
        if (StarSystem.instance.planetsDestroyed == 2 && !dialogue4)
        {
            currentDialogue = 3;
            StartDialogue(currentDialogue, dialogues[currentDialogue].rootNode);
            dialogue4 = true;
        }
        if (StarSystem.instance.planetsDestroyed == 3 && !dialogue5)
        {
            currentDialogue = 4;
            StartDialogue(currentDialogue, dialogues[currentDialogue].rootNode);
            dialogue5 = true;

            // Cut to Game Over Scene
        }
    }

    public void StartDialogue(int dialogueNumber, DialogueNode node)
    {
        animator.SetBool("isOpen", true);
        // Set dialogue title and body dialogueText
        dialogueText.text = "";
        fullText = "";
        fullText = node.dialogueText;
        StartCoroutine(TypeText(node));

        // Remove any existing response buttons
        foreach (Transform child in responseButtonContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void SelectResponse(DialogueResponse response)
    {
        // Execute Event if existing
        if (response.associatedEvents != null && response.associatedEvents.Length > 0)
        {
            for (int i = 0; i < response.associatedEvents.Length; i++)
            {
                ExecuteEvent(response.associatedEvents[i]);
            }
        }

        DialogueNode nextNode = GetDialogueNodeById(response.nextNodeId);

        // Continue to next node if exists
        if (nextNode != null && !nextNode.IsLastNode())
        {
            Debug.Log("nextNode responses --> " + nextNode.responses.Count());
            StartDialogue(currentDialogue, nextNode);
        }
    }

    // Search next node by id. It avoids recursive serialization
    private DialogueNode GetDialogueNodeById(string id)
    {

        if (dialogues[currentDialogue].dialogueNodes == null || dialogues[currentDialogue].dialogueNodes.Count == 0)
        {
            Debug.Log("La lista dialogueNodes de currentDialogue no está inicializada o está vacía.");
            return null;
        }

        foreach (DialogueNode node in dialogues[currentDialogue].dialogueNodes)
        {
            if (node.id == id)
            {
                return node;
            }
        }
        Debug.Log($"No se encontró un nodo con el ID: {id}");
        animator.SetBool("isOpen", false);

        return null;
    }

    private void GenerateResponseButtons(DialogueNode node)
    {
        // Create and setup response buttons based on current dialogue node
        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responseButtonContainer);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            // Setup button to trigger SelectResponse when clicked
            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response));
        }
    }

    //Coroutine writes text letter by letter
    IEnumerator TypeText(DialogueNode node)
    {
        isTyping = true;
        string previousText = dialogueText.text;
        foreach (char character in fullText)
        {
            dialogueText.text += character;
            AudioManager.instance.Play("type_character");
            yield return new WaitForSeconds(typeSpeed);

            if (!isTyping)
            {
                dialogueText.text = previousText + fullText;
                break;
            }
        }
        if (isTyping) isTyping = false;
        GenerateResponseButtons(node);
    }

    private void ExecuteEvent(string associatedEvent)
    {
        if (associatedEvent == "end")
        {
            cutToBlack.SetActive(true);
            AudioManager.instance.Stop("game_music");
            GameManager.instance.LoadLevel(2);
        }
    }
}