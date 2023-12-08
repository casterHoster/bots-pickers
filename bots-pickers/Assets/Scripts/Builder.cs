using System.Collections;
using UnityEngine;

public class Builder : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] private Plane _plane;

    public bool IsFlagCreated { get; private set; }

    public bool IsBaseChose { get; private set; }

    public bool CanBuild { get; private set; }

    public bool IsUnitAtFlag { get; private set; }

    private void Start()
    {
        StartCoroutine(ProvideBuild());
    }

    private void OnEnable()
    {
        _plane.FlagIsCreated += SetFlagCreatedIfBuildChose;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity) && hit.collider.TryGetComponent<Base>(out Base theBase))
            {
                IsBaseChose = true;
                theBase.BuildIsCreated += SetCharacteristicsDefault;
            }
        }
    }

    private IEnumerator ProvideBuild()
    {
        while (enabled)
        {
            if (IsBaseChose && IsFlagCreated)
            {
                CanBuild = true;
            }

            if (_plane.CurrentFlag != null)
            {
                _plane.CurrentFlag.UnitOnPointAndCanBuild += SetStatusIsUnitAtFlagTrue;
            }

            yield return null;
        }
    }

    private void SetCharacteristicsDefault()
    {
        IsFlagCreated = false;
        IsBaseChose = false;
        _plane.CurrentFlag.Destroy();
        CanBuild = false;
        IsUnitAtFlag = false;
        _plane.FlagIsCreated += SetFlagCreatedIfBuildChose;
    }

    private void SetStatusIsUnitAtFlagTrue()
    {
        IsUnitAtFlag = true;
    }

    private void SetFlagCreatedIfBuildChose()
    {
        if (IsBaseChose)
        {
            IsFlagCreated = true;
            _plane.FlagIsCreated -= SetFlagCreatedIfBuildChose;
        }
    }

}
