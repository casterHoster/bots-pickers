using System.Collections;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] private Plane _plane;

    public bool FlagIsCreated { get; private set; }

    public bool BaseIsChose { get; private set; }

    public bool CanBuild { get; private set; }

    public bool IsUnitAtFlag { get; private set; }

    private void Start()
    {
        StartCoroutine(ProvideBuild());
    }

    private void Awake()
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
                BaseIsChose = true;
                theBase.BuildIsCreated += SetCharacteristicsDefault;
            }
        }
    }

    private IEnumerator ProvideBuild()
    {
        while (true)
        {
            if (BaseIsChose && FlagIsCreated)
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
        FlagIsCreated = false;
        BaseIsChose = false;
        _plane.CurrentFlag.Destroy();
        CanBuild = false;
        IsUnitAtFlag = false;
    }

    private void SetStatusIsUnitAtFlagTrue()
    {
        IsUnitAtFlag = true;
    }

    private void SetFlagCreatedIfBuildChose()
    {
        if (BaseIsChose)
        {
            FlagIsCreated = true;
        }
    }

}
