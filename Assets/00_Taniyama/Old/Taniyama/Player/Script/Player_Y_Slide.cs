using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Y_Slide : MonoBehaviour
{
    Animator anim;
    [SerializeField] Vector2 speed = new Vector2(1, 1);
    Vector2 inputSpeed;

    [SerializeField] Transform cam;

    I_State state;
    IdleState idle;
    WalkState walk;

    private void Start()
    {
        anim = GetComponent<Animator>();
        idle = new IdleState(this);
        walk = new WalkState(this);

        ChangeState(idle);
    }

    private void Update()
    {
        inputSpeed.x = Input.GetAxis("Horizontal");
        inputSpeed.y = Input.GetAxis("Vertical");

        state.OnUpdate();
    }

    private void FixedUpdate()
    {
        state.OnFixedUpdate();
    }

    private void ChangeState(I_State next)
    {
        state?.OnExit();
        state = next;
        state.OnEnter();
    }

    private void ChengeDirection(bool isLeft)
    {
        Vector3 direction = transform.localScale;
        direction.x = Mathf.Abs(direction.x) * ((isLeft) ? (1) : (-1));
        transform.localScale = direction;
    }

    public interface I_State
    {
        public void OnEnter();
        public void OnUpdate();
        public void OnFixedUpdate();
        public void OnExit();
    }

    public class IdleState : I_State
    {
        Player_Y_Slide player;
        public IdleState(Player_Y_Slide player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
            
        }

        public void OnUpdate()
        {
            if (player.inputSpeed.magnitude > 0.3f)
            {
                player.ChangeState(player.walk);
            }
        }

        public void OnFixedUpdate()
        {

        }

        public void OnExit()
        {

        }
    }

    public class WalkState : I_State
    {
        Player_Y_Slide player;
        public WalkState(Player_Y_Slide player)
        {
            this.player = player;
        }

        public void OnEnter()
        {
            player.anim.SetBool("isWalk", true);
        }

        public void OnUpdate()
        {
            if(player.inputSpeed.magnitude < 0.1f)
            {
                player.ChangeState(player.idle);
            }
        }

        public void OnFixedUpdate()
        {
            if(player.inputSpeed.x > 0)
            {
                player.ChengeDirection(false);
            }
            else if(player.inputSpeed.x < 0)
            {
                player.ChengeDirection(true);
            }

            Vector3 move = new Vector3(player.speed.x * player.inputSpeed.x,0, player.speed.y * player.inputSpeed.y);
            player.transform.position += move;

            Vector3 camPos = player.transform.position;
            camPos.y = 5;
            player.cam.position = camPos;
        }

        public void OnExit()
        {
            player.anim.SetBool("isWalk", false);
        }
    }
}
