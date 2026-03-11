using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponTriggerZone
{
    public Vector3 position;
    public float radius;
}

public class MeleeWeaponController : MonoBehaviour, IWeaponObservable<GameObject>
{
    [SerializeField] private WeaponTriggerZone[] _triggerZones;
    [SerializeField] LayerMask targetLayerMask;

    private HashSet<Collider> _hitColliders = new HashSet<Collider>();
    private Vector3[] _prevTriggerPositions = new Vector3[0];

    private List<IWeaponObserver<GameObject>> _observers = new List<IWeaponObserver<GameObject>>();

    public void Subscribe(IWeaponObserver<GameObject> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Unsubscribe(IWeaponObserver<GameObject> observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }

    public void Notify(GameObject value)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(value);
        }
    }
}
