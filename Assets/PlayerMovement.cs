using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float JumpPower;
    private float jumps = 0;
    private Vector2 velocity = Vector2.zero;
    public float gravity = -.5f;

    // Use this for initialization
    void Start()
    {
    
    }
        
    // Update is called once per frame
    void Update()
    {
        velocity.x = 0.0f;

        if (Input.GetButton("Left"))
        {

            velocity.x = -1.0f * Speed * Time.deltaTime;
        }
            
        if (Input.GetButton("Right"))
        {
            velocity.x = 1.0f * Speed * Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (jumps == 0 || jumps == 1)
            {
            
                velocity.y = JumpPower;
    
                Debug.Log("jumping!");
                //Debug.Break();
                jumps++;
            } 
        }

        UpdateVelocityX();
        UpdateVelocityY();

        transform.position = new Vector2(transform.position.x + velocity.x, transform.position.y + velocity.y);
    }

    void UpdateVelocityX()
    {
        float directionModifier = Mathf.Sign(velocity.x);
        
        if (directionModifier != 0.0f)
        {
            Vector3 positionEdge = transform.position;
            float edgeOffset = directionModifier * renderer.bounds.extents.x;
            positionEdge.x += edgeOffset;
            positionEdge.y += renderer.bounds.extents.y;

            int playerLayer = 8;            
            int playerMask = 1 << playerLayer;            
            int layerMask = playerMask;            
            
            RaycastHit2D hitInfo = Physics2D.Raycast(positionEdge, Vector3.Normalize(new Vector2(velocity.x, 0)), Mathf.Infinity, layerMask);
            
            if (hitInfo.fraction > Mathf.Abs(velocity.x) || hitInfo.collider == null)
            {
                positionEdge.y += (-2.0f * renderer.bounds.extents.y) + 0.1f;

                RaycastHit2D hitInfo2 = Physics2D.Raycast(positionEdge, Vector3.Normalize(new Vector2(velocity.x, 0)), Mathf.Infinity, layerMask);

                if (hitInfo2.fraction > Mathf.Abs(velocity.x) || hitInfo2.collider == null)
                {
                } else
                {
                    velocity.x = hitInfo2.fraction * directionModifier;
                }
            } else
            {
                velocity.x = hitInfo.fraction * directionModifier;
            }
        }
    }

    void UpdateVelocityY()
    {
        velocity.y += gravity * Time.deltaTime;
        
        float directionModifier = Mathf.Sign(velocity.y);
        
        if (directionModifier != 0.0f)
        {
            Vector3 positionEdge = transform.position;
            float edgeOffset = directionModifier * renderer.bounds.extents.y;
            positionEdge.y += edgeOffset;
            positionEdge.x += renderer.bounds.extents.x - 0.1f;
            
            int playerLayer = 8;            
            int playerMask = 1 << playerLayer;            
            int layerMask = playerMask;            
            
            RaycastHit2D hitInfo = Physics2D.Raycast(positionEdge, Vector3.Normalize(new Vector2(0, velocity.y)), Mathf.Infinity, layerMask);
            
            if (hitInfo.fraction > Mathf.Abs(velocity.y) || hitInfo.collider == null)
            {
                positionEdge.x += (-2.0f * renderer.bounds.extents.y) + 0.2f;
                
                RaycastHit2D hitInfo2 = Physics2D.Raycast(positionEdge, Vector3.Normalize(new Vector2(0, velocity.y)), Mathf.Infinity, layerMask);
                
                if (hitInfo2.fraction > Mathf.Abs(velocity.y) || hitInfo2.collider == null)
                {
                } else
                {
                    velocity.y = (hitInfo2.fraction) * directionModifier;
                    jumps = 0;
                }
            } else
            {
                velocity.y = hitInfo.fraction * directionModifier;
                jumps = 0;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        jumps = 0;

    }
}

