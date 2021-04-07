using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class player : MonoBehaviour {
	public float Factor;

	public float MaxDistance = 5;

	public GameObject Stage;

	// 盒子仓库，可以放上各种盒子的prefab，用于动态生成。
	public GameObject[] BoxTemplates;

	public Transform Camera;

	public Text ScoreText;

	// 小人头部
	public Transform Head;

	// 小人身体
	public Transform Body;

	// 粒子效果
	public GameObject Particle;

	Vector3 _direction = new Vector3(1, 0, 0);
	private Rigidbody _rigidbody;
	private float _startTime;
	private GameObject _currentStage;
	private Collider _lastCollisionCollider;
	private Vector3 _cameraRelativaPosition;
	private int _score;
	private bool _enableInput = true;

	private int maxListLen = 3;
	private List<int> curListLen = new List<int>();
	List<LinkedList<GameObject>> stageList = new List<LinkedList<GameObject>>();

	// Use this for initialization
	void Start () {
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.centerOfMass = new Vector3(0, 0, 0);

		_currentStage = Stage;
		_lastCollisionCollider = _currentStage.GetComponent<Collider>();

		///
		for (int i =0;i< BoxTemplates.Length; i++)
        {
			LinkedList<GameObject> _stageList = new LinkedList<GameObject>();
			curListLen.Add(0);
			stageList.Add(_stageList);
		}
		stageList[0].AddLast(Stage);
		curListLen[0]++;
		//stageList.AddLast(Stage2);

		// Debug.Log(stageList);
		///

		SpawnStage();

		_cameraRelativaPosition = Camera.position - transform.position;
		Particle.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_startTime = Time.time;
			Particle.SetActive(true);
		}

		if (Input.GetKeyUp(KeyCode.Space))
		{
			var elapse = Time.time - _startTime;
			OnJump(elapse);
			Particle.SetActive(false);
		}


	}

	void OnJump(float elapse)
    {
		// _rigidbody.AddForce(new Vector3(1, 1, 0) * elapse * Factor, ForceMode.Impulse);
		_rigidbody.AddForce(new Vector3(0, 3f, 0) + (_direction) * elapse * Factor, ForceMode.Impulse);
		// 棋子的旋转效果。
		// transform.DOLocalRotate(new Vector3(0, 0, -360), 0.6f, RotateMode.LocalAxisAdd);
	}

	void SpawnStage()
    {


		GameObject prefab;
		// GameObject prefab = stageList.First.Value;
		// stageList.RemoveLast(); /// 
		int prefab_ind = 0;
		if (BoxTemplates.Length > 0)
		{
			// 从盒子库中随机取盒子进行动态生成
			// prefab = BoxTemplates[Random.Range(0, BoxTemplates.Length)];
			prefab_ind = Random.Range(0, BoxTemplates.Length);
			prefab = BoxTemplates[prefab_ind];
		}
		else
		{
			prefab = Stage;
		}

		// stageList.AddFirst(prefab); ///

		GameObject stage;
		if(curListLen[prefab_ind] < maxListLen)
        {
			stage = Instantiate(prefab);
			//Debug.Log(stage.Mesh());
			stageList[prefab_ind].AddLast(stage);
			curListLen[prefab_ind]++;
		}
        else
        {
			stage = stageList[prefab_ind].First.Value;
			stageList[prefab_ind].RemoveFirst();
			stageList[prefab_ind].AddLast(stage);
		}
		
		stage.transform.position = _currentStage.transform.position + _direction * Random.Range(1.1f, MaxDistance);

		var randomScale = Random.Range(0.5f, 1);
		// stage.transform.localScale = new Vector3(randomScale, 0.5f, randomScale);
		stage.transform.localScale = new Vector3(randomScale, prefab.transform.localScale[1], randomScale);

		// 重载函数 或 重载方法
		stage.GetComponent<Renderer>().material.color =
			new Color(Random.Range(0f, 1), Random.Range(0f, 1), Random.Range(0f, 1));
	}

	void OnCollisionEnter(Collision collision)
    {
		//Debug.Log(collision.gameObject.name);
		// Debug.Log(collision.collider);
		// _currentStage = collision.gameObject;
		if (collision.gameObject.name.Contains("Stage") && collision.collider != _lastCollisionCollider)
        {
			_lastCollisionCollider = collision.collider;
			_currentStage = collision.gameObject;
			RandomDirection();
			SpawnStage();
			MoveCamera();

			_score++;
			ScoreText.text = _score.ToString();

			
		}

		if (collision.gameObject.name == "Ground")
        {
			// 输了， 重开

			SceneManager.LoadScene(1);
        }
    }

	void MoveCamera()
    {
		Camera.DOMove(transform.position + _cameraRelativaPosition, 1);
		//Camera.position = transform.position + _cameraRelativaPosition;
    }

	void RandomDirection()
	{
		var seed = Random.Range(0, 2);
		_direction = seed == 0 ? new Vector3(1, 0, 0) : new Vector3(0, 0, 1);
		transform.right = _direction;
	}
}
