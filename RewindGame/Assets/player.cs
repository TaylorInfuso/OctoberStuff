using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
	Vector3 x,z;
	[SerializeField] float playerSpeed;
	[SerializeField] float maxSpeed;
	[SerializeField] float jumpStrength;

	private Rigidbody rb;
	
	Vector3 [] record= new Vector3[50000];
	int i = 0;
	
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
		x = transform.right * Input.GetAxisRaw("Horizontal");
		z = transform.forward  * Input.GetAxisRaw("Vertical");
        
    }
	
	void FixedUpdate()
	{
		Vector3 movement = (x + z).normalized * playerSpeed;	 
		 if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
			 movement.y = jumpStrength;
		 rb.AddForce(movement);	
		 if((rb.velocity.x != 0 || rb.velocity.z!= 0 || rb.velocity.y!= 0) && !Input.GetKeyDown(KeyCode.Z))
		 {
			 i++;
			 if(i >= 50000)
				 i = 0;
			 //print(movement);
			 record[i] = rb.velocity;
			 //record[i] = movement;
			 
		 }
		else if (Input.GetKey(KeyCode.Z))
		{
			if(record[i] != new Vector3(0,0,0))
			{
				
				Vector3 v = rb.velocity;
				if(movement.x == 0)
					v.x = record[i].x * -1f;
				if(movement.z == 0)
					v.z = record[i].z * -1f;
				v.y = record[i].y * -1.25f;
				print("v.y: " + v.y);
				rb.velocity = v;
				//rb.AddForce(-record[i]);
				record[i] = new Vector3(0,0,0);
				
			}
				i--;
				if(i < 0)
				i = 49999;
		}
		if(!Input.GetKey(KeyCode.Z))
		{
			Vector3 temp = rb.velocity;
			temp.y = 0;
			temp = Vector3.ClampMagnitude(temp, maxSpeed);
			temp.y = rb.velocity.y;
			rb.velocity = temp;
		}
	}
	
		bool IsGrounded(){
			return Physics.Raycast(transform.position, -Vector3.up, 1);
	}
}
