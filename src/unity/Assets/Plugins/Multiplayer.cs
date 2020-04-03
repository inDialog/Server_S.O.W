using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

// define classed needed to deserialize recieved data
[Serializable]
public class InfoPlayers {
    public Vector3 position;
    public Color32 color;
    public int timestamp;
    public string id;


}
[Serializable]
public class Players {
   public List<InfoPlayers> players;
}

public class Multiplayer : MonoBehaviour
{

    // define public game object used to visualize other players
    public GameObject otherPlayerObject;
    public GameObject myPlayer;
    public string url;
    //"ws://localhost:8000"
    private Vector3 prevPosition;
    //private List<GameObject> otherPlayers = new List<GameObject>();
    //private List<string> playersID = new List<GameObject>();
    Dictionary<string, GameObject> otherPlayers = new Dictionary<string, GameObject>();
    public Color32 myColor;
    //WebSocket w = new WebSocket(new Uri("ws://www.in-dialog.com:3000/socket.io/?EIO=4&transport=websocket"));
    WebSocket w;
    //System.Guid myGUID;
    System.Guid myGUID;
    
    private void Awake()
    {
        w = new WebSocket(new Uri(url));
        //myGUID = UnityEngine.Random.RandomRange(0, 1000).ToString() +"R2:"+ UnityEngine.Random.RandomRange(100, 1000).ToString();
        myGUID = System.Guid.NewGuid();
        //print(myGUID.ToString());
        myColor = Color();
        myPlayer.GetComponent<Renderer>().material.color = myColor;
    }

    IEnumerator Start()
    {
        // get player

        // connect to server
        yield return StartCoroutine(w.Connect());
        Debug.Log("CONNECTED TO WEBSOCKETS");
        // generate random ID to have idea for each client (feels unsecure)
        // wait for messages

        while (true)
        {
            // read message
            string message = w.RecvString();
            // check if message is not empty
            if (message != null)
            {
                if (message.ToString() == "Conceted")
                {
                    w.SendString(myGUID + "\t" + StringToCollor(myColor) + "\t" + "color");
                    continue;
                }
                else if (message.ToString() == "Deleted")
                {
                    continue;
                }


                // deserialize recieved data
                Players data = JsonUtility.FromJson<Players>(message);
                //Debug.Log("PlayerCount: " + data.players.Count);
                //Debug.Log("otherPlayers: " + otherPlayers.Count);
                SpawnPlayers(data);
                UpdatePositions(data);
            }


            // if connection error, break the loop
            if (w.error != null)
            {
                Debug.Log("Error: " + w.error);
                break;
            }


            SendPositions();
            yield return 0;
        }

        // if error, close connection
        w.Close();
    }
    private void OnDisable()
    {
        w.SendString(myGUID + "\t" + "Disconected");
    }
    void UpdatePositions(Players data)
    {

        for (int i = 0; i < data.players.Count; i++)
        {
            //print(data.players[i].color.ToString());

            otherPlayers[data.players[i].id.ToString()].transform.position = Vector3.Lerp(otherPlayers[data.players[i].id.ToString()].transform.position, data.players[i].position, Time.deltaTime * 10F);
            // otherPlayers[i].transform.position = data.players[i].position;
        }
    }
    void SpawnPlayers(Players data)
    {
        // if number of players is not enough, create new ones
        if(data.players.Count!= otherPlayers.Count)
        for (int i = 0; i < data.players.Count; i++)
        {
            Debug.Log(i + "data color " + data.players[i].color);
            if (!otherPlayers.ContainsKey(data.players[i].id.ToString()))
            {
                GameObject instance = Instantiate(otherPlayerObject, data.players[i].position, Quaternion.identity);
                instance.GetComponent<Renderer>().material.color = data.players[i].color;
                otherPlayers.Add(data.players[i].id.ToString(), instance);
            }
        }

    }
    private void SendPositions()
    {
        // check if player moved
        float distance = Vector3.Distance(prevPosition, myPlayer.transform.position);
        if (distance > 0.01f)
        {
            // send update if position had changed
            string toSend = myGUID + "\t" + myPlayer.transform.position.x + "\t" + myPlayer.transform.position.y + "\t" + myPlayer.transform.position.z;
            //toSend = string.Format("[\"{0}\",{1}]", "mxessage", toSend);
            w.SendString(toSend);
            prevPosition = myPlayer.transform.position;
        }
    }

    string StringToCollor(Color32 Color)
    {
     return
     Color.r + "\t" +
     Color.g + "\t" +
     Color.b;
    }
    Color32 Color()
    {
        return new Color32(
        (byte)UnityEngine.Random.Range(0, 255),
        (byte)UnityEngine.Random.Range(0, 255),
        (byte)UnityEngine.Random.Range(0, 255),
        225);
    }

}
