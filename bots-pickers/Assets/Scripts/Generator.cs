using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Generator : MonoBehaviour
{
    [SerializeField] private List<Transform> _resourcePoints;
    [SerializeField] private Resource _resource;
    [SerializeField] private float _delay;
    [SerializeField] private int _maxCountResourseOffScene;

    private int _countResourseOnScene;
    private Resource _currentResource;

    public event UnityAction ResourceIsAppeared;

    private void Start()
    {
        StartCoroutine(CreateResource());
    }

    private void Awake()
    {
        _countResourseOnScene = 0;
    }

    public Resource GetCurrentResource()
    {
        return _currentResource;
    }

    private Transform GetRandomPoint(List<Transform> points)
    {
        Transform point = _resourcePoints[Random.Range(0, points.Count)];
        return point;
    }

    private IEnumerator CreateResource()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (true)
        {
            if (_maxCountResourseOffScene > _countResourseOnScene)
            {
                Resource resource = Instantiate(_resource, GetRandomPoint(_resourcePoints).position, Quaternion.identity);
                _currentResource = resource;
                _countResourseOnScene++;
                ResourceIsAppeared?.Invoke();
            }

            yield return delay;
        }
    }
}
