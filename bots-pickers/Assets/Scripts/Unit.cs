using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] public Base _base;
    [SerializeField] private float _speed;
    [SerializeField] private BuildManager _buildManager;

    private bool _isFree = true;
    private Resource _resourceOnScene;

    public bool IsBuilder { get; private set; }

    public Transform Target { get; private set; }

    private void Update()
    {
        if (Target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, _speed * Time.deltaTime);
        }

        if (_base.GetFlagOfBuildNewBaseTransform() != null && _isFree == true && _base.HasBuilder == false && _buildManager.FlagIsCreated == true && _base.ResourseCount >= _base.ResourceCountForCreateBuilding)
        {
            _base.SetStatusBuilderIsTrue();
            _isFree = false;
            Target = _base.GetFlagOfBuildNewBaseTransform();
            IsBuilder = true;
        }

        if (_base.CurrentResource != null && _isFree == true)
        {
            _isFree = false;
            _resourceOnScene = _base.CurrentResource;
            _base.SetCurrentResourceNull();
            Target = _resourceOnScene.transform;
            _resourceOnScene.Collected += ChooseTargetBase;
            _resourceOnScene.Delivered += SetFree;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Flag>(out Flag flag) && IsBuilder == true)
        {
            Base newBase = Instantiate(_base, transform.position, Quaternion.identity);
            _base = newBase;
            IsBuilder = false;
            Target = null;
            _isFree = true;
        }
    }

    public Base GetBase()
    {
        return _base;
    }


    public void SetBase(Base newBase)
    {
        _base = newBase;
    }

    public void SetBuildManager(BuildManager buildManager)
    {
        _buildManager = buildManager;
    }

    private void ChooseTargetBase()
    {
        Target = _base.transform;
    }

    private void SetFree()
    {
        _isFree = true;
    }
}
