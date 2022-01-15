using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    private float _width = 2.5f;
    [SerializeField]
    private float _speed = 5;
    [SerializeField]
    private float _movementLimit = 10;

    private Vector3 _targetPosition;
    private int score = 0;
    private Camera _cam;

    [SerializeField]
    private ArkanoidController _arkanoidController;

    private Camera Camera {
        get {
            if (_cam == null)
            {
                _cam = Camera.main;
            }
            return _cam;
        }
    }

    private float powerUpTime = 0f;
    private float _time = 0f;

    void Update() {
        _targetPosition.x = Camera.ScreenToWorldPoint(Input.mousePosition).x;
        _targetPosition.x = Mathf.Clamp(_targetPosition.x, -_movementLimit, _movementLimit);
        _targetPosition.y = this.transform.position.y;
        
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _speed);

        if(powerUpTime > 0){
            if(_time <= powerUpTime){
                _time += Time.deltaTime;
            }else{
                powerUpTime = 0f;
                _time = 0f;
                transform.localScale = new Vector3(1f,1f,1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 
        PowerUp1 powerUp1Hit;
        PowerUp1_1 powerUp1_1Hit;
        PowerUp2 powerUp2Hit;
        PowerUp3 powerUp3Hit;
        PowerUp3_1 powerUp3_1Hit;
        PowerUp3_2 powerUp3_2Hit;
        PowerUp3_3 powerUp3_3Hit;
        
        if (other.TryGetComponent(out powerUp1Hit))
        {
            transform.localScale = new Vector3(2f,1f,1f);
            powerUpTime = 7f;
            _time = 0f;
            Destroy(powerUp1Hit.gameObject);
        } else if (other.TryGetComponent(out powerUp1_1Hit))
        {
            transform.localScale = new Vector3(0.5f,0.5f,0.5f);
            powerUpTime = 7f;
            _time = 0f;
            Destroy(powerUp1_1Hit.gameObject);
        } else if (other.TryGetComponent(out powerUp2Hit))
        {
            int aux = 0;
            int balls = _arkanoidController._balls.Count;

            if(balls < 3){
                for (int i = 0; i < (3-balls); i++)
                {
                    _arkanoidController.SetInitialBall(2);
                }
            }
            
            Destroy(powerUp2Hit.gameObject);
        } else if (other.TryGetComponent(out powerUp3Hit))
        {
            _arkanoidController.scorePlus(50);
            Destroy(powerUp3Hit.gameObject);
        } else if (other.TryGetComponent(out powerUp3_1Hit))
        {
            _arkanoidController.scorePlus(100);
            Destroy(powerUp3_1Hit.gameObject);
        } else if (other.TryGetComponent(out powerUp3_2Hit))
        {
            _arkanoidController.scorePlus(250);
            Destroy(powerUp3_2Hit.gameObject);
        } else if (other.TryGetComponent(out powerUp3_3Hit))
        {
            _arkanoidController.scorePlus(500);
            Destroy(powerUp3_3Hit.gameObject);
        }
    }
}