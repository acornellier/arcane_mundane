﻿using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void Interact();
    public abstract void Highlight();
    public abstract void Unhighlight();
}