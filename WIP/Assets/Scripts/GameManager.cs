using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI wood;
    [SerializeField] private TextMeshProUGUI stone;
    void Update()
    {
        CallGlobalInv();
    }
    public void CallGlobalInv()
    {
        StartCoroutine(GetGlobalInv());
    }

    IEnumerator GetGlobalInv()
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://localhost:8002/global_inventory/get-global_inv"))
        {
            www.SetRequestHeader("key", "1");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string dH = www.downloadHandler.text;
                GlobalInventory globalInv = new GlobalInventory();
                globalInv = JsonUtility.FromJson<GlobalInventory>(dH);
                wood.text = $"Wood: {globalInv.data[0].res_amount}";
                stone.text = $"Stone: {globalInv.data[1].res_amount}";
            }
        }
    }
}

