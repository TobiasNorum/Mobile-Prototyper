using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Character : MonoBehaviour {
	
	float dirX, dirY;
	public int maxHealth;
	public int currentHealth;
	public int gold;
	public bool Walking;
	public bool Attacking;
	private bool isDodge;
	private static bool playerExists;
	float nextDodgeTime = 0f;
	public float dodgeRate = 2f;
	public GameObject hitEffect;
	public HealthBar healthBar;
	public Animator anim;
    public AudioSource missSound;
    public AudioSource coinsound;

    [SerializeField]
	float moveSpeed = 2f;

	void Start () {
		
		/*if (!playerExists)
		{
			playerExists = true;
			DontDestroyOnLoad(transform.gameObject);
		}
		else
		{
			Destroy(gameObject);
		}*/
		
		currentHealth = maxHealth;
		healthBar.SetMaxHealth(maxHealth);
		anim = GetComponent<Animator>();
		anim.speed = 1;
	}	
	void Update () {
		Move ();	
		float lastInputX = CrossPlatformInputManager.GetAxis("Horizontal");
		float lastInputY = CrossPlatformInputManager.GetAxis("Vertical");

		if (lastInputX != 0 || lastInputY != 0)
		{
			anim.SetBool("Walking", true);
			anim.SetFloat("LastMoveX", lastInputX);
			anim.SetFloat("LastMoveY", lastInputY);
			anim.Play("Walk");
		}
		else if (CrossPlatformInputManager.GetButtonDown("Fire1"))
		{
			anim.SetBool("Attacking", true);
			StartCoroutine(Attack());
			anim.Play("Attack");
		}
		else 
		{
			anim.SetBool("Walking", false);
		}

		float inputX = CrossPlatformInputManager.GetAxis("Horizontal");
		float inputY = CrossPlatformInputManager.GetAxis("Vertical");

		anim.SetFloat("SpeedX", inputX);
		anim.SetFloat("SpeedY", inputY);

		//if (CrossPlatformInputManager.GetButtonDown("Fire2"))
		if (Input.GetKeyDown(KeyCode.Space))
		{
			isDodge = true;
			nextDodgeTime = Time.time + 1f / dodgeRate;
			anim.Play("Dash");
		}
		/*if (Time.time > nextDodgeTime)
		{
		
			//if (CrossPlatformInputManager.GetButtonDown("Fire2"))
			if (Input.GetKeyDown(KeyCode.Space))
		{
			isDodge = true;
				nextDodgeTime = Time.time + 1f / dodgeRate;
		}		

		}*/
	}
	void FixedUpdate()
	{
		if (isDodge)
		{
			moveSpeed *= 10;
			isDodge = false;
		}
		else
		{			
			moveSpeed = 0.5f;
		}
	}

	void Move()
	{
		dirX = Mathf.RoundToInt(CrossPlatformInputManager.GetAxis ("Horizontal"));
		dirY = Mathf.RoundToInt(CrossPlatformInputManager.GetAxis ("Vertical"));

		transform.position = new Vector2 (dirX * moveSpeed * Time.deltaTime + transform.position.x,
			dirY * moveSpeed * Time.deltaTime + transform.position.y);
	}
	public void TakeDamage(int damage)
	{
		Instantiate(hitEffect, transform.position, Quaternion.identity);
		currentHealth -= damage;
		healthBar.SetHealth(currentHealth);
		if (currentHealth <= 0)
		{
			Destroy(gameObject);
			Debug.Log("Dead");
		}
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Coin"))
		{
			Debug.Log("Hit coin");
			ScoreTextScript.coinAmount += 1;
			Destroy(other.gameObject);
            coinsound.Play();
		}
	}
	public IEnumerator Attack()
	{
		yield return new WaitForSeconds(0.5f);
		Debug.Log("done attacking");
		anim.SetBool("Attacking", false);
        missSound.Play();
	}
}
