using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(MapPrefabs), menuName = "ScriptableObjects/MapPrefabs")]
public class MapPrefabs : ScriptableObject
{
    public GameObject[] RoomsPrefabs;
}
