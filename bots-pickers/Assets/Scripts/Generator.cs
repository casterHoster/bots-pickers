using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Generator : MonoBehaviour
{
    [SerializeField] private List<Transform> _resourcePoints;
    [SerializeField] private Resource _resourceSample;
    [SerializeField] private float _delay;
    [SerializeField] private int _maxCountResourse;

    private int _countResourse;
    private List<Resource> _resources;

    private void Start()
    {
        StartCoroutine(CreateResource());
    }

    private void Awake()
    {
        _resources = new List<Resource>();
        _countResourse = 0;
    }

    public Resource GiveFirstListedResource()
    {
        if (_resources.Count > 0)
        {
            Resource givenResource = _resources[0];
            _resources.RemoveAt(0);
            return givenResource;
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
            if (_maxCountResourse > _countResourse)
            {
                Resource resource = Instantiate(_resourceSample, GetRandomPoint(_resourcePoints).position, Quaternion.identity);
                _resources.Add(resource);
                _countResourse++;
            }
                
            yield return delay;
        }
    }
}
