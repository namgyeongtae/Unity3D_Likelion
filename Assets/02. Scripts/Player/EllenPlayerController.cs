using UnityEngine;

public class EllenPlayerController : PlayerController
{
    [SerializeField] private Transform _weaponAttachTransform;

    private WeaponController _weaponController;

    protected override void Start()
    {
        base.Start();
        
        var staffObject = Resources.Load<GameObject>("Staff");
        _weaponController = Instantiate(staffObject, _weaponAttachTransform).GetComponent<WeaponController>();
    }

    public void MeleeAttackStart()
    {

    }

    public void MeleeAttackEnd()
    {
        
    }
}
