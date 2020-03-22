using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class SendState : MonoBehaviour
{
    //public SocketIOComponent socket;

    SocketIOComponent socket;
    GameObject player;
    Navigator navigator;
    bool active;
    // Start is called before the first frame update
    void Start()
    {
        socket = FindObjectOfType<SocketIOComponent>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        navigator = GetComponent<Navigator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            if (active)
            {
                GetComponent<BoxCollider>().enabled = false;
                navigator.target = player.transform.position;
                socket.Emit("move", new JSONObject(Network.VectorToJson(player.transform.position)));

            }
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            active = false;
            GetComponent<BoxCollider>().enabled = true;

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
            active = true;
    }

    //private void OnTriggerEnter(Collider collision)
    //{
    //    if (collision.gameObject == player)
    //        active = true;

    //}
}
