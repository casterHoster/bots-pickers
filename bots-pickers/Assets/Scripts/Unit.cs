using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] public Base _base;
    [SerializeField] private float _speed;

    private bool _isFree = true;
    private Resource _resourceOnScene;

    public bool IsBuilder { get; private set; }

    public Transform Target { get; private set; }

    private void ChooseTargetBase()
    {
        Target = _base.transform;
    }

    private void Update()
    {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, _speed * Time.deltaTime);
        }

        if (_base.GetFlagOfBuildNewBaseTransform() != null && _isFree == true && _base.HasBuilder == false && _base.FlagIsCreated == true && _base.ResourseCount >= _base.ResourceCountForCreateBuilding)
        {
            _base.SetStatusBuilderIsTrue();
            _isFree = false;
            Target = _base.GetFlagOfBuildNewBaseTransform();
            IsBuilder = true;
        }

        if (_base.IsBuildNewBase && IsBuilder == true)
        {
            _base.SetStatusIsBuildNewBaseFalse();
            SetBase(_base.NewBase);
            IsBuilder = false;
            Target = null;
            _isFree = true;
        }

        if (_base.GetResource() != null && _isFree == true)  // ?????????????
        {
            _isFree = false;
            _resourceOnScene = _base.GetResource();
            Target = _resourceOnScene.transform;
            _resourceOnScene.Collected += ChooseTargetBase;
            _resourceOnScene.Delivered += SetFree;
        }
    }

    private void SetFree()
    {
        _isFree = true;
    }

    public Base GetBase()
    {
        return _base;
    }

    public void SetBase(Base newBase)
    {
        _base = newBase;
    }
}
