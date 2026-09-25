using JetBrains.Annotations;
using UnityEngine;

public class Credits : MonoBehaviour
{

    public Camera StartMenuCamera;
    public Camera CreditsCamera;

    void Start()
    {
        StartMenuCamera.enabled = true;
        StartMenuCamera.GetComponent<AudioListener>().enabled = true;
        CreditsCamera.enabled = false;
        CreditsCamera.GetComponent<AudioListener>().enabled = false;
    }

    public void OpenCredits()
    {
        StartMenuCamera.enabled = false;
        StartMenuCamera.GetComponent<AudioListener>().enabled = false;
        CreditsCamera.enabled = true;
        CreditsCamera.GetComponent<AudioListener>().enabled = true;
    }

    public void CloseCredits()
    {
        CreditsCamera.enabled = false;
        CreditsCamera.GetComponent<AudioListener>().enabled = false;
        StartMenuCamera.enabled = true;
        StartMenuCamera.GetComponent<AudioListener>().enabled = true;
        
    }
}
