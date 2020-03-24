using UnityEngine;
using SocketIO;
using System.Collections.Generic;
using System;
using System.Collections;
/// <summary>
/// 1. ID -  the key of the player from the server 
/// 2. KEY -  the local key of the user
/// 3. myColor - random color atributed to user and  to the towers
/// </summary>
public class Network : MonoBehaviour
{
    Dictionary<string, GameObject> players;

    static SocketIOComponent socket;

    public GameObject playerPrefab;
    public GameObject myPlayer;
    public GameObject MainPlayer;

    int lastIndex;
    Color32 myColor;

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();
        socket.On("open", OnConnected);
        socket.On("SendId", GetKey);
        socket.On("spawn", OnSpawned);
        socket.On("move", OnMove);
        socket.On("disconnected", OnDisconnected);
        socket.On("requestPosition", OnRequestPosition);
        socket.On("updatePosition", OnUpdatePosition);

        myColor = new Color32((byte)PlayerPrefs.GetFloat("R"), (byte)PlayerPrefs.GetFloat("G"), (byte)PlayerPrefs.GetFloat("B"), (byte)225);
        players = new Dictionary<string, GameObject>();
        myPlayer.GetComponent<Renderer>().material.SetColor("_EmissionColor", myColor);
        MainPlayer.GetComponent<Renderer>().material.SetColor("_EmissionColor", myColor);
    }

    void OnConnected(SocketIOEvent e)
    {
        Debug.Log("Connected");
    }

    void GetKey(SocketIOEvent e)
    {
        string id = e.data["id"].ToString().Replace("\"", "");
        FindObjectOfType<SendState>().thisTower.towerKey = id;

        if (PlayerPrefs.GetString("key").Length < 6)
        {
            PlayerPrefs.SetString("key", id);
            FindObjectOfType<PlayerState>()._playerKey = id;
            myPlayer.GetComponent<SendState>().thisTower.playerKey = id;
        }
        else
        {
            id = PlayerPrefs.GetString("key");
            FindObjectOfType<PlayerState>()._playerKey = id;
            myPlayer.GetComponent<SendState>().thisTower.playerKey = id;
            print(id);
        }
        socket.Emit("SetKey", new JSONObject(string.Format("[\"{0}\"]", id)));

        int con = PlayerPrefs.GetInt("NrOfConnections");
        PlayerPrefs.SetInt("NrOfConnections", con + 1);
        socket.Emit("setNr", new JSONObject(string.Format("[\"{0}\"]", con)));
        socket.Emit("setColor", new JSONObject(ColorToJson(myColor)));
    }


    void OnSpawned(SocketIOEvent e)
    {
        Debug.Log("Player spawned" + e.data);
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        player.GetComponent<SendState>().thisTower.towerKey = e.data["id"].ToString().Replace("\"", "");

        if (e.data["x"])
        {
            Vector3 movePosition = new Vector3(GetFloatFromJson(e.data, "x"), GetFloatFromJson(e.data, "y"), GetFloatFromJson(e.data, "z"));
            Navigator navigatePos = player.GetComponent<Navigator>();
            navigatePos.Spawn(movePosition);
        }
        players.Add(e.data["id"].ToString(), player);

        string tempKey = e.data["key"].ToString().Replace("\"", "").Replace("[", "").Replace("]", "");
        if (player.GetComponent<SendState>())
        {
            player.GetComponent<SendState>().thisTower.playerKey = tempKey;
            //Debug.Log("playerKey   " + tempKey);
        }

        if (PlayerPrefs.GetString("key") == tempKey)
        {
            int curentIndex;
            string temp = e.data["nr"].ToString().Replace("\"", "").Replace("[", "").Replace("]", "");
            int.TryParse(temp, out curentIndex);
            Debug.Log("NrOfConnections   " + PlayerPrefs.GetInt("NrOfConnections"));
            ///////////////////////////////////////////////////////////////////////////////////////// Find last object created by this local key
            if (curentIndex > lastIndex)
                FindObjectOfType<PlayerState>().respawnPos = player.transform;
            lastIndex = curentIndex;
            Debug.Log("curentIndex   " + curentIndex);
        }
        ///////////////////////////////////////////////////////////////////////////////////////// applay unique color to tower 
        player.GetComponent<Renderer>().material.SetColor("_EmissionColor", ColorFromJson(e));
        Debug.Log("Player Count: " + players.Count);
    }
    private void OnMove(SocketIOEvent e)
    {
        Debug.Log("Player is moving " + e.data);

        Vector3 position = new Vector3(GetFloatFromJson(e.data, "x"), GetFloatFromJson(e.data, "y"), GetFloatFromJson(e.data, "z"));

        var player = players[e.data["id"].ToString()];

        Navigator navigatePos = player.GetComponent<Navigator>();

        navigatePos.NavigateTo(position);
    }

    private void OnRequestPosition(SocketIOEvent e)
    {
        Debug.Log("Server is requesting position");

        socket.Emit("updatePosition", new JSONObject(VectorToJson(myPlayer.transform.position)));
    }

    private void OnUpdatePosition(SocketIOEvent e)
    {
        Debug.Log("Updating position: " + e.data);

        Vector3 position = new Vector3(GetFloatFromJson(e.data, "x"), GetFloatFromJson(e.data, "x"), GetFloatFromJson(e.data, "y"));

        var player = players[e.data["id"].ToString()];

        player.transform.position = position;
    }

    private void OnDisconnected(SocketIOEvent e)
    {
        Debug.Log("Client disconnected: " + e.data);

        string id = e.data["id"].ToString();

        var player = players[id];
        Destroy(player);
        players.Remove(id);
    }

    float GetFloatFromJson(JSONObject data, string key)
    {
        return float.Parse(data[key].ToString().Replace("\"", ""));
    }

    public Color32 ColorFromJson(SocketIOEvent e)
    {
        return new Color32((byte)GetFloatFromJson(e.data, "r"), (byte)GetFloatFromJson(e.data, "g"), (byte)GetFloatFromJson(e.data, "b"), 1);
    }

    public static string VectorToJson(Vector3 vector)
    {
        return string.Format(@"{{""x"":""{0}"", ""y"":""{1}"",""z"":""{2}""}}", vector.x, vector.y, vector.z);
    }
    public static string VectorToJsonWithId(Vector3 vector,string id)
    {
        return string.Format(@"{{""x"":""{0}"", ""y"":""{1}"",""z"":""{2}"",""id"":""{3}""}}", vector.x, vector.y, vector.z,id);
    }

    public static string ColorToJson(Color32 color32)
    {
        return string.Format(@"{{""r"":""{0}"", ""g"":""{1}"",""b"":""{2}""}}", color32.r, color32.g, color32.b);
    }

}

