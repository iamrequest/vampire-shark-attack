using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Settings : MonoBehaviour {
    // Singleton pattern
    private static Settings _instance;
    public static Settings instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<Settings>();
            }

            return _instance;
        }
    }

    public UnityEvent onSettingsSaved;
    private void Awake() {
        _instance = this;
    }

    public void SaveSettings() {
        Debug.Log("Saving settings...");
        onSettingsSaved.Invoke();
    }
}
