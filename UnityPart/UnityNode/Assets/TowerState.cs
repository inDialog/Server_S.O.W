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
    public int nrInLine;
    public int myId;

    // Start is called before the first frame update
    void Awake()
    {
        thisTower = new Tower();
        socket = FindObjectOfType<SocketIOComponent>();
        navigator = GetComponent<Navigator>();
        thisTower.TowerState =  "Neutral";
        thisTower.Master = null;


    }
    private void Update()
    {
        if (thisTower.Master != null)
        {
            UpdatePosition(thisTower.Master.transform.position - thisTower.Master.transform.forward  * (nrInLine+1));
            transform.rotation = thisTower.Master.transform.rotation;
            GetComponent<MeshCollider>().isTrigger = true;
            colisionPoint = Vector3.zero;
            thisTower.TowerState = "Fallow";

            return;
        }
        else
        {
            if (colisionPoint != Vector3.zero & thisTower.TowerState != "Locked")
            {
                UpdatePosition(colisionPoint);
            }
            else
            {
                GetComponent<MeshCollider>().isTrigger = false;

            }
        }
        //Debug.Log(thisTower.TowerKey);


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerState playerState = collision.gameObject.GetComponent<PlayerState>();
            if (thisTower.PlayerKey == playerState._playerKey)
            {
                if (thisTower.TowerState != "Locked")
                {
                    thisTower.TowerState = playerState.playerState;
                }
                if (playerState.playerState == "Locked")
                {
                    if (firstClik == true)
                    {
                        thisTower.TowerState = "Locked";
                    }
                    else
                    {
                        thisTower.TowerState = "Neutral";
                    }
                    firstClik = !firstClik;
                }

                if (playerState.playerState == "Fallow" & thisTower.TowerState != "Locked")
                {
                    thisTower.Master = collision.gameObject;
                    if (!playerState.TowersFowlowing.Contains(this.gameObject))
                    {
                        playerState.TowersFowlowing.Add(this.gameObject);
                        nrInLine = playerState.TowersFowlowing.Count;
                    }

                }
                Debug.Log(thisTower.TowerState);
            }
        }
       
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Tower")
        {
            colisionPoint = other.ClosestPointOnBounds(this.transform.position);
            //Debug.Log("Contact");
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
    }
    
    void UpdatePosition(Vector3 position)
    {
        navigator.target = position;
        //Debug.Log(EMIT);
        if (Vector3.Distance(colisionPoint,this.transform.position)<0.1f)
        {
            colisionPoint = Vector3.zero;

        }
        socket.Emit("move", new JSONObject(Network.VectorToJsonWithId(position, thisTower.TowerKey)));

    }

    private void SendPosition()
    {

    }


}

