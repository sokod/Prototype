using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitions : MonoBehaviour
{
    Animator animator;
    int lvl;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTransition(int lvl)
    {
        animator.SetTrigger("ChangeLvl");
        this.lvl = lvl;
    }

    public void OnTransitionComplete()
    {
        SceneManager.LoadScene(lvl);
    }
}
