using System.IO;
using System.Text;
using Domain.Entities;
using Domain.Interfaces;
using UnityEngine;

namespace Infrastructure
{
    /// <summary>
    /// 인프라 인벤토리 저장
    /// 인벤토리와 장비를 JSON으로 저장 및 로드
    /// </summary>
    public class InventoryRepository : IInventoryRepository
    {
        private readonly string inventoryPath;
        private readonly string equipmentPath;

        /// <summary>
        /// 인벤토리 저장소 생성
        /// 저장 경로 설정
        /// </summary>
        public InventoryRepository()
        {
            inventoryPath = UnityEngine.Application.persistentDataPath + "/inventory.json";
            equipmentPath = UnityEngine.Application.persistentDataPath + "/equipment.json";
            Debug.Log($"[InventoryRepository] 경로 설정:");
            Debug.Log($"  인벤토리: {inventoryPath}");
            Debug.Log($"  장비: {equipmentPath}");
        }

        /// <summary>
        /// 인벤토리를 JSON으로 저장
        /// 무기와 일반 아이템 분리 저장
        /// </summary>
        public void SaveInventory(Inventory inventory)
        {
            try
            {
                var inventoryData = new InventoryData();

                Debug.Log($"[InventoryRepository] 인벤토리 저장 - 총 슬롯: {inventory.Slots.Count}");

                // 인벤토리 슬롯 저장
                for (int i = 0; i < inventory.Slots.Count; i++)
                {
                    var slot = inventory.Slots[i];
                    if (!slot.IsEmpty && slot.Item.Value != null)
                    {
                        var item = slot.Item.Value;
                        Debug.Log($"[InventoryRepository] 슬롯 {i}: {item.ItemName} (비어있음: {slot.IsEmpty})");

                        // 무기면 무기데이터로 저장
                        if (item is Weapon weapon)
                        {
                            inventoryData.Weapons.Add(WeaponData.FromWeapon(weapon));
                            Debug.Log($"[InventoryRepository] 무기 추가됨: {weapon.ItemName} (공격력: {weapon.AttackPower})");
                        }
                        else
                        {
                            inventoryData.Items.Add(ItemData.FromItem(item));
                            Debug.Log($"[InventoryRepository] 아이템 추가됨: {item.ItemName}");
                        }
                    }
                }

                Debug.Log($"[InventoryRepository] 저장할 무기: {inventoryData.Weapons.Count}");
                Debug.Log($"[InventoryRepository] 저장할 아이템: {inventoryData.Items.Count}");

                string json = JsonUtility.ToJson(inventoryData, true);
                Debug.Log($"[InventoryRepository] JSON: {json}");

                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                File.WriteAllBytes(inventoryPath, jsonBytes);

                Debug.Log($"[InventoryRepository] 저장 성공 {inventoryPath}");
                Debug.Log($"[InventoryRepository] 파일 크기: {new FileInfo(inventoryPath).Length} 바이트");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[InventoryRepository] 저장 오류: {e.Message}");
                Debug.LogError($"[InventoryRepository] 스택: {e.StackTrace}");
            }
        }

        /// <summary>
        /// JSON 파일에서 인벤토리 로드
        /// 파일이 없으면 빈 인벤토리 반환
        /// </summary>
        public Inventory LoadInventory()
        {
            var inventory = new Inventory();

            try
            {
                Debug.Log($"[InventoryRepository] 인벤토리 로드 - 경로: {inventoryPath}");
                Debug.Log($"[InventoryRepository] 파일 존재: {File.Exists(inventoryPath)}");

                if (!File.Exists(inventoryPath))
                {
                    Debug.Log("[InventoryRepository] 저장 파일 없음");
                    return inventory;
                }

                byte[] jsonBytes = File.ReadAllBytes(inventoryPath);
                string json = Encoding.UTF8.GetString(jsonBytes);
                Debug.Log($"[InventoryRepository] 로드한 JSON: {json}");

                var inventoryData = JsonUtility.FromJson<InventoryData>(json);

                Debug.Log($"[InventoryRepository] 파싱된 무기: {inventoryData.Weapons.Count}");
                Debug.Log($"[InventoryRepository] 파싱된 아이템: {inventoryData.Items.Count}");

                // 무기 로드
                foreach (var weaponData in inventoryData.Weapons)
                {
                    var weapon = weaponData.ToWeapon();
                    inventory.AddItem(weapon);
                    Debug.Log($"[InventoryRepository] 무기 로드됨: {weapon.ItemName} (공격력: {weapon.AttackPower})");
                }

                // 아이템 로드
                foreach (var itemData in inventoryData.Items)
                {
                    if (itemData != null)
                    {
                        var item = itemData.ToItem();
                        inventory.AddItem(item);
                        Debug.Log($"[InventoryRepository] 아이템 로드됨: {item.ItemName}");
                    }
                }

                Debug.Log($"[InventoryRepository] 로드 성공 - 총 아이템: {inventory.ItemCount}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[InventoryRepository] 로드 오류: {e.Message}");
                Debug.LogError($"[InventoryRepository] 스택: {e.StackTrace}");
            }

            return inventory;
        }

        /// <summary>
        /// 장비를 JSON으로 저장
        /// </summary>
        public void SaveEquipment(Equipment equipment)
        {
            try
            {
                Debug.Log("[InventoryRepository] 장비 저장");
                var equipmentData = EquipmentData.FromEquipment(equipment);
                string json = JsonUtility.ToJson(equipmentData, true);
                Debug.Log($"[InventoryRepository] 장비 JSON: {json}");

                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                File.WriteAllBytes(equipmentPath, jsonBytes);

                Debug.Log("[InventoryRepository] 장비 저장 성공");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[InventoryRepository] 장비 저장 오류: {e.Message}");
            }
        }

        /// <summary>
        /// JSON 파일에서 장비 로드
        /// 파일이 없으면 빈 장비 반환
        /// </summary>
        public Equipment LoadEquipment()
        {
            var equipment = new Equipment();

            try
            {
                Debug.Log($"[InventoryRepository] 장비 로드 - 파일 존재: {File.Exists(equipmentPath)}");

                if (!File.Exists(equipmentPath))
                {
                    Debug.Log("[InventoryRepository] 장비 저장 파일 없음");
                    return equipment;
                }

                byte[] jsonBytes = File.ReadAllBytes(equipmentPath);
                string json = Encoding.UTF8.GetString(jsonBytes);
                Debug.Log($"[InventoryRepository] 장비 JSON: {json}");

                var equipmentData = JsonUtility.FromJson<EquipmentData>(json);

                equipment = equipmentData.ToEquipment();
                Debug.Log("[InventoryRepository] 장비 로드 성공");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[InventoryRepository] 장비 로드 오류: {e.Message}");
            }

            return equipment;
        }
    }

    /// <summary>
    /// 인벤토리 데이터 직렬화
    /// 무기와 아이템 목록 포함
    /// </summary>
    [System.Serializable]
    public class InventoryData
    {
        public System.Collections.Generic.List<WeaponData> Weapons = new();
        public System.Collections.Generic.List<ItemData> Items = new();
    }

    /// <summary>
    /// 아이템 데이터 직렬화
    /// 일반 아이템 정보 저장
    /// </summary>
    [System.Serializable]
    public class ItemData
    {
        public string ItemId;
        public string ItemName;

        public static ItemData FromItem(Item item)
        {
            if (item == null) return null;
            return new ItemData { ItemId = item.ItemId, ItemName = item.ItemName };
        }

        public Item ToItem()
        {
            return new Item(ItemId, ItemName);
        }
    }

    /// <summary>
    /// 무기 데이터 직렬화
    /// 무기의 모든 정보 저장
    /// </summary>
    [System.Serializable]
    public class WeaponData
    {
        public string Id;
        public string Name;
        public int AttackPower;
        public float CriticalChance;
        public int Rarity;
        public string Description;

        public static WeaponData FromWeapon(Weapon weapon)
        {
            if (weapon == null) return null;

            return new WeaponData
            {
                Id = weapon.ItemId,
                Name = weapon.ItemName,
                AttackPower = weapon.AttackPower,
                CriticalChance = weapon.CriticalChance,
                Rarity = weapon.Rarity,
                Description = weapon.Description
            };
        }

        public Weapon ToWeapon()
        {
            return new Weapon(
                id: Id,
                name: Name,
                attack: AttackPower,
                criticalChance: CriticalChance,
                rarity: Rarity,
                desc: Description
            );
        }
    }

    /// <summary>
    /// 장비 데이터 직렬화
    /// 장착 무기 정보 저장
    /// </summary>
    [System.Serializable]
    public class EquipmentData
    {
        public WeaponData EquippedWeapon;

        public static EquipmentData FromEquipment(Equipment equipment)
        {
            if (equipment == null) return null;

            return new EquipmentData
            {
                EquippedWeapon = WeaponData.FromWeapon(equipment.EquippedWeapon.Value)
            };
        }

        public Equipment ToEquipment()
        {
            var equipment = new Equipment();
            if (EquippedWeapon != null)
            {
                equipment.EquipWeapon(EquippedWeapon.ToWeapon());
            }
            return equipment;
        }
    }
}