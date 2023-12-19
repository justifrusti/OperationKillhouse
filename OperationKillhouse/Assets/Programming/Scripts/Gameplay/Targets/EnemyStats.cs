using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 1;
    //public Animator Animator;
    public Animation animation;
    bool death;
    TargetManager targetManager;
    public bool redTarget;

    public AnimationClip deathAnim;

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
            //Animator.SetTrigger("Dead");
            
            animation.clip = deathAnim;
            animation.Play();
            death = false;
        }
    }

    public void PopUp()
    {
        animation.Play();
        death = false;
    }
}
