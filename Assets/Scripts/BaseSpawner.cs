using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private ResourceData _resourceData;

    public Base Spawn(Vector3 position, Flag flag)
    {
        Base newBase = Instantiate(_basePrefab, position, Quaternion.identity);
        newBase.SetFlag(flag);
        newBase.SetUnitCreated();
        newBase.SetFlagPlaced();
        newBase.SetResourceData(_resourceData);
        return newBase;
    }
}
