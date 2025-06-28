using UnityEngine;
using System;
using GK.Platform;

namespace GK.Managers
{
    /// <summary>
    /// 输入管理器
    /// 统一处理触摸、鼠标、键盘等不同平台的输入方式
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [Header("输入设置")]
        [SerializeField] private bool enableTouch = true;
        [SerializeField] private bool enableMouse = true;
        [SerializeField] private bool enableKeyboard = true;
        [SerializeField] private float touchSensitivity = 1.0f;
        [SerializeField] private float swipeThreshold = 50f;

        private static InputManager _instance;
        private Vector2 _lastInputPosition;
        private bool _isInputDown;
        private float _inputStartTime;

        // 事件定义
        public static event Action<Vector2> OnInputDown;
        public static event Action<Vector2> OnInputUp;
        public static event Action<Vector2> OnInputDrag;
        public static event Action<Vector2> OnTap;
        public static event Action<Vector2, Vector2> OnSwipe;

        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InputManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("InputManager");
                        _instance = go.AddComponent<InputManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 当前输入位置
        /// </summary>
        public Vector2 CurrentInputPosition { get; private set; }

        /// <summary>
        /// 是否正在输入
        /// </summary>
        public bool IsInputActive => _isInputDown;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            HandleInput();
        }

        /// <summary>
        /// 处理输入
        /// </summary>
        private void HandleInput()
        {
            if (PlatformDetector.IsMobile && enableTouch)
            {
                HandleTouchInput();
            }
            else if ((PlatformDetector.IsDesktop || PlatformDetector.IsWeb) && enableMouse)
            {
                HandleMouseInput();
            }

            if (enableKeyboard)
            {
                HandleKeyboardInput();
            }
        }

        /// <summary>
        /// 处理触摸输入
        /// </summary>
        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        HandleInputDown(touchPosition);
                        break;
                    case TouchPhase.Moved:
                        HandleInputDrag(touchPosition);
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        HandleInputUp(touchPosition);
                        break;
                }
            }
        }

        /// <summary>
        /// 处理鼠标输入
        /// </summary>
        private void HandleMouseInput()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                HandleInputDown(mousePosition);
            }
            else if (Input.GetMouseButton(0) && _isInputDown)
            {
                HandleInputDrag(mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                HandleInputUp(mousePosition);
            }
        }

        /// <summary>
        /// 处理键盘输入
        /// </summary>
        private void HandleKeyboardInput()
        {
            // 方向键输入
            Vector2 direction = Vector2.zero;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                direction.y = 1;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                direction.y = -1;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                direction.x = -1;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                direction.x = 1;

            if (direction != Vector2.zero)
            {
                // 可以添加方向键事件处理
            }
        }

        /// <summary>
        /// 处理输入开始
        /// </summary>
        private void HandleInputDown(Vector2 position)
        {
            _isInputDown = true;
            _lastInputPosition = position;
            CurrentInputPosition = position;
            _inputStartTime = Time.time;

            OnInputDown?.Invoke(position);
        }

        /// <summary>
        /// 处理输入拖拽
        /// </summary>
        private void HandleInputDrag(Vector2 position)
        {
            if (!_isInputDown) return;

            CurrentInputPosition = position;
            OnInputDrag?.Invoke(position);
        }

        /// <summary>
        /// 处理输入结束
        /// </summary>
        private void HandleInputUp(Vector2 position)
        {
            if (!_isInputDown) return;

            _isInputDown = false;
            CurrentInputPosition = position;
            float inputDuration = Time.time - _inputStartTime;
            Vector2 deltaPosition = position - _lastInputPosition;

            OnInputUp?.Invoke(position);

            // 判断是点击还是滑动
            if (deltaPosition.magnitude < swipeThreshold && inputDuration < 0.5f)
            {
                OnTap?.Invoke(position);
            }
            else if (deltaPosition.magnitude >= swipeThreshold)
            {
                OnSwipe?.Invoke(_lastInputPosition, position);
            }
        }

        /// <summary>
        /// 获取滑动方向
        /// </summary>
        public static Vector2 GetSwipeDirection(Vector2 startPos, Vector2 endPos)
        {
            return (endPos - startPos).normalized;
        }

        /// <summary>
        /// 判断是否为水平滑动
        /// </summary>
        public static bool IsHorizontalSwipe(Vector2 startPos, Vector2 endPos)
        {
            Vector2 direction = endPos - startPos;
            return Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
        }

        /// <summary>
        /// 判断是否为垂直滑动
        /// </summary>
        public static bool IsVerticalSwipe(Vector2 startPos, Vector2 endPos)
        {
            Vector2 direction = endPos - startPos;
            return Mathf.Abs(direction.y) > Mathf.Abs(direction.x);
        }

        /// <summary>
        /// 设置触摸灵敏度
        /// </summary>
        public void SetTouchSensitivity(float sensitivity)
        {
            touchSensitivity = Mathf.Clamp01(sensitivity);
        }

        /// <summary>
        /// 设置滑动阈值
        /// </summary>
        public void SetSwipeThreshold(float threshold)
        {
            swipeThreshold = Mathf.Max(0, threshold);
        }

        /// <summary>
        /// 启用/禁用输入类型
        /// </summary>
        public void SetInputEnabled(bool touch, bool mouse, bool keyboard)
        {
            enableTouch = touch;
            enableMouse = mouse;
            enableKeyboard = keyboard;
        }

        /// <summary>
        /// 获取输入信息
        /// </summary>
        public string GetInputInfo()
        {
            return $"Touch: {enableTouch}, Mouse: {enableMouse}, Keyboard: {enableKeyboard}, " +
                   $"Current Position: {CurrentInputPosition}, Is Active: {IsInputActive}";
        }
    }
}
