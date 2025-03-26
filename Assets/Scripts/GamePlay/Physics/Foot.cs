using UnityEngine;

public class Foot : MonoBehaviour
{
	
	public bool isGrounded = false;

	public void CalcGrounded()
	{
		isGrounded = (Physics2D.Raycast(transform.position, Vector2.down, GameManager.GROUNDNTHRESHOLD, 1 << GameManager.GROUNDLAYER));

	}
}
