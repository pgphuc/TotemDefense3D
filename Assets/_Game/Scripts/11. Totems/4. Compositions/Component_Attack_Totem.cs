using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component_Attack_Totem : ComponentBase
{
    public Component_Attack_Totem(GameUnit owner, float damage, float speed, GameObject shootArm)
    {
        _owner = owner;
        _damage = damage;
        _speed = speed;
        _shootArm = shootArm;
    }
    public override void OnInit()
    {
        _target = null;
    }
    public GameUnit _owner { get; set; }
    public float _lastAttackTime { get; set; }
    public float _damage { get; set; }
    public float _speed { get; set; }
    public Component_Health _target { get; set; }
    public List<Collider> _targetList = new List<Collider>();
    public void Attack()
    {
        //TODO: swing arm
        
        // _target = ComponentCache.GetHealthComponent(_targetList[0]);
        // _target.TakeDamage(_damage);
    }
    public void StopAttacking()
    {
        //TODO: cancel attack animation
        _lastAttackTime = Time.time;
    }
    
    #region totem attack

    public BulletBase _bullet;
    public GameObject _shootArm;
    public float swingAngle = 120f;  // Góc quay khi bắn
    public float swingTime = 0.3f;   // Thời gian quay xuống
    public float returnTime = 0.5f;  // Thời gian quay về vị trí ban đầu
    public AnimationCurve curve;     // Để điều chỉnh tốc độ xoay mượt mà

    private Quaternion startRotation => _shootArm.transform.rotation;
    private Quaternion targetRotation => Quaternion.Euler(new Vector3 (swingAngle, 0f, 0f)) * startRotation;

    private IEnumerator SwingArm()
    {
        float elapsedTime = 0f;
        while (elapsedTime < swingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = curve.Evaluate(elapsedTime / swingTime);
            _shootArm.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        //Ném viên đạn đi
        CoroutineManager.StartRoutine(_bullet.ShootBullet(BulletPathCalculator.ParabolPath
            (_bullet.transform.position, _targetList[0].transform.position)));
        // Đợi 1 chút trước khi quay về
        yield return new WaitForSeconds(0.2f);
        
        // Quay cánh tay về vị trí ban đầu
        elapsedTime = 0f;
        while (elapsedTime < returnTime)
        {
            elapsedTime += Time.deltaTime;
            float t = curve.Evaluate(elapsedTime / returnTime);
            _shootArm.transform.rotation = Quaternion.Slerp(targetRotation, startRotation, t);
            yield return null;
        }
        
        
    }

    #endregion
}
