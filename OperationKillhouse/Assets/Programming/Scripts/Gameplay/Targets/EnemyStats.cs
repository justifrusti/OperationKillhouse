using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    TargetManager targetManager;

    public int health = 1;
    bool death = false;
    public bool redTarget;
    bool targetActive;

    public bool popUp;
    public Animator animator;

    private void Awake()
    {
        targetManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TargetManager>();
        if(popUp)
        {
           PopUp();
        }
    }

    public void Damage(int damage)
    {
        if((health -= damage) <= 0)
        {
            if(!death)
            {
               Death();
            }

            if (redTarget)
            {
                targetManager.redTargets.Remove(this.gameObject);
            }
        }
    }

    public void Death()
    {
        print("dad");
        animator.SetTrigger("Dead");
        death = true;
    }

    public void PopUp()
    {
        animator.SetTrigger("Activate");
        targetActive = true;
        death = false;
        popUp = false;
    }

    public bool GetActivateTaget()
    {
        return targetActive;
    }

    public void SetTargetActive(bool active)
    {
        targetActive = active;
    }
}
