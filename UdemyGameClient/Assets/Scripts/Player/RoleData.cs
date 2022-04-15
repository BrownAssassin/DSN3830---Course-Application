using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RoleData {

    public GameObject rolePrefab { get; private set; }
    public GameObject arrowPrefab { get; private set; }
    public RoleType roleType { get; private set; }
    public Vector3 spawnPosition { get; private set; }
    private const string PATHPREFAB = "Player/";

    public RoleData(RoleType rt, string rolePath, string arrowPath, Vector3 spawnPosition)
    {
        this.roleType = rt;
        rolePrefab = Resources.Load(PATHPREFAB+rolePath) as GameObject;
        arrowPrefab = Resources.Load(PATHPREFAB+arrowPath) as GameObject;
        this.spawnPosition = spawnPosition;
    }
}
