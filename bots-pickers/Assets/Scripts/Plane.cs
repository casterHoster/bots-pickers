using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Plane : MonoBehaviour, IPointerClickHandler
{
    public event UnityAction FlagIsCreated;

    [SerializeField] private Flag _flag;

    public Flag CurrentFlag { get; private set; } 

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentFlag != null)
        {
        CurrentFlag.Destroy();
        }

        Vector3 vector3 = eventData.pointerCurrentRaycast.worldPosition;
        vector3.y = vector3.y + 1;
        Flag flag = Instantiate(_flag, vector3, Quaternion.identity);
        CurrentFlag = flag;
        FlagIsCreated?.Invoke();
    } 
}
