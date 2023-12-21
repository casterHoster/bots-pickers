using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private Generator _generator;
    [SerializeField] private Unit _buildUnit;
    [SerializeField] private Plane _plane;
    [SerializeField] private Builder _buildManager;

    private const int _resourceCountForCreateUnit = 3;

    public event UnityAction BuildingIsCreated;

    public int ResourceCountForCreateBuilding { get; private set; }

    public int ResourseCount { get; private set; }

    public bool HasBuilderUnit { get; private set; }

    public Resource TargetResource { get; private set; }

    private void Start()
    {
        ResourseCount = 0;
        ResourceCountForCreateBuilding = 5;
        StartCoroutine(Create());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Resource>(out Resource resource) && resource.isDelivered)
        {
            ResourseCount++;
        }
    }

    private void Update()
    {
        GetResource();
    }

    public void GetResource()
    {
        if (TargetResource == null)
        {
            TargetResource = _generator.GiveResourceAndDeleteItFromList();
        }
    }

    public Transform GetFlagNewBase()
    {
        if (_plane.TargetFlag != null)
        {
            return _plane.TargetFlag.transform;
        }

        return null;
    }

    public void SetBuilderUnitForNewBase()
    {
        HasBuilderUnit = true;
    }

    public void SetLackTargetResource()
    {
        TargetResource = null;
    }

    public Transform SendUnitToBuild()
    {
        if (GetFlagNewBase() != null && HasBuilderUnit == false && _buildManager.IsFlagCreated == true && ResourseCount >= ResourceCountForCreateBuilding)
        {
            return GetFlagNewBase();
        }

        return null;
    }

    private IEnumerator Create()
    {
        while (enabled)
        {

            if (_buildManager.CanBuild && ResourseCount >= ResourceCountForCreateBuilding && _buildManager.IsUnitAtFlag)
            {
                ResourseCount -= ResourceCountForCreateBuilding;
                HasBuilderUnit = false;
                BuildingIsCreated?.Invoke();
            }

            if (ResourseCount >= _resourceCountForCreateUnit && _buildManager.CanBuild == false)
            {
                Unit unit = Instantiate(_buildUnit, transform.position, UnityEngine.Quaternion.identity);
                unit.SetBase(this);
                unit.SetBuildManager(_buildManager);
                ResourseCount -= _resourceCountForCreateUnit;
            }

            yield return null;
        }
    }
}
