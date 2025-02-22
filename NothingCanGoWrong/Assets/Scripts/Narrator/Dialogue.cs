using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/DialogueAsset")]
public class Dialogue : ScriptableObject
{
    public DialogueNode rootNode; // First node of the conversation
    public List<DialogueNode> dialogueNodes; // List of all dialogue nodes
}
