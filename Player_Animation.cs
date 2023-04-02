using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    private Animator playerAnimator;
    private bool left=false, right=false;
    private Player playerObject;
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool("left",left);
        playerAnimator.SetBool("right",right);
    }
    public void leftAnim()
    {
        left=true;
        right = false;
    }
    public void rightAnim()
    {
        left=false;
        right = true;
    }
    public void idleAnim()
    {
        left=false;
        right=false;
    }
}
