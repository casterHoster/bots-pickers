using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Base : MonoBehaviour
{
    public event UnityAction NewCoordinateIsAdded;

    [SerializeField] private Generator _generator;

    private int _resourseCount = 0;

    public List<Resource> Resources = new List<Resource>();

    private void Awake()
    {
        _generator.ResourceIsAppeared += AddResourceCoordinate;
    }

    private void AddResourceCoordinate()
    {
        Resources.Add(_generator.GetCurrentResource());
        NewCoordinateIsAdded?.Invoke();
    }

    public Resource GetResource()
    {
        Resource resource = Resources[0];
        DeleteFirstReceivedResource();
        return resource;
    }

    private void DeleteFirstReceivedResource()
    {
        Resources.RemoveAt(0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Resource> (out Resource resource))
        {
            _resourseCount++;
            Debug.Log("Общее количество ресурсов: " + _resourseCount);
        }
    }
}
