using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueResponse
{
    [Header("Response Text")]
    [TextArea(1, 3)]
    public string responseText;
    [Header("Next Node")]
    public string nextNodeId; // Use an ID to reference the next node. It avoids recursive serialization
}
