using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 1;
    public Animator Animator;
    bool death;
    TargetManager targetManager;
    public bool redTarget;

    private void Awake()
    {
        targetManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TargetManager>();
    }

    public void Damage(int damage)
    {
        if((health -= damage) <= 0)
        {
            death = true;

            if (redTarget)
            {
                targetManager.redTargets.Remove(this.gameObject);
            }
        }
    }

    public void Update()
    {
        if (death)
        {
            Animator.SetTrigger("Dead");
            
        }
    }

    public void PopUp()
    {
        Animator.SetTrigger("Activate");
        death = false;
    }
}
