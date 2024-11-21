using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{

    public GameObject SFX;
    public GameObject Music;
    public GameObject ST;
    public GameObject textures;
    public GameObject models;
    public GameObject anims;

    public Text buttontext;
    public Text buttontext1;
    public Text buttontext2;

    public bool switched = false;
    public bool switched1 = false;
    public bool switched2 = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnMouseDown()
    {
        if (gameObject.tag == "models")
        {
            if (switched)
            {
                models.SetActive(false);
                Music.SetActive(true);
                buttontext.text = "3D Models";
                switched = !switched;
                return;
            }

            models.SetActive(true);
            Music.SetActive(false);
            buttontext.text = "Music";
            switched = !switched;
        }

        if (gameObject.tag == "Animations")
        {
            if (switched1)
            {
                ST.SetActive(true);
                anims.SetActive(false);
                buttontext1.text = "Animations";
                buttontext1.fontSize = 73;
                switched1 = !switched1;
                return;
            }

            ST.SetActive(false);
            anims.SetActive(true);
            buttontext1.text = "Special Thanks";
            buttontext1.fontSize = 61;
            switched1 = !switched1;
        }

        if (gameObject.tag == "textures")
        {
            if (switched2)
            {
                SFX.SetActive(true);
                textures.SetActive(false);
                buttontext2.text = "Textures";
                switched2 = !switched2;
                return;
            }

            SFX.SetActive(false);
            textures.SetActive(true);
            buttontext2.text = "SFX";
            switched2 = !switched2;
        }
    }
}
