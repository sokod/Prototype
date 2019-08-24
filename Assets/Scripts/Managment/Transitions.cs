using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitions : MonoBehaviour
{
    Animator animator;
    int lvl=-1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTransition(int lvl)
    {
        this.lvl = lvl;
        animator.SetTrigger("ChangeLvl");
    }

    public void OnTransitionComplete()
    {
        SceneManager.LoadScene(lvl);
    }
}
