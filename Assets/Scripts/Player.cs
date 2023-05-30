using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float jumpForce = 300;
    public int vida=10;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    public GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("itsjumping", true);
            rigidbody2D.AddForce(new Vector2 (0, jumpForce));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("itsjumping", false);
            // Obtener la normal de la superficie de colisión
            Vector2 surfaceNormal = collision.contacts[0].normal;
           // Calcular la rotación para que el personaje caiga parado
           Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up, surfaceNormal);
          // Aplicar la rotación al personaje
          transform.rotation = targetRotation;
        }     
        
        if (collision.gameObject.tag == "obstaculo")
        {
            vida--;
            if (vida <= 0)
            {
                gameManager.gameOver = true;
                print("Has chocado");
            }
            else
            {
                Debug.Log("Has chocado. Vidas restantes: " + vida);
            }
            // Calcular la dirección del choque
            Vector2 pushDirection = transform.position - collision.transform.position;
            // Normalizar el vector de dirección
            pushDirection.Normalize();
            // Definir la dirección hacia arriba
            Vector2 upwardForce = new Vector2(0, 1);
            // Definir la dirección hacia el lado del choque
            Vector2 sideForce = new Vector2(pushDirection.y, -pushDirection.x);
            // Aplicar la fuerza de empuje
            rigidbody2D.AddForce((upwardForce + sideForce) * jumpForce);
            // Evitar que el personaje se quede atascado en el obstáculo
            rigidbody2D.velocity = Vector2.zero;
            // Evitar que el personaje rote
            rigidbody2D.freezeRotation = true;

        }
    }
}
