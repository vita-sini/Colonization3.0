using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private float _moveSpeed = 15f;
    private float _pickupRange = 0.5f;
    private float _carryDistance = 0.5f;
    private float _buildDistance = 1f;

    private BaseSpawner _baseSpawner;
    private Base _base;
    private Resource _carriedResource;
    private Flag _targetFlag;

    public bool IsBusy { get;  set; } = false;

    public void SetBaseSpawner(BaseSpawner baseSpawner)
    {
        _baseSpawner = baseSpawner;
    }

    public void SetDestination(Component targetComponent)
    {
        if (targetComponent == null)
            return;

        IsBusy = true;

        if (targetComponent is Resource resource)
        {
            _carriedResource = resource;
            StartCoroutine(MoveToTarget(resource.transform, _pickupRange, PickupResource));
        }
        else if (targetComponent is Flag flag)
        {
            _targetFlag = flag;
            StartCoroutine(MoveToTarget(flag.transform, _buildDistance, CreateNewBase));
        }
    }

    public void SetBaseBot(Base baseBot)
    {
        _base = baseBot;
    }

    private void CreateNewBase()
    {
        Vector3 newBasePosition = new Vector3(_targetFlag.transform.position.x, 1.01f, _targetFlag.transform.position.z);
        Base newBase = _baseSpawner.Spawn(newBasePosition, _targetFlag);
        _base.RemoveFlag(this);

        _base = newBase;
        newBase.AddBot(this);

        _targetFlag = null;
    }

    private void PickupResource()
    {
        if (_carriedResource == null)
            return;

        _carriedResource.transform.SetParent(transform);
        _carriedResource.transform.localPosition = Vector3.forward * _carryDistance;
        _carriedResource.transform.localRotation = Quaternion.identity;

        StartCoroutine(MoveToTarget(_base.transform, _carryDistance, DropResource));
    }

    private void DropResource()
    {
        if (_carriedResource == null || _base == null)
            return;

        _carriedResource.transform.SetParent(null);
        _base.TakeResource(_carriedResource);
        _carriedResource.Release();
        _carriedResource = null;

        IsBusy = false;
    }

    private IEnumerator MoveToTarget(Transform target, float stopDistance, Action onComplete)
    {
        while ((target.position - transform.position).sqrMagnitude > stopDistance * stopDistance)
        {
            transform.position += (target.position - transform.position).normalized * _moveSpeed * Time.deltaTime;
            yield return null;
        }

        onComplete();
    }
}