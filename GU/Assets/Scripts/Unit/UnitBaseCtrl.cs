using UnityEngine;
using System.Collections;

public class UnitBaseCtrl : MonoBehaviour {

    public enum AnimType
    {
        Idle = 0,
        Walk,
        Dead,
        Attack,
        Skill,
    };
    public AnimType eAnimType = AnimType.Idle;

    public Animator Anim; 

	void Start () {
        if (Anim == null)
            Anim = gameObject.GetComponent<Animator>();
        
        Anim.Play(eAnimType.ToString());

    }

    public void SetAnim(AnimType _AnimType)
    {
        if (eAnimType != _AnimType)
        {
            eAnimType = _AnimType;
            Anim.Play(eAnimType.ToString());
        }
    }
	
}
