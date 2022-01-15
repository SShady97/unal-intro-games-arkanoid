using UnityEngine;
using Random = UnityEngine.Random;

public class PowerUp2 : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Collider2D _collider;
    public void Init()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;
    }

    public void Hide()
    {
        _collider.enabled = false;
        gameObject.SetActive(false);
    }

}