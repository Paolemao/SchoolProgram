using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Wepon : MonoBehaviour {

    private AllCharacter _user;

    public AllCharacter User
    {
        get
        {
            if (_user==null)
            {
                _user = GetComponent<AllCharacter>();
            }
            if (!_user)
            {
                _user = GetComponentInParent<AllCharacter>();//返回其子类
            }
            return _user;
        }
    }

    public bool IsEquiped { get; private set; }

    public UnityEvent onEquip;
    public UnityEvent onUnEquip;
    public UnityEvent onAttack;

    public virtual bool AttackOnceHandle() { return false; }
    public virtual bool AttackContinuouslyHandle() { return false; }


    public virtual void OnEquip()
    {
        IsEquiped = true;
        onEquip.Invoke();
    }
    public virtual void OnUnEquip()
    {
        IsEquiped = false;
        onUnEquip.Invoke();
    }
}
