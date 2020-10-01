using UnityEngine;
using Rust;
using VLB;

namespace Oxide.Plugins
{
    [Info("Sticky Nades", "Bazz3l", "1.0.6")]
    [Description("Ability to throw grenades that stick to other players.")]
    public class StickyNades : RustPlugin
    {
        #region Fields
        
        private const string PermUse = "stickynades.use";
        
        #endregion

        #region Oxide
        
        private void OnServerInitialized()
        {
            permission.RegisterPermission(PermUse, this);
        }

        private void OnExplosiveThrown(BasePlayer player, BaseEntity entity, ThrownWeapon item)
        {
            if (!(entity.ShortPrefabName.Contains("f1.deployed") || entity.ShortPrefabName.Contains("beancan.deployed")))
            {
                return;
            }

            if (!permission.UserHasPermission(player.UserIDString, PermUse))
            {
                return;
            }

            entity.gameObject.GetOrAddComponent<StickyComponent>().player = player;
        }
        
        #endregion

        #region Component
        
        private class StickyComponent : MonoBehaviour
        {
            private BaseEntity _entity;
            public BasePlayer player;

            private void Awake()
            {
                _entity = GetComponent<BaseEntity>();
                if (_entity == null)
                {
                    Destroy(this);
                    return;
                }

                gameObject.layer = (int) Layer.Reserved1;
                
                SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
                sphere.isTrigger = true;
                sphere.radius = 0.1f;
            }

            private void OnCollisionEnter(Collision collision)
            {
                BasePlayer target = collision.gameObject.GetComponent<BasePlayer>();
                if (target == null || target == player)
                {
                    return;
                }

                _entity.transform.localPosition = new Vector3(0f, 1f, 0f);
                _entity.SetParent(target);

                Destroy(_entity.GetComponent<Rigidbody>());
                Destroy(this);
            }
        }
        
        #endregion
    }
}