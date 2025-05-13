using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interacte();
    string ShowHint();
    void OnCursorIn();
    void OnCursorOut();
}
