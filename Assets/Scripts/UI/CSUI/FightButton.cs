using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightButton : MonoBehaviour
{
    public AudioManager audioManager;

    public void OnMouseDown()
    {
        if (!ConfirmSelect.Instance.confirmedP1 || !ConfirmSelect.Instance.confirmedP2)
        {
            return;
        }

        audioManager.PlaySFX(audioManager.beep);
        SceneManager.LoadScene(2);
    }
}