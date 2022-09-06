using System;
using TMPro;
using UnityEngine;

[Serializable]
public class Dialogue
{
    public DialogueCharacter character;
    public DialogueFontSize fontSize = DialogueFontSize.Normal;
    public FontStyles fontStyle = FontStyles.Normal;
    public Wobble wobble = Wobble.None;
    [TextArea(3, 10)] public string line;
    public bool topOfScreen;
}

public enum DialogueFontSize
{
    Normal,
    Large,
}

public enum Wobble
{
    None,
    Slight,
    Serious,
}