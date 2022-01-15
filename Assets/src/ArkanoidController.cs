using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArkanoidController : MonoBehaviour
{
    private const string BALL_PREFAB_PATH = "Prefabs/Ball";
    private const string POWERUP1_PREFAB_PATH = "Prefabs/PowerUp1";
    private const string POWERUP1_1_PREFAB_PATH = "Prefabs/PowerUp1_1";
    private const string POWERUP2_PREFAB_PATH = "Prefabs/PowerUp2";
    private const string POWERUP3_PREFAB_PATH = "Prefabs/PowerUp3";
    private const string POWERUP3_1_PREFAB_PATH = "Prefabs/PowerUp3_1";
    private const string POWERUP3_2_PREFAB_PATH = "Prefabs/PowerUp3_2";
    private const string POWERUP3_3_PREFAB_PATH = "Prefabs/PowerUp3_3";
    private readonly Vector2 BALL_INIT_POSITION = new Vector2(0, -0.86f);
    [SerializeField]
    private GridController _gridController;
    
    [Space(20)]
    [SerializeField]
    private List<LevelData> _levels = new List<LevelData>();
    
    private int _currentLevel = 1;

    private Ball _ballPrefab = null;
    public List<Ball> _balls = new List<Ball>();

    private PowerUp1 _powerup1Prefab = null;
    private PowerUp1_1 _powerup1_1Prefab = null;
    private PowerUp2 _powerup2Prefab = null;
    private PowerUp3 _powerup3Prefab = null;
    private PowerUp3_1 _powerup3_1Prefab = null;
    private PowerUp3_2 _powerup3_2Prefab = null;
    private PowerUp3_3 _powerup3_3Prefab = null;
    public int _totalScore = 0;

    private int _count = 1;
    
    
    private void Start()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent += OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent += OnBlockDestroyed;
    }

    private void OnDestroy()
    {
        ArkanoidEvent.OnBallReachDeadZoneEvent -= OnBallReachDeadZone;
        ArkanoidEvent.OnBlockDestroyedEvent -= OnBlockDestroyed;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitGame();
        }
    }
    
    private void InitGame()
    {
        _gridController.BuildGrid(_levels[0]);
        SetInitialBall(1);
        _currentLevel = 1;
        _totalScore = 0;
        ArkanoidEvent.OnGameStartEvent?.Invoke();
        ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(0, _totalScore);
    }
    

    public void SetInitialBall(int type)
    {
        Ball ball = null;

        if(type == 1){
            ClearBalls();
            ball = CreateBallAt(BALL_INIT_POSITION);
        } else if(type == 2){
            Vector2 position = new Vector2(_balls[0].transform.position.x, _balls[0].transform.position.y);
            ball = CreateBallAt(position);
        }
        ball.Init();
        _balls.Add(ball);
    }
    
    private Ball CreateBallAt(Vector2 position)
    {
        if (_ballPrefab == null)
        {
            _ballPrefab = Resources.Load<Ball>(BALL_PREFAB_PATH);
        }

        return Instantiate(_ballPrefab, position, Quaternion.identity);
    }
    
    
    private void ClearBalls()
    {
        for (int i = _balls.Count - 1; i >= 0; i--)
        {
            _balls[i].gameObject.SetActive(false);
            Destroy(_balls[i]);
        }
        
        _balls.Clear();
    }
    
    private void OnBallReachDeadZone(Ball ball)
    {
        ball.Hide();
        _balls.Remove(ball);
        Destroy(ball.gameObject);

        CheckGameOver();
    }
    
    private void CheckGameOver()
    {
        if (_balls.Count == 0)
        {
            //Game over
            ClearBalls();
            
            Debug.Log("Game Over: LOSE!!!");
            ArkanoidEvent.OnGameOverEvent?.Invoke();
        }
    }
    
    private void OnBlockDestroyed(int blockId)
    {
        BlockTile blockDestroyed = _gridController.GetBlockBy(blockId);
        Vector2 block_position = new Vector2(blockDestroyed.transform.position.x, blockDestroyed.transform.position.y);

        if (blockDestroyed != null)
        {
            _totalScore += blockDestroyed.Score;
            ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(blockDestroyed.Score, _totalScore);

            if(Random.value < 0.9f){
                float rand2 = Random.value;
                float rand3 = Random.value;
                string type = "";
                if(rand2 <= 0.33f){
                    if(rand3 <= 0.5f){
                        type = "PowerUp1";
                    }else{
                        type = "PowerUp1_1";
                    }
                }else if(rand2 > 0.33f && rand2 <= 0.66f){
                    type = "PowerUp2";
                }else{
                    if(rand3 <= 0.25f){
                        type = "PowerUp3";;
                    }else if(rand3 > 0.25 && rand3 <= 0.5){
                        type = "PowerUp3_1";
                    }else if(rand3 > 0.5 && rand3 <= 0.75){
                        type = "PowerUp3_2";
                    }else{
                        type = "PowerUp3_3";
                    }
                }
                SetPowerUps(block_position, type);
            }
        }
        if (_gridController.GetBlocksActive() == 0)
        {
            _currentLevel++;
            if (_currentLevel > _levels.Count)
            {
                ClearBalls();
                Debug.LogError("Game Over: WIN!!!!");
            }
            else
            {
                SetInitialBall(1);
                _gridController.BuildGrid(_levels[_currentLevel-1]);
                ArkanoidEvent.OnLevelUpdatedEvent?.Invoke(_currentLevel);
            }

        }
    }
    
    
    private void SetPowerUps(Vector2 position, string type)
    {
        if(type == "PowerUp1"){
            PowerUp1 powerUp1 = CreatePowerUp1At(position);
            powerUp1.Init();
        }else if(type == "PowerUp1_1"){
            PowerUp1_1 powerUp1_1 = CreatePowerUp1_1At(position);
            powerUp1_1.Init();
        }else if(type == "PowerUp2"){
            PowerUp2 powerUp2 = CreatePowerUp2At(position);
            powerUp2.Init();
        }else if(type == "PowerUp3"){
            PowerUp3 powerUp3 = CreatePowerUp3At(position);
            powerUp3.Init();
        }else if(type == "PowerUp3_1"){
            PowerUp3_1 powerUp3_1 = CreatePowerUp3_1At(position);
            powerUp3_1.Init();
        }else if(type == "PowerUp3_2"){
            PowerUp3_2 powerUp3_2 = CreatePowerUp3_2At(position);
            powerUp3_2.Init();
        }else if(type == "PowerUp3_3"){
            PowerUp3_3 powerUp3_3 = CreatePowerUp3_3At(position);
            powerUp3_3.Init();
        }
        
    }

    private PowerUp1 CreatePowerUp1At(Vector2 position)
    {
        if (_powerup1Prefab == null)
        {   
            _powerup1Prefab = Resources.Load<PowerUp1>(POWERUP1_PREFAB_PATH);
        }
        return Instantiate(_powerup1Prefab, position, Quaternion.identity);;
    }
    private PowerUp1_1 CreatePowerUp1_1At(Vector2 position)
    {
        if (_powerup1_1Prefab == null)
        {   
            _powerup1_1Prefab = Resources.Load<PowerUp1_1>(POWERUP1_1_PREFAB_PATH);
        }
        return Instantiate(_powerup1_1Prefab, position, Quaternion.identity);;
    }
    private PowerUp2 CreatePowerUp2At(Vector2 position)
    {
        if (_powerup2Prefab == null)
        {   
            _powerup2Prefab = Resources.Load<PowerUp2>(POWERUP2_PREFAB_PATH);
        }
        return Instantiate(_powerup2Prefab, position, Quaternion.identity);;
    }
    private PowerUp3 CreatePowerUp3At(Vector2 position)
    {
        if (_powerup3Prefab == null)
        {   
            _powerup3Prefab = Resources.Load<PowerUp3>(POWERUP3_PREFAB_PATH);
        }
        return Instantiate(_powerup3Prefab, position, Quaternion.identity);;
    }
    private PowerUp3_1 CreatePowerUp3_1At(Vector2 position)
    {
        if (_powerup3_1Prefab == null)
        {   
            _powerup3_1Prefab = Resources.Load<PowerUp3_1>(POWERUP3_1_PREFAB_PATH);
        }
        return Instantiate(_powerup3_1Prefab, position, Quaternion.identity);;
    }
    private PowerUp3_2 CreatePowerUp3_2At(Vector2 position)
    {
        if (_powerup3_2Prefab == null)
        {   
            _powerup3_2Prefab = Resources.Load<PowerUp3_2>(POWERUP3_2_PREFAB_PATH);
        }
        return Instantiate(_powerup3_2Prefab, position, Quaternion.identity);;
    }
    private PowerUp3_3 CreatePowerUp3_3At(Vector2 position)
    {
        if (_powerup3_3Prefab == null)
        {   
            _powerup3_3Prefab = Resources.Load<PowerUp3_3>(POWERUP3_3_PREFAB_PATH);
        }
        return Instantiate(_powerup3_3Prefab, position, Quaternion.identity);;
    }
    public void scorePlus (int plus){
        _totalScore += plus;
        ArkanoidEvent.OnScoreUpdatedEvent?.Invoke(plus, _totalScore);
    }
}
