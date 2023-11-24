using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] public Base _base;
    [SerializeField] private float _speed;

    private Resource _resourceOnScene;

    public bool IsFree { get; private set; }

    public bool IsBuilder {  get; private set; }

    public Transform Target { get; private set; }

    private void Awake()
    {
        _base.ResourceIsGot += GetTarget;
        IsFree = true;
    }

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

        if (_base.GetFlagOfBuildNewBaseTransform()  != null && IsFree == true && _base.HasBuilder == false && _base.FlagIsCreated == true && _base.ResourseCount >= _base.ResourceCountForCreateBuilding)
        {
            _base.SetStatusHasBuilderIsTrue();
            IsFree = false;
            Target = _base.GetFlagOfBuildNewBaseTransform();
            IsBuilder = true;
        }

        if (_base.IsBuildNewBase && IsBuilder == true)
        {
            SetBase(_base.NewBase);
            _base.SetStatusIsBuildNewBaseFalse();
            IsBuilder = false;
            Target = null;
            IsFree = true;
        }
    }

    private void GetTarget()
    {
        if (IsFree)
        {
            IsFree = false;
            _resourceOnScene = _base.CurrentResourceToCollect;
            Target = _resourceOnScene.transform;
            _base.SetCurrentResourceToCollectNull();
            _resourceOnScene.Collected += ChooseTargetBase;
            _resourceOnScene.Delivered += SetFree;
        }
    }

    private void SetFree()
    {
        IsFree = true;
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
