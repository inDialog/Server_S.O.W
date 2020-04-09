using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    MoveII Move;


    public GameObject textObject;
    GameObject objectText;
    TextMeshPro myTextMesh;
    Multiplayer multiplayer;
     string text;
     bool uppercase, active;
     string myName = "EnterName";

    public Color32 inWrighting;


    // Start is called before the first frame update
    void Start()
    {
        myTextMesh = textObject.GetComponent<TextMeshPro>();
        multiplayer = FindObjectOfType<Multiplayer>();
        Move = GetComponent<MoveII>();
        myTextMesh.text = myName;
        //textObject.get
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) | Input.GetKey(KeyCode.RightShift))
            uppercase = true;
        else
            uppercase = false;

        if (active)
        {
           //if (myName == "EnterName") myTextMesh.text = myName;
            if (Input.anyKeyDown & myTextMesh.text == "EnterName") myTextMesh.text = "";
            myTextMesh.color = inWrighting;

            foreach (char c in Input.inputString)
            {

                if (c == '\b') // has backspace/delete been pressed?
                {
                    if (myTextMesh.text.Length != 0)
                    {
                        myTextMesh.text = myTextMesh.text.Substring(0, myTextMesh.text.Length - 1);
                    }
                }
                else if ((c == '\n') || (c == '\r')) // enter/return
                {
                    //print("User entered their msg: " + myTextMesh.text);
                    if (myName != "EnterName")
                    {
                        myTextMesh.color = multiplayer.myColor;

                        if (objectText == null)
                        {
                            
                            objectText = Instantiate(textObject, transform.position-(transform.up*-1), transform.rotation);
                        }
                        else
                        {
                            objectText.transform.SetPositionAndRotation(transform.position - (transform.up * -1), this.transform.rotation);
                            objectText.GetComponent<TextMeshPro>().text = myTextMesh.text;
                        }
                        multiplayer.w.SendString(multiplayer.FormatMessege(objectText.transform, myTextMesh.text, "TextMessage"));

                    }
                    else
                    {
                       
                        myName = myTextMesh.text;
                        //active = false;
                    }
                    myTextMesh.text = "@" + myName + ":" +"\n";
                }
                else
                {
                    if (uppercase)
                        myTextMesh.text += c.ToString().ToUpper();
                    else
                        myTextMesh.text += c.ToString();

                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
            active = !active;

        myTextMesh.enabled = active;
        Move.enabled = !active;
    }

    
}

