using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleManagement : MonoBehaviour
{
    [SerializeField] private List<GameObject> Castles;
    [SerializeField] private pausemenu Pausemenu;

    private void Update()
    {
        if(Castles.Count > 0 && Castles[0] == null){
            PromoteNextCastle();
        }
    }

    private void PromoteNextCastle()
    {
        Castles.RemoveAll(item => item == null);

        if(Castles.Count > 0){
            Castles[0].tag = "CastleTarget";
            Debug.Log("New Target");
        }
        else{
            Pausemenu.GameOver();
        }
    }
}
