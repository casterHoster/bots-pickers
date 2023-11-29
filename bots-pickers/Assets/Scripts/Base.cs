using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Base : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Generator _generator;
    [SerializeField] private Unit _unit;
    [SerializeField] private Plane _plane;

    public Base NewBase = null;
    private int _resourceCountForCreateUnit = 3;
    private bool _canBuildNewBuilding = false;
    private bool _isUnitAtFlag = false;

    public int ResourceCountForCreateBuilding { get; private set; }

    public int ResourseCount { get; private set; }

    public bool FlagIsCreated { get; private set; }

    public bool IsChose { get; private set; }

    public bool HasBuilder { get; private set; }

    public bool IsBuildNewBase { get; private set; }

    public Resource CurrentResource { get; private set; }

    private void Start()
    {
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
        if (IsChose && FlagIsCreated)
        {
            _canBuildNewBuilding = true;
        }

        if (_plane.CurrentFlag != null)
        {
            _plane.CurrentFlag.UnitOnPointAndCanBuild += SetStatusIsUnitAtFlag;
        }

        GetResource();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Resource>(out Resource resource) && resource.isDelivered)
        {
            ResourseCount++;
        }
    }

    public void GetResource()
    {
        if (CurrentResource == null)
        {
            CurrentResource = _generator.GiveFirstListedResourceAndDeleteItFromList();
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

    public void OnPointerClick(PointerEventData eventData)
    {
        IsChose = true;
    }

    public void SetStatusBuilderIsTrue()
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

    public void SetCurrentResourceNull()
    {
        CurrentResource = null;
    }

    private IEnumerator Create()
    {
        while (true)
        {
            if (_canBuildNewBuilding && ResourseCount >= ResourceCountForCreateBuilding && _isUnitAtFlag == true)
            {
                NewBase = Instantiate(this, _plane.CurrentFlag.transform.position, UnityEngine.Quaternion.identity);
                FlagIsCreated = false;
                IsChose = false;
                _plane.CurrentFlag.Destroy();
                _canBuildNewBuilding = false;
                ResourseCount -= ResourceCountForCreateBuilding;
                HasBuilder = false;
                IsBuildNewBase = true;
                _isUnitAtFlag = false;
            }

            if (ResourseCount >= _resourceCountForCreateUnit && _canBuildNewBuilding == false)
            {
                Unit unit = Instantiate(_unit, transform.position, UnityEngine.Quaternion.identity);
                unit.SetBase(this);
                ResourseCount -= _resourceCountForCreateUnit;
            }

            yield return null;
        }
    }

    private void SetStatusIsUnitAtFlag()
    {
        _isUnitAtFlag = true;
    }

    private void SetStatusFlagCreated()
    {
        if (IsChose)
        {
            FlagIsCreated = true;
        }
    }
}
