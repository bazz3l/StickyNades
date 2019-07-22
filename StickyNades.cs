using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Sticky Nades", "Bazz3l", "1.0.0")]
    [Description("Sticks granades to players")]
    class StickyNades : RustPlugin
    {
        #region Oxide
        void OnExplosiveThrown(BasePlayer player, BaseEntity entity, ThrownWeapon item)
        {
            if (entity == null) return;
            if (entity.ShortPrefabName != "grenade.f1.deployed") return;

            RaycastHit hit;
            var raycast        = Physics.Raycast(player.eyes.HeadRay(), out hit, 50f);
            BaseEntity rentity = raycast ? hit.GetEntity() : null;
            if (rentity != null && rentity is BasePlayer)
            {
                entity?.Kill();

                BaseEntity nade = GameManager.server.CreateEntity("assets/prefabs/weapons/f1 grenade/grenade.f1.deployed.prefab", rentity.transform.position);
                nade.Spawn();
                nade.SetParent(rentity);
                nade.transform.localPosition = new Vector3(0.2f, 1f, 0f);

                var rb = nade.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.useGravity = false;
            }
        }
        #endregion
    }
}
