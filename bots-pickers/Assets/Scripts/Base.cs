using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    [SerializeField] private Generator _generator;
    [SerializeField] private Unit _unit;
    [SerializeField] private Plane _plane;
    [SerializeField] private Builder _buildManager;

    private int _resourceCountForCreateUnit = 3;

    public event UnityAction BuildIsCreated;

    public int ResourceCountForCreateBuilding { get; private set; }

    public int ResourseCount { get; private set; }

    public bool HasBuilder { get; private set; }

    public Resource CurrentResource { get; private set; }

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
        if (CurrentResource == null)
        {
            CurrentResource = _generator.GiveResourceAndDeleteItFromList();
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

    public void SetStatusBuilderIsTrue()
    {
        HasBuilder = true;
    }

    public void SetCurrentResourceNull()
    {
        CurrentResource = null;
    }

    public Transform SendUnitToBuild()
    {
        if (GetFlagOfBuildNewBaseTransform() != null && HasBuilder == false && _buildManager.IsFlagCreated == true && ResourseCount >= ResourceCountForCreateBuilding)
        {
            return GetFlagOfBuildNewBaseTransform();
        }
        else
        {
            return null;
        }
    }

    private IEnumerator Create()
    {
        while (enabled)
        {

            if (_buildManager.CanBuild && ResourseCount >= ResourceCountForCreateBuilding && _buildManager.IsUnitAtFlag)
            {
                ResourseCount -= ResourceCountForCreateBuilding;
                HasBuilder = false;
                BuildIsCreated?.Invoke();
            }

            if (ResourseCount >= _resourceCountForCreateUnit && _buildManager.CanBuild == false)
            {
                Unit unit = Instantiate(_unit, transform.position, UnityEngine.Quaternion.identity);
                unit.SetBase(this);
                unit.SetBuildManager(_buildManager);
                ResourseCount -= _resourceCountForCreateUnit;
            }

            yield return null;
        }
    }
}
