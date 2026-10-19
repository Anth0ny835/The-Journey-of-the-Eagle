using System.Collections;
using System.Collections.Generic; // 必须包含此行
using UnityEngine;

public class EagleAttackController : MonoBehaviour
{
    public GameObject[] potentialTargets; // 可攻击目标数组（通过 Inspector 设置）
    public float minAttackDistance = 5f; // 攻击的最小距离
    public float damageAmount = 10f; // 每次攻击造成的伤害
    public GameObject hitEffectPrefab; // 命中效果预制体（可选）
    public List<AudioClip> attackSounds = new List<AudioClip>(); // 攻击音效列表
    public AudioSource audioSource; // 用于播放音效的 AudioSource 组件
    private GameObject target; // 当前攻击目标
    private Animator animator; // 控制角色动画
    private int currentSoundIndex = 0; // 当前播放的音效索引

    void Start()
    {
        animator = GetComponent<Animator>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OnAttackButtonPressed()
    {
        // 查找最近的目标
        TrySetTarget();

        PlaySequentialAttackSound();

        // 执行攻击
        PerformAttack();
    }

    private void TrySetTarget()
    {
        target = null; // 初始化为空
        float closestDistance = minAttackDistance + 1; // 初始距离超出攻击范围

        // 动态查找场景中所有挂有 Target 脚本的对象
        Target[] allTargets = FindObjectsOfType<Target>();

        foreach (Target targetScript in allTargets)
        {
            // 获取目标所在的 GameObject
            GameObject potentialTarget = targetScript.gameObject;
            
            // 如果目标已被销毁或禁用，则跳过
            if (!potentialTarget.activeInHierarchy)
                continue;

            float distance = Vector3.Distance(transform.position, potentialTarget.transform.position);
            if (distance <= minAttackDistance && (target == null || distance < closestDistance))
            {
                target = potentialTarget; // 设置最近的目标
                closestDistance = distance;
            }
        }
    }

    private void PerformAttack()
    {
        if (target != null)
        {
            // 朝向目标
            transform.LookAt(target.transform);

            // 播放攻击动画
            animator.SetTrigger("Attack");

            // 造成伤害
            DealDamage();
        }
        else
        {
            // 显示没有目标提醒
            Debug.Log("没有目标在攻击范围内！");
            
            // 播放攻击动画，模拟攻击动作
            animator.SetTrigger("Attack");
        }
    }

    private void DealDamage()
    {
        if (target != null)
        {
            // 获取目标的 Target 脚本并造成伤害
            Target targetScript = target.GetComponent<Target>();
            if (targetScript != null)
            {
                targetScript.TakeDamage(damageAmount);
            }

            // 播放目标的受击动画
            Animator targetAnimator = target.GetComponent<Animator>();
            if (targetAnimator != null)
            {
                targetAnimator.SetTrigger("Hit");
            }

            // 创建命中特效（可选）
            if (hitEffectPrefab != null)
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab);
                hitEffect.transform.position = target.transform.position + Vector3.up;
                Destroy(hitEffect, 2f); // 2 秒后销毁命中特效
            }
        }
    }

    private void PlaySequentialAttackSound()
    {
        if (attackSounds.Count > 0)
        {
            audioSource.PlayOneShot(attackSounds[currentSoundIndex]);

            // 更新索引，循环播放
            currentSoundIndex = (currentSoundIndex + 1) % attackSounds.Count;
        }
        else
        {
            Debug.LogWarning("攻击音效列表为空！");
        }
    }
}
