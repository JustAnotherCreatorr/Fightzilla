using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightButton : MonoBehaviour
{
    public void OnMouseDown()
    {
        if (!ConfirmSelect.Instance.confirmedP1 || !ConfirmSelect.Instance.confirmedP2)
        {
            return;
        }

        SceneManager.LoadScene(2);
    }
}