using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    TargetManager targetManager;

    public int health = 1;
    public Animation animation;
    bool death = false;
    public bool redTarget;
    bool targetActive;
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
            animation.clip = deathAnim;
            animation.Play();
            death = false;
        }
    }

    public void PopUp()
    {
        animation.Play();
        targetActive = true;
        death = false;
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
