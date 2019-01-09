using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //添加UI命名空间

public class Player : MonoBehaviour {
	//============================
	//	玩家状态
	//============================
	public enum State {
		standby, //待机
		run, //奔跑（移动）
		rushing, //冲刺中
		slowDown, //贴墙缓慢下落
		die, //去死
		dead //死亡
	}

	//============================
	//	类型：可变化值
	//============================
	public struct Value {
		public int maxValue; //最大值
		public int nowValue; //现在值
		public void init (int initValue) {
			maxValue = initValue;
			nowValue = maxValue;
		} //初始化函数
		public void minus (int minusValue) {
			nowValue -= minusValue;
			if (nowValue < 0) {
				nowValue = 0;
			}
		}

		//	增加值（不会超过最大值）
		public void addTo (int addValue) {
			nowValue += addValue;
			if (nowValue > maxValue) {
				nowValue = maxValue;
			}
		}

		//	减少值（不会小于0）
		public bool isZero () {
			if (nowValue <= 0) {
				return true;
			} else {
				return false;
			}
		}
	}

	//============================
	//	类型：控制器
	//============================
	public struct operationValue {
		public float moveInput; //移动输入值：浮点数
		public bool isWannaJump; //跳跃输入值：是否
		public bool isWannaRush; //冲刺输入值：是否
		public bool isWannaFire; //发射输入值：是否
		public bool isWannaFindMove; //瞬移输入值：是否
		private bool isMacInput; //输入平台：决定手柄键位

		// 初始化
		public void init (bool isMac) {
			moveInput = 0f; //移动输入值 = 0
			isWannaJump = false; // 跳跃输入值 = 否
			isWannaRush = false; // 冲刺输入值 = 否
			isWannaFire = false; // 发射输入值 = 否
			isWannaFindMove = false; // 瞬移输入值 = 否
			isMacInput = isMac; // 获取平台类型
		}

		// 重新初始化
		public void reInit (bool isMac) {
			moveInput = 0f; //移动输入值 = 0
			isWannaJump = false; // 跳跃输入值 = 否
			isWannaRush = false; // 冲刺输入值 = 否
			isWannaFire = false; // 发射输入值 = 否
			isWannaFindMove = false; // 瞬移输入值 = 否
			isMacInput = isMac; // 获取平台类型
		}

		// 获取移动输入
		public void inputMove () {
			moveInput = Input.GetAxisRaw ("Horizontal"); //获取横向移动轴（AD，手柄左摇杆）
		}

		// 获取跳跃输入
		public void inputJump () {
			if (isMacInput) {
				// 当平台为 mac 时获取跳跃键和手柄对应按键
				if (Input.GetButtonDown ("Jump") || Input.GetKeyDown ("joystick button 16")) {
					isWannaJump = true;
				}
			}

		}

		// 获取发射输入
		public void inputFire () {
			if (isMacInput) {
				// 当平台为 mac 时获取射击键和手柄对应按键
				if (Input.GetButtonDown ("Fire2") || Input.GetKeyDown ("joystick button 14")) {
					isWannaFire = true;
				}
			}
		}

		// 获取冲刺输入
		public void inputRush () {
			if (isMacInput) {
				// 当平台为 mac 时获取冲刺键和手柄对应按键
				if (Input.GetButtonDown ("Fire1") || Input.GetKeyDown ("joystick button 17")) {
					isWannaRush = true;
				}
			}
		}

		// 获取瞬移输入
		public void inputFindMove () {
			if (isMacInput) {
				// 当平台为 mac 时获取瞬移键和手柄对应按键
				if (Input.GetButtonDown ("Fire3") || Input.GetKeyDown ("joystick button 13")) {
					isWannaFindMove = true;
				}
			}
		}

		public bool isInputed () {
			if (moveInput != 0 || isWannaFindMove || isWannaFire || isWannaJump || isWannaRush) {
				return true;
			} else {
				return false;
			}
		}
	}

	//===========================================================================
	//																			|
	//					玩家初始数值声明										   |
	//																			|
	//===========================================================================
	public float runSpeed = 5.0f; //移动速度
	public float jumpHeight = 2.0f; //跳跃高度
	public float highJumpPower = 4.0f; //高跳倍数
	static float direction = 1f; //方向
	public bool isFacingRight = true; //朝向（是否朝右）
	public bool isGrounded = false; //是否着地
	private bool isCanAirJump = false; //是否可以空中跳跃

	private Vector3 startJumpPosition; //起跳地点（坠落前地点）
	private bool isDead = false; //是否死亡
	static LayerMask groundLayer; //地面层
	static LayerMask enemyLayer; //敌人层
	private GameObject groundCheckPoint; //着地检测
	Rigidbody2D rigidbody; //玩家刚体
	private Animator animator; //玩家动画控制
	public State state; //玩家状态
	public Value rushPower; //玩家能量（使用能力时消耗）
	public int initMaxPower = 5; //初始玩家最大能量
	public float bulletSpeed = 6.0f; //子弹速度
	public float bulletFar = 30.0f; //子弹距离
	public int flashRunhPower = 2; //冲刺消耗能量
	public int flashMovePower = 2; //瞬移消耗能量
	public int firePower = 1; //发射子弹消耗的能量
	public PlayerBar playerBar; //高跳条状物	
	public PlayerBullet bullet; //子弹
	public float slowToDown = 0.2f; //缓慢落下倍率
	private float initGravity; //初始重力
	operationValue inputOperation; //输入控制器
	private GameObject startPoint; //关卡开始位置
	private SlowDownCheck slowDownCheck; //检查是否缓慢落下的碰撞
	public bool isWannaToExitSlowDown; // 是否想要离开缓慢下落
	public bool isMac = true; // 游戏构建平台（真为 mac；伪为 PC）
	private Vector2 startRushPosition; //开始冲刺的坐标
	private PlayerLight playerLight; //	玩家背景
	public int startNum = 99;
	private float deadTime = 9.0f / 6.0f;
	private Clock deadTimer;

	//===========================================================================
	//																			|
	//					玩家系统函数											  |
	//																			|
	//===========================================================================

	//======================================================
	//	玩家初始化函数
	//======================================================
	void init () {
		// 拾取操作对象
		rigidbody = GetComponent<Rigidbody2D> (); // 获取玩家刚体
		animator = GetComponent<Animator> (); // 获取玩家动画控制器
		startPoint = findedStartPoint ();
		if (startPoint == null) {
			startPoint = GameObject.FindGameObjectWithTag ("StartPoint");
		}
		slowDownCheck = transform.Find ("slowDownCheck").gameObject.GetComponent<SlowDownCheck> ();
		playerLight = transform.Find ("playerLight").gameObject.GetComponent<PlayerLight> ();

		// hp.HPStrip = GetComponentInChildren<Slider> ();
		// sePlayer = GetComponent<AudioSource> ();

		groundCheckPoint = transform.Find ("GroundCheckPoint").gameObject; //从子对象获取着地碰撞判定
		groundLayer = 1 << LayerMask.NameToLayer ("Ground"); //获取地面图层

		isFacingRight = true; // 初始朝向右
		isWannaToExitSlowDown = false;
		// hp.init (initHp);
		state = State.standby; //初始状态为待机
		rushPower.init (initMaxPower); //初始化玩家能量	

		initGravity = rigidbody.gravityScale;

		inputOperation.init (isMac);

		flashMoveTo (startPoint.transform.position);
		moveStop ();

	}

	//======================================================
	//	玩家操作输入函数 | 返回值：操作控制器
	//======================================================
	private operationValue operation () {
		inputOperation.reInit (isMac); // 重置操作控制器
		// 根据玩家状态区别录入操作
		switch (state) {
			case State.standby: // 玩家在待机状态时
				inputOperation.inputMove (); //获取移动
				inputOperation.inputJump (); //获取跳跃
				inputOperation.inputFire (); //获取发射
				inputOperation.inputRush (); //获取冲刺
				inputOperation.inputFindMove (); //获取瞬移
				break;
			case State.run: // 玩家在移动状态时
				inputOperation.inputMove (); //获取移动
				inputOperation.inputJump (); //获取跳跃
				inputOperation.inputFire (); //获取发射
				inputOperation.inputRush (); //获取冲刺
				inputOperation.inputFindMove (); //获取瞬移
				break;
			case State.slowDown: // 玩家在缓慢下落状态时
				inputOperation.inputMove (); //获取移动
				inputOperation.inputJump (); //获取跳跃
				inputOperation.inputFire (); //获取发射
				inputOperation.inputRush (); //获取冲刺
				inputOperation.inputFindMove (); //获取瞬移
				break;

		}

		return inputOperation; //返回操作控制器

	}

	//======================================================
	//	玩家处理函数
	//======================================================
	private void process () {
		// 根据玩家状态进行处理
		switch (state) {
			case State.standby:
				move (); //移动
				jump (); //跳跃
				startRush (); //冲刺
				fire (); //发射
				flashFindToMove (); //瞬移
				if (inputOperation.moveInput != 0) {
					moveStateTo (State.run); //当输入值非零时，更改玩家状态到移动
				}
				break;
			case State.run:
				move (); //移动
				jump (); //跳跃
				startRush (); //冲刺
				fire (); //发射
				flashFindToMove (); //瞬移
				if (inputOperation.moveInput == 0) {
					moveStateTo (State.standby); //当输入值为零时，更改玩家状态到待机
				}
				break;
			case State.rushing:
				_Rush ();
				break;
			case State.slowDown:
				startRush (); //冲刺
				fire (); //发射
				flashFindToMove (); //瞬移
				slowDown (); //缓慢下落

				break;
			case State.die:
				toDie ();
				break;

		}

	}

	//===========================================================================
	//																			|
	//					玩家主体函数											  |
	//																			|
	//===========================================================================
	//======================================================
	//	变更玩家状态函数
	//======================================================
	public void moveStateTo (State movedState) {
		switch (movedState) {
			case State.slowDown: //变更玩家状态到 缓慢下落
				direction = 0f; //玩家移动方向归零
				if (rushPower.isZero ()) {
					return;
				} //当玩家没有 XPOWER 时不更改状态，退出
				rigidbody.gravityScale *= slowToDown; //更改玩家质量实现缓慢下落
				isCanAirJump = true; //让玩家可以进行跳跃
				isFacingRight = !isFacingRight; // 让玩家朝向相反方向
				turnRound (); //转身
				break;
			case State.standby:

				break;
			case State.die:
				playerLight.moveStateTo (PlayerLight.State.dead);
				animator.SetBool ("isDead", true);
				deadTimer.init (deadTime);
				Debug.Log (deadTime);
				break;
		}

		state = movedState; //玩家的状态 = 要更改的状态
	}

	//======================================================
	//	玩家移动函数
	//======================================================
	void move () {
		float inJumpSlowDown = 0.4f; //在跳跃中的移动减缓倍数
		float moveInput = inputOperation.moveInput; // 同步玩家移动输入值

		checkIsGrounded (); //检测是否着地
		if (isGrounded) {
			//	着地时正常进行移动处理
			if (moveInput > 0) {
				direction = moveInput;
				isFacingRight = true;
			} else if (moveInput < 0) {
				direction = moveInput;
				isFacingRight = false;
			} else {
				direction = 0;
			}

		} else {
			if (!isCanAirJump) {
				// 跳跃中若冲刺后则必须二段跳后才能进行移动操作

				//跳跃中若想回头
				if ((moveInput > 0 && !isFacingRight) || (moveInput < 0 && isFacingRight)) {
					direction += moveInput * inJumpSlowDown;
					isFacingRight = !isFacingRight;
				} else if ((direction < 0 && moveInput > 0) || (direction > 0 && moveInput < 0)) {
					direction += moveInput * inJumpSlowDown;
				}
			}
		}
		rigidbody.velocity = new Vector2 (runSpeed * direction, rigidbody.velocity.y); // 根据获取的值移动玩家刚体
		turnRound (); //转身
	}

	//======================================================
	//	玩家跳跃函数
	//======================================================
	void jump () {
		if (inputOperation.isWannaJump) {
			// 在着地状态或冲刺后可二段跳的状态进行跳跃处理
			if (isGrounded || isCanAirJump) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, Mathf.Sqrt (2.0f * Mathf.Abs (Physics.gravity.y) * jumpHeight));
			}
			if (!isGrounded) {
				isCanAirJump = false; //在空中跳跃时重置二段跳变量
			}
			isGrounded = false; //着地->否
		}
	}

	//======================================================
	//	玩家开始一RUSH函数
	//======================================================
	private void startRush () {
		if (inputOperation.isWannaRush) {
			//	只有剩余能量大于需要能量时才执行
			if (rushPower.nowValue >= flashRunhPower) {
				startRushPosition = transform.position; //记录开始冲刺坐标
				moveStateTo (State.rushing); //状态变为冲刺
			} else {
				errorOut (); //能力不足时进行错误提示
			}
		}
	}

	//======================================================
	//	玩家一RUSH函数
	//======================================================
	private void _Rush () {
		float rushSpeed = 15.0f; //冲刺速度
		float endRushPower = 0.7f; //冲刺惯性
		if (isFacingRight) {
			float moveX = startRushPosition.x + 3.15f; //冲刺终点 X 坐标
			rigidbody.velocity = new Vector2 (rushSpeed, 0); // 根据获取的值移动玩家刚体
			if (transform.position.x > moveX) { //当到达距离时
				move_Bar (); //生成一状物
				direction = endRushPower; //施加惯性
				moveStateTo (State.run); //状态改为移动
			}
		} else {
			float moveX = startRushPosition.x - 3.15f; //冲刺终点 X 坐标
			rigidbody.velocity = new Vector2 (-rushSpeed, 0); // 根据获取的值移动玩家刚体
			if (transform.position.x < moveX) { //当到达距离时
				move_Bar (); //生成一状物
				direction = -endRushPower; //施加惯性
				moveStateTo (State.run); //状态改为移动
			}
		}

	}

	//======================================================
	//	玩家一状物生成函数
	//======================================================
	private void move_Bar () {
		float barX = startRushPosition.x + transform.localScale.x * 1.15f; //生成条状物的 X 坐标
		float barY = startRushPosition.y - (transform.localScale.y / 4.0f);
		playerBar.init (new Vector2 (barX, barY), flashRunhPower); //初始化条状物
		playerBar.changeFace (isFacingRight);
		Instantiate (playerBar); //生成条状物
		isCanAirJump = true; //允许玩家跳跃
		rushPower.minus (flashRunhPower); //扣除能量
	}

	//======================================================
	//	玩家〇SHOOT函数
	//======================================================
	private void fire () {
		if (inputOperation.isWannaFire) {
			float stopTime = 0.1f; // 时间暂停时间（单位：秒）
			if (rushPower.nowValue >= firePower) { //当 XPOWER 够用时才会执行
				if (isFacingRight) {
					bullet.init (transform.position, bulletSpeed, stopTime, bulletFar, firePower); //初始化子弹
					bullet.initPosition += new Vector2 (0.1f, 0.0f); //初始化子弹位置
				} else {
					bullet.init (transform.position, -bulletSpeed, stopTime, bulletFar, firePower); //初始化子弹
					bullet.initPosition -= new Vector2 (0.1f, 0.0f); //初始化子弹位置
				}
				Instantiate (bullet); //生成子弹
				rushPower.minus (firePower); //扣除 XPOWER
			} else {
				errorOut (); //当 XPOWER 不足时报错
			}
		}
	}

	//======================================================
	//	玩家〇移动函数
	//======================================================
	private void flashFindToMove () {
		if (inputOperation.isWannaFindMove) {
			GameObject[] playerBullets = null; // 子弹列表
			playerBullets = GameObject.FindGameObjectsWithTag ("PlayerBullet"); //寻址子弹
			if (playerBullets != null) { // 当列表不为空
				foreach (GameObject bullet in playerBullets) { //在列表中循环
					flashMoveTo (bullet.transform.position); //移动到第一个子弹的位置
					bullet.GetComponent<PlayerBullet> ().breakSelf (); //子弹自我销毁
					break; //退出循环
				}
			} else {
				flashMoveTo (transform.position); //移动到自己当前位置
			}
		}
	}

	//======================================================
	//	玩家瞬移函数
	//======================================================
	private void flashMoveTo (Vector2 movePosition) {
		transform.position = new Vector3 (movePosition.x, movePosition.y, transform.position.z);
	}

	//======================================================
	//	玩家缓慢落下函数
	//======================================================
	private void slowDown () {
		rigidbody.velocity = new Vector2 (rigidbody.velocity.x, rigidbody.velocity.y); //维持玩家的移动
		if (inputOperation.isInputed ()) { //当玩家进行输入时
			exitFromSlowDown (); //进入离开缓慢落下的处理
		}
	}

	//======================================================
	//	玩家离开缓慢落下处理函数
	//======================================================
	private void exitFromSlowDown () {
		bool isMoveToWall = false; //玩家是否对着墙移动
		if ((isFacingRight && inputOperation.moveInput < 0) || (!isFacingRight && inputOperation.moveInput > 0)) {
			isMoveToWall = true;
		}

		if (!isMoveToWall) { //若玩家没有对着墙移动才处理
			isWannaToExitSlowDown = true; //玩家想要离开缓慢下落
			if (inputOperation.isWannaJump) { //若进行跳跃
				if (isFacingRight) {
					rigidbody.velocity = new Vector2 (rigidbody.velocity.x, Mathf.Sqrt (2.0f * Mathf.Abs (Physics.gravity.y) * jumpHeight));
					direction = 1f;
				} else {
					rigidbody.velocity = new Vector2 (rigidbody.velocity.x, Mathf.Sqrt (2.0f * Mathf.Abs (Physics.gravity.y) * jumpHeight));
					direction = -1f;
				} //根据玩家方向进行跳跃处理
			} else {
				direction = inputOperation.moveInput; //若不跳跃，则离开墙面
			}
			exitSlowDown (); //离开缓慢下落
		}

	}

	//======================================================
	//	玩家离开缓慢落下函数
	//======================================================
	public void exitSlowDown () {
		isCanAirJump = false; //不可以进行跳跃
		rigidbody.gravityScale = initGravity; //恢复重量
		isWannaToExitSlowDown = false; //不想离开墙面
		moveStateTo (State.run); //状态迁移为移动
	}

	//===========================================================================
	//																			|
	//					玩家辅助函数											  |
	//																			|
	//===========================================================================
	//======================================================
	//	玩家转身函数
	//======================================================
	void turnRound () {
		Vector3 selfScale = transform.localScale;
		if (isFacingRight) {
			selfScale.x = Mathf.Abs (selfScale.x);
		} else {
			selfScale.x = -Mathf.Abs (selfScale.x);
		}
		transform.localScale = selfScale;
	}

	//======================================================
	//	高跳函数
	//======================================================
	public void highJump () {
		isGrounded = false;
		rigidbody.velocity = new Vector2 (rigidbody.velocity.x, Mathf.Sqrt (2.0f * Mathf.Abs (Physics.gravity.y) * jumpHeight * highJumpPower));
	}

	//======================================================
	//	检查是否着地函数
	//======================================================
	private void checkIsGrounded () {
		isGrounded = groundCheckPoint.GetComponent<CheckOnLand> ().isOnland; //读取着地检测体的值
		if (isGrounded) {
			isCanAirJump = false;
		}
	}

	//======================================================
	//	玩家错误提示函数
	//======================================================
	private void errorOut () {
		GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<CameraManage> ().camaraShockFor (0.2f);
	}

	//======================================================
	//	玩家死亡函数
	//======================================================
	private void toDie () {
		rigidbody.velocity = Vector2.zero; //移动力清零
		deadTimer.timeFlies (); //死亡倒计时

		if (deadTimer.isTime ()) {
			moveStateTo (State.dead); //死亡
		}
	}

	//======================================================
	//	检测玩家是否存活函数（可调用）
	//======================================================
	public bool isAlive () {
		if (state == State.dead) {
			return false;
		} else {
			return true;
		}
	}

	//======================================================
	//	玩家停止移动函数（可调用）
	//======================================================
	public void moveStop () {
		direction = 0f;
	}

	//======================================================
	//	玩家恢复XPOWER函数（可调用）
	//======================================================
	public void recoveryRushPower (int power) {
		rushPower.addTo (power);
	}

	//======================================================
	//	玩家落地处理函数（可调用）
	//======================================================
	public void landing () {
		if (state == State.slowDown && !isWannaToExitSlowDown) {
			return;
		} //若玩家在缓慢下落状态且不准备离开则跳出
		isCanAirJump = false; //不可以跳跃
		rigidbody.gravityScale = initGravity; //恢复重力
		moveStateTo (State.standby); //变为待机状态
	}

	//======================================================
	//	玩家从属函数（可调用）
	//======================================================
	public void toBeSonWith (Transform parent) {
		transform.SetParent (parent); //将玩家变为指定物体的子对象
	}

	//======================================================
	//	玩家取消从属函数（可调用）
	//======================================================
	public void toBeFree () {
		transform.SetParent (null); //将玩家独立
	}

	//======================================================
	//	玩家获取启示位置函数
	//======================================================
	private GameObject findedStartPoint () {
		GameObject[] startPoints; //所有关卡开始位置
		GameObject truePoint = null; //真正的开始点
		startPoints = GameObject.FindGameObjectsWithTag ("StageMovePoint"); //所有关卡切换检查点
		foreach (GameObject stPoint in startPoints) { //所有关卡切换检查点中循环
			if (startNum == stPoint.GetComponent<StageMovePoint> ().nowStartNum) { //若玩家开始序列=开始点的序列
				truePoint = stPoint.transform.Find ("startPoint").gameObject;
			}
		}
		if (truePoint == null) {
			Debug.Log ("没有找到开始坐标");
		}
		return truePoint;
	}

	//---------------------------------------------------------------------------------------
	//																						|
	//										系统处理函数									  |
	//																						|
	//---------------------------------------------------------------------------------------
	//-----------------------------------------------
	//	系统初始化函数
	//-----------------------------------------------
	void Start () {
		init ();
	}

	//-----------------------------------------------
	//	系统每帧运行函数
	//-----------------------------------------------
	void Update () {
		// checkIsGrounded (); //每帧检测是否着地

		inputOperation = operation (); //获取玩家输入
		process (); // 执行游戏处理

	}

	//-----------------------------------------------
	//	系统进入碰撞时执行函数
	//-----------------------------------------------
	private void OnTriggerEnter2D (Collider2D other) {
		if (state != State.dead && state != State.die) { //非死亡时才进行处理
			// 若碰到陷阱层，则死亡
			if (other.tag == "Trap") {
				moveStateTo (State.die);
				Debug.Log ("PLAYER IS DIE");
			}

			if (other.tag != "Trap" && other.tag != "Other" && state == State.rushing) {
				moveStateTo (State.standby);
			}
		}

	}

	//-----------------------------------------------
	//	系统碰撞停留时执行函数
	//-----------------------------------------------
	private void OnTriggerStay2D (Collider2D other) {
		if (state != State.dead && state != State.die) { //非死亡时才进行处理
			// 若碰到陷阱层，则死亡
			if (other.tag == "Trap") {
				moveStateTo (State.die);
				Debug.Log ("PLAYER IS DIE");
			}

			if (other.tag != "Trap" && other.tag != "Other" && state == State.rushing) {
				moveStateTo (State.standby);
			}
		}
	}

}