using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] private List<Transform> _resourcePoints;
    [SerializeField] private Resource _resource;
    [SerializeField] private float _delay;
    [SerializeField] private int _maxCountResourcesOnScene;

    private int _countResourseOnScene;
    private List<Resource> _resources;

    private void Start()
    {
        StartCoroutine(CreateResource());
    }

    private void Awake()
    {
        _resources = new List<Resource>();
        _countResourseOnScene = 0;
    }

    public Resource GiveResourceAndDeleteItFromList()
    {
        if (_resources.Count > 0)
        {
            Resource resource = _resources[0];
            _resources.RemoveAt(0);
            return resource;
        }
        else
        {
            return null;
        }
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
            if (_maxCountResourcesOnScene > _countResourseOnScene)
            {
                Resource resource = Instantiate(_resource, GetRandomPoint(_resourcePoints).position, Quaternion.identity);
                _resources.Add(resource);
                _countResourseOnScene++;
            }

            yield return delay;
        }
    }
}
