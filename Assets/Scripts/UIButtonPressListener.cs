using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

        
/// <summary>
/// Listens for simple trigger enters, and invokes the button.onClick event
/// </summary>
public class UIButtonPressListener : MonoBehaviour {
    private Button button;
    public string tag;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void OnTriggerEnter(Collider other) {
        // Make sure it's the player interacting with the trigger
        if (other.CompareTag("Player")) {
            button.onClick.Invoke();
        }
    }
}
