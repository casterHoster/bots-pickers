using UnityEngine;
using UnityEngine.Events;

public class Flag : MonoBehaviour
{
    public event UnityAction UnitOnPointAndCanBuild;

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Unit>(out Unit unit) && unit.IsBuilder == true) 
        {
            UnitOnPointAndCanBuild.Invoke();
        }
    }
}
