using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerControls : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float timer = 0f;
    private GameEngine engine;

    private float rotateSpeed = 100f;
    private float speed = 10f;
    public float fireRate = .2f;

    private bool followMouse = true;

    // Start is called before the first frame update
    void Start()
    {
        engine = FindObjectOfType<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mouse Movement
        if (Input.GetKeyDown(KeyCode.M))
        {
            followMouse = !followMouse;
            engine.mouseMode = followMouse;
        }
        Vector2 pos = transform.position;
        if (followMouse)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
        }
        //

        //Player WASD movement
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if (horizontalInput != 0)
        {
            transform.Rotate(transform.forward, Time.deltaTime * -horizontalInput * rotateSpeed);
        }
        if (verticalInput != 0)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime * verticalInput);
        }
        //

        //Fire Egg
        if (Input.GetKey(KeyCode.Space) && Time.time > timer) 
        {
            timer = Time.time + fireRate;
            GameObject egg = Instantiate(Resources.Load("Prefabs/Egg") as GameObject);
            egg.transform.localPosition = transform.localPosition;
            egg.transform.rotation = transform.rotation;
            engine.NewEgg();
        }
        //
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Plane")
        {
            Destroy(collision.gameObject);
            engine.PlaneDestroyed();
        }
    }
}
