using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private float _speed;

    private bool _isFree = true;
    private Resource _resourceOnScene;

    public Transform Target { get; private set; }

    private void Awake()
    {
        _base.NewCoordinateIsAdded += Update;
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

        if (_base.Resources.Count > 0 && _isFree == true) 
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
}
