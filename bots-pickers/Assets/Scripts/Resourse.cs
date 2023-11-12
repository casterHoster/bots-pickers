using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    public event UnityAction Collected;
    public event UnityAction Delivered;
    private bool _isRaised = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Unit> (out Unit unit) && _isRaised == false && unit.Target == transform)
        {
            transform.parent = unit.transform;
            _isRaised = true;
            Collected?.Invoke();
        }

        if (collision.collider.TryGetComponent<Base> (out Base theBase))
        {
            Destroy(gameObject);
            Delivered?.Invoke();
        }
    }
}
