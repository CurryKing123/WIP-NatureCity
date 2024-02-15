using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    private NavMeshAgent agent;

    private bool isMoving = false;
    private bool isMovingToResource = false;
    private Transform targetResource;
    public void CallRace(string race)
    {
        StartCoroutine(GetRace(race));
    }

    private void Start()
    {
        string dH = (File.ReadAllText(Application.persistentDataPath + "CharData.json"));
        CharArray myChar = new CharArray();
        myChar = JsonUtility.FromJson<CharArray>(dH);
        string charRace = myChar.data[0].character_race;
        CallRace(charRace);


        agent = GetComponent<NavMeshAgent>();
    }

    IEnumerator GetRace(string race)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/race/get-race-by-name?{race}"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.Log(www.error);
            }
            else
            {
                Races charRace = new Races();
                string dH = www.downloadHandler.text;
                charRace = JsonUtility.FromJson<Races>(dH);
                agent.speed = charRace.data[0].move_speed;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            

            if (Physics.Raycast(ray, out hit))
            {
                

                if (hit.collider.CompareTag("Resource"))
                {
                    ResourceManager resource = hit.collider.GetComponent<ResourceManager>();
                    targetResource = hit.transform;
                    
                    
                    if (resource != null)
                    {
                        MoveToResource(targetResource);
                        isMovingToResource = true;
                        Debug.Log("Moving to resource");
                    }
                }
                else
                {
                    // Move the player to the clicked position
                    MovePlayer(hit.point);
                    isMovingToResource = false;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In Resource Area");
        
        if (other.CompareTag("Resource") && (isMovingToResource))
        {
            agent.speed = 0f;
            
        }
    }
    



    
    private void MoveToResource(Transform resource)
    {
        agent.SetDestination(resource.position);
    }
    void MovePlayer(Vector3 destination)
    {
        agent.SetDestination(destination);
        isMoving = true;
    }


}