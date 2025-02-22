using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    [Header("Node Id")]
    public string id; // Unique identifier for the dialogue node. It avoids recursive serialization
    [Header("DialogueText")]
    [TextArea(3, 6)]
    public string dialogueText;
    [Header("Responses")]
    public List<DialogueResponse> responses;

    internal bool IsLastNode()
    {
        return responses.Count <= 0;
    }
}
