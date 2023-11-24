using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Base : MonoBehaviour, IPointerClickHandler
{
    public event UnityAction ResourceIsGot;

    [SerializeField] private Generator _generator;
    [SerializeField] private Unit _unitSample;
    [SerializeField] private Plane _plane;
    [SerializeField] private List<Unit> _units;

    private int _resourceCountForCreateUnit = 3;
    private bool _canBuildNewBuilding = false;
    private bool _isUnitAtFlag = false;

    public Base NewBase = null;

    public Resource CurrentResourceToCollect { get; private set; }

    public int ResourceCountForCreateBuilding { get; private set; }

    public int ResourseCount { get; private set; }

    public bool FlagIsCreated { get; private set; }

    public bool IsItChose { get; private set; }

    public bool HasBuilder { get; private set; }

    public bool IsBuildNewBase { get; private set; }

    private void Start()
    {
        CurrentResourceToCollect = null;
        ResourseCount = 0;
        ResourceCountForCreateBuilding = 5;
        StartCoroutine(Create());
    }

    private void Awake()
    {
        _plane.FlagIsCreated += SetStatusFlagCreated;
    }

    private void Update()
    {
        if (IsItChose && FlagIsCreated)
        {
            _canBuildNewBuilding = true;
        }

        if (_plane.CurrentFlag != null)
        {
            _plane.CurrentFlag.UnitOnPointAndCanBuild += SetStatusIsUnitAtFlag;
        }

        foreach (Unit unit in _units)
        {
            if (unit.IsFree)
            {
                GetResource();
                ResourceIsGot?.Invoke();
            }
        }
    }

    private void GetResource()
    {
        if (CurrentResourceToCollect == null && _generator.GiveFirstListedResource() != null)
        {
            CurrentResourceToCollect = _generator.GiveFirstListedResource();
        }
    }

    public Transform GetFlagOfBuildNewBaseTransform()
    {
        if (_plane.CurrentFlag != null)
        {
            return _plane.CurrentFlag.transform;
        }

        else
        {
            return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Resource>(out Resource resource) && resource.isDelivered)
        {
            ResourseCount++;
            Debug.Log("Общее количество ресурсов: " + ResourseCount);
        }
    }

    private IEnumerator Create()
    {
        while (true)
        {
            if (_canBuildNewBuilding && ResourseCount >= ResourceCountForCreateBuilding && _isUnitAtFlag == true)
            {
                NewBase = Instantiate(this, _plane.CurrentFlag.transform.position, UnityEngine.Quaternion.identity);
                FlagIsCreated = false;
                IsItChose = false;
                _plane.CurrentFlag.Destroy();
                _canBuildNewBuilding = false;
                ResourseCount -= ResourceCountForCreateBuilding;
                IsBuildNewBase = true;
            }

            if (ResourseCount >= _resourceCountForCreateUnit && _canBuildNewBuilding == false)
            {
                Unit unit = Instantiate(_unitSample, transform.position, UnityEngine.Quaternion.identity);
                unit.SetBase(this);
                ResourseCount -= _resourceCountForCreateUnit;
            }

            yield return null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsItChose = true;
    }

    private void SetStatusIsUnitAtFlag()
    {
        _isUnitAtFlag = true;
    }

    private void SetStatusFlagCreated()
    {
        if (IsItChose)
        {
            FlagIsCreated = true;
        }
    }

    public void SetStatusHasBuilderIsTrue()
    {
        HasBuilder = true;
    }

    public Base GetNewBase()
    {
        return NewBase;
    }

    public void SetStatusIsBuildNewBaseFalse()
    {
        IsBuildNewBase = false;
    }

    public void SetCurrentResourceToCollectNull()
    {
        CurrentResourceToCollect = null;
    }
}
