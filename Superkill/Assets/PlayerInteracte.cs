using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteracte : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Text _textHint;

    private IInteractable _lastInteractableObject;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out hit, 5f))
        {
            IInteractable interactableObject;
            if (hit.collider.TryGetComponent<IInteractable>(out interactableObject))
            {
                _lastInteractableObject = interactableObject;
                interactableObject.OnCursorIn();
                _textHint.text = interactableObject.ShowHint();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactableObject.Interacte();
                }
            }
            else
            {
                _lastInteractableObject?.OnCursorOut();
                _textHint.text = "";
            }
        }

        else
        {
            _lastInteractableObject?.OnCursorOut();
            _textHint.text = "";
        }
    }
}