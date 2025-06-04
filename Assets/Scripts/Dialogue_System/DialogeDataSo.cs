using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/DialogueData")]
public class DialogueDataSO : ScriptableObject
{ 
    [Header("ĳ���� ����")]
    public string chatacterName = "ĳ����";
    public Sprite characterImage;

    [Header("��ȭ ����")]
    [TextArea(3, 10)]
    public List<string> dialogueLines = new List<string>();
}
