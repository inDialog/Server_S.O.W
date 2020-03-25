using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class TowerState : MonoBehaviour
{
    Navigator navigator;
    SocketIOComponent socket;
    public Tower thisTower;
    public Vector3 colisionPoint;
    bool firstClik;

    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Awake()
    {
        thisTower = new Tower();
        socket = FindObjectOfType<SocketIOComponent>();
        navigator = GetComponent<Navigator>();
        thisTower.stateOf =  "Neutral";
        thisTower.Master = null;


    }
    private void Update()
    {
        if (thisTower.Master != null)
        {
            UpdatePosition(thisTower.Master.transform.position);
            GetComponent<BoxCollider>().isTrigger = true;
            return;
        }
        else
        {
            if (colisionPoint != Vector3.zero & thisTower.stateOf != "Fallow")
            {
                UpdatePosition(colisionPoint);
            }
            else
            {
                GetComponent<BoxCollider>().isTrigger = false;
            }
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerState playerState = collision.gameObject.GetComponent<PlayerState>();
            if (thisTower.PlayerKey == playerState._playerKey)
            {
                if (thisTower.stateOf != "Locked")
                {
                    thisTower.stateOf = playerState.playerState;
                }
                if (playerState.playerState == "Locked")
                {
                    if (firstClik == true)
                    {
                        thisTower.stateOf = "Locked";
                    }
                    else
                    {
                        thisTower.stateOf = "Neutral";
                    }
                    firstClik = !firstClik;
                }

                if (playerState.playerState == "Fallow" & thisTower.stateOf != "Locked")
                {
                    Debug.Log("dsd");
                    thisTower.Master = collision.gameObject;
                    if (!playerState.TowersFowlowing.Contains(this.gameObject))
                        playerState.TowersFowlowing.Add(this.gameObject);
                }
                Debug.Log(thisTower.stateOf);
            }
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tower")
        {
            colisionPoint = other.ClosestPointOnBounds(this.transform.position);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            thisTower.Master = null;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.tag == "Tower")
        //{
        //    colisionPoint = Vector3.zero;
        //}
    }
    void UpdatePosition(Vector3 position)
    {
        navigator.target = position;
        //Debug.Log(EMIT);
        if (Vector3.Distance(colisionPoint,this.transform.position)<0.2f)
        {
            colisionPoint = Vector3.zero;
        }
        socket.Emit("move", new JSONObject(Network.VectorToJsonWithId(position, thisTower.TowerKey)));
    }


}

