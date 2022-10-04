using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using Fusion;

public class AvatarAnimationController : NetworkBehaviour
{
    [SerializeField] private Animator animator;
    public XRController leftController;

    public override void FixedUpdateNetwork()
    {
        if(GetInput(out NetworkInputData data))
        {
            if(data.primaryAxisLeft.y>0)
            {
                this.animator.SetBool("isMoving", true);
                this.animator.SetFloat("animSpeed", 1.0f);
            }
            else if(data.primaryAxisLeft.y<0)
            {
                this.animator.SetBool("isMoving", true);
                this.animator.SetFloat("animSpeed", -1.0f);
            }
            else
            {
                this.animator.SetBool("isMoving", false);
                this.animator.SetFloat("animSpeed", 0.0f);
            }
        }
    }
}
