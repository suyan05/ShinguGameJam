using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;

    [Header("X Wrapping / Z Clamp Limits")]
    public Vector2 limitX = new Vector2(-8f, 8f);   // X는 텔레포트
    public Vector2 limitZ = new Vector2(-4f, 4f);   // Z는 제한

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;     // 총알 프리팹
    public Transform firePoint;         // 발사 위치
    public float fireRate = 0.15f;      // 연사 속도

    private Vector2 moveInput;
    private float nextFireTime = 0f;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Update()
    {
        HandleMovement();
        HandleXWrapping();
        HandleZClamp();
        TryFire();
    }

    private void HandleMovement()
    {
        Vector3 dir = new Vector3(moveInput.x, 0, moveInput.y);
        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    private void HandleXWrapping()
    {
        Vector3 pos = transform.position;

        if (pos.x > limitX.y)
            pos.x = limitX.x;
        else if (pos.x < limitX.x)
            pos.x = limitX.y;

        transform.position = pos;
    }

    private void HandleZClamp()
    {
        Vector3 pos = transform.position;

        pos.z = Mathf.Clamp(pos.z, limitZ.x, limitZ.y);

        transform.position = pos;
    }

    private void TryFire()
    {
        if (Time.time < nextFireTime)
            return;

        nextFireTime = Time.time + fireRate;

        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
