using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    public event UnityAction Collected;
    public event UnityAction Delivered;
    private bool _isRaised = false;
    private Unit _unit;

    public bool isDelivered { get; private set;}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Unit>(out Unit unit) && _isRaised == false && unit.Target == transform)
        {
            transform.position.Set(unit.transform.position.x, unit.transform.position.y, unit.transform.position.z);
            transform.parent = unit.transform;
            _isRaised = true;
            _unit = unit;
            Collected?.Invoke();
        }

        if (collision.collider.TryGetComponent<Base>(out Base theBase))
        {
            if (_unit.GetBase().transform.Equals(theBase.transform))
            {
                isDelivered = true;
                Destroy(gameObject);
                Delivered?.Invoke();
            }
        }
    }
}
