using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int health = 1;
    public Animator Animator;
    bool death;

    private void Awake()
    {
        PopUp();
    }

    public void Damage(int damage)
    {
        if((health -= damage) <= 0)
        {
            death = true;
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
    }

}
