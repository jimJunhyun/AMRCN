using UnityEngine;

public class Foot : MonoBehaviour
{
	
	public bool isGrounded = false;
	//public bool clipped = false;


	internal RaycastHit2D sampledHit;

	public void CalcGrounded()
	{
		sampledHit = (Physics2D.Raycast(transform.position + (Vector3.up * GameManager.SAMPLEPOINTDISTANCE), Vector2.down, Mathf.Infinity, 1 << GameManager.GROUNDLAYER));

		isGrounded = (Physics2D.Raycast(transform.position, Vector2.down, GameManager.GROUNDTHRESHOLD, 1 << GameManager.GROUNDLAYER));
		//clipped = isGrounded && Physics2D.Raycast(transform.position + (Vector3.up * GameManager.SMALL), Vector2.up, GameManager.GROUNDTHRESHOLD, 1 << GameManager.GROUNDLAYER);
	}
}
