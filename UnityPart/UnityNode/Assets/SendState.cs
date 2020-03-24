using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class SendState : MonoBehaviour
{
    //public SocketIOComponent socket;
    Navigator navigator;
    SocketIOComponent socket;
    public  GameObject master;
    public Tower thisTower;
    // Start is called before the first frame update
    void Awake()
    {
        thisTower = new Tower();
        socket = FindObjectOfType<SocketIOComponent>();
        navigator = GetComponent<Navigator>();
        //socket.Emit("RequestState");

        master = null;
    }

    // Update is called once per frame
    void Update()
    {
//        Debug.Log(
//thisTower.towerKey);
        if (master != null)
            thisTower.stateOf = master.GetComponent<PlayerState>().playerState;

        switch (thisTower.stateOf)
        {
            case "Locked":
                GetComponent<BoxCollider>().enabled = true;
                master = null;

                break;

            case "Neutral":
                GetComponent<BoxCollider>().enabled = true;
                //master = null;

                break;

            case "Fallow":
                GetComponent<BoxCollider>().enabled = false;

                navigator.target = master.transform.position;
                JSONObject EMIT = new JSONObject(Network.VectorToJsonWithId(master.transform.position, thisTower.towerKey));
                socket.Emit("move", EMIT);
                print(EMIT);
                break;

            default:
                break;
        }
   

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerState _player = collision.gameObject.GetComponent<PlayerState>();
            if (thisTower.playerKey == _player._playerKey)
            {
                master = collision.gameObject;
                //_player.playerState = "Neutral";
                Debug.Log(thisTower.stateOf);
            }
        }
    }
}

