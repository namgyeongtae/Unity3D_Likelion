using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ChomperEnemyController : EnemyController, IWeaponObserver<GameObject>
{
    private MeleeWeaponController _meleeWeaponController;

    private void Start()
    {
        _meleeWeaponController = GetComponent<MeleeWeaponController>();
        _meleeWeaponController.Subscribe(this);
    }

    public void PlayStep()
    {

    }

    public void Grunt()
    {

    }

    public void AttackBegin()
    {

    }

    public void AttackEnd()
    {
        
    }

    public void OnNext(GameObject value)
    {
        // 
    }

    public void OnCompleted()
    {

    }
    
    public void OnError(Exception error)
    {

    }
}
