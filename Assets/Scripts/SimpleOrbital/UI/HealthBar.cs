using UnityEngine;
using UnityEngine.UI;

namespace SimpleOrbital.UI
{
    /// <summary>
    /// 血条组件，显示怪物的生命值
    /// </summary>
    public class HealthBar : MonoBehaviour
    {
        [Header("血条设置")]
        [SerializeField] private Slider healthSlider; // 血条滑块
        [SerializeField] private float followSpeed = 100f; // 跟随速度
        [SerializeField] private Vector3 offset = new Vector3(0f, 2f, 0f); // 偏移量
        
        private Transform targetTransform; // 目标 transform
        private Camera mainCamera;
        
        #region 初始化
        private void Start()
        {
            // 尝试获取相机
            GetCamera();
        }
        
        /// <summary>
        /// 获取相机
        /// </summary>
        private void GetCamera()
        {
            // 1. 首先尝试使用场景中的主相机
            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            
            // 2. 如果没有主相机，尝试查找带有特定标签的相机
            if (mainCamera == null)
            {
                GameObject cameraObj = GameObject.FindWithTag("MainCamera");
                if (cameraObj != null)
                {
                    mainCamera = cameraObj.GetComponent<Camera>();
                }
            }
            
            // 3. 如果还是没有找到，尝试查找所有相机并使用第一个
            if (mainCamera == null)
            {
                Camera[] cameras = FindObjectsOfType<Camera>();
                if (cameras.Length > 0)
                {
                    mainCamera = cameras[0];
                }
            }
        }
        #endregion
        
        #region 更新
        private void Update()
        {
            if (targetTransform == null)
                return;
            
            // 确保相机存在
            if (mainCamera == null)
            {
                GetCamera();
            }
            
            // 跟随目标
            FollowTarget();
        }
        #endregion
        
        #region 血条控制
        /// <summary>
        /// 设置血条目标
        /// </summary>
        /// <param name="target">目标 transform</param>
        public void SetTarget(Transform target)
        {
            targetTransform = target;
        }
        
        /// <summary>
        /// 设置生命值
        /// </summary>
        /// <param name="current">当前生命值</param>
        /// <param name="max">最大生命值</param>
        public void SetHealth(float current, float max)
        {
            // 更新血条值
            float healthPercentage = current / max;
            healthSlider.value = healthPercentage;
        }
        
        /// <summary>
        /// 跟随目标
        /// </summary>
        private void FollowTarget()
        {
            if (targetTransform == null || mainCamera == null)
                return;
            
            // 将目标位置转换为屏幕坐标
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetTransform.position + offset);
            
            // 平滑跟随
            transform.position = Vector3.Lerp(transform.position, screenPosition, followSpeed * Time.deltaTime);
        }
        
        /// <summary>
        /// 显示血条
        /// </summary>
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        /// <summary>
        /// 隐藏血条
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
