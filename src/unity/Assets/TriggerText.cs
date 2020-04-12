using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TriggerText : MonoBehaviour
{
    public Sprite artistImage;
    public string artistDescription;
    public string shortDescription;
    public string urlArtis;

    static GameObject textUI;
    static Text sDescription, lDescription;
    Image image;
    Button button;

    private void Start()
    {
        textUI =  GameObject.FindGameObjectWithTag("ArtistUI");
        sDescription = textUI.GetComponentsInChildren<Text>()[0];
        lDescription = textUI.GetComponentsInChildren<Text>()[1];
        image = textUI.GetComponentsInChildren<Image>()[1];
        button = textUI.GetComponentsInChildren<Button>()[0];

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            sDescription.text = shortDescription;
            lDescription.text = artistDescription;
            image.sprite = artistImage;
            button.onClick.AddListener(CustomButton_onClick);
            textUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            textUI.SetActive(false);
            button.onClick.RemoveAllListeners();

        }
    }
    void CustomButton_onClick()
    {
        if (urlArtis.Contains("www"))
        {
            Application.OpenURL("http://" + urlArtis + "/");
            //Application.
        }
            //     Application.ExternalEval("window.open(\"http://www.unity3d.com\")");
            //    Application.OpenURL("http://"+ urlArtis+ "/");
            //Application.OpenURL("http://" + urlArtis + "/");
            //Application.ExternalEval("window.open(\"http://" + urlArtis +"\"+")");

    }


}

