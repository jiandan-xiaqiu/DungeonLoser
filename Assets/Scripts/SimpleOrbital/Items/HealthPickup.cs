using UnityEngine;

namespace SimpleOrbital
{
    /// <summary>
    /// 回血道具拾取组件
    /// </summary>
    public class HealthPickup : MonoBehaviour
    {
        public float healthAmount = 20f;
        public float pickupRange = 1f;
        public float attractSpeed = 5f;
        
        private Transform playerTransform;
        private bool isAttracting = false;
        
        private void Update()
        {
            if (playerTransform == null)
            {
                FindPlayer();
                return;
            }
            
            // 检查是否在拾取范围内
            float distance = Vector3.Distance(transform.position, playerTransform.position);
            
            if (distance <= pickupRange)
            {
                isAttracting = true;
            }
            
            if (isAttracting)
            {
                // 吸引到玩家
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    attractSpeed * Time.deltaTime
                );
                
                // 到达玩家
                if (Vector3.Distance(transform.position, playerTransform.position) < 0.1f)
                {
                    Pickup();
                }
            }
        }
        
        /// <summary>
        /// 寻找玩家
        /// </summary>
        private void FindPlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
        
        /// <summary>
        /// 拾取回血道具
        /// </summary>
        private void Pickup()
        {
            if (CharacterManager.Instance != null)
            {
                CharacterManager.Instance.Heal(healthAmount);
            }
            
            Destroy(gameObject);
        }
    }
}