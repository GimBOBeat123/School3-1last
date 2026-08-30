using System.Collections.Generic;
using Domain.Entities;
using UnityEngine;

namespace Infrastructure
{
    /// <summary>
    /// 인프라 무기 데이터베이스
    /// 모든 무기 정보 관리 및 생성
    /// </summary>
    public class WeaponDatabase : MonoBehaviour
    {
        /// <summary>
        /// 무기 정보 딕셔너리
        /// 무기 5개 정보 저장
        /// </summary>
        public static readonly Dictionary<string, WeaponInfo> WeaponDatabase_Dict = new()
        {
            {
                "iron_sword",
                new WeaponInfo(
                    id: "iron_sword",
                    name: "철 검",
                    attackPower: 5,
                    criticalChance: 0.05f,
                    rarity: 0,
                    description: "기본 철로 만든 검",
                    dropRate: 0.30f
                )
            },
            {
                "steel_sword",
                new WeaponInfo(
                    id: "steel_sword",
                    name: "강철 검",
                    attackPower: 10,
                    criticalChance: 0.10f,
                    rarity: 1,
                    description: "강철로 강화된 검",
                    dropRate: 0.25f
                )
            },
            {
                "gold_sword",
                new WeaponInfo(
                    id: "gold_sword",
                    name: "황금 검",
                    attackPower: 15,
                    criticalChance: 0.15f,
                    rarity: 2,
                    description: "귀한 황금으로 만든 검",
                    dropRate: 0.20f
                )
            },
            {
                "diamond_sword",
                new WeaponInfo(
                    id: "diamond_sword",
                    name: "다이아몬드 검",
                    attackPower: 20,
                    criticalChance: 0.20f,
                    rarity: 3,
                    description: "전설의 다이아몬드 검",
                    dropRate: 0.15f
                )
            },
            {
                "dragon_sword",
                new WeaponInfo(
                    id: "dragon_sword",
                    name: "드래곤 검",
                    attackPower: 25,
                    criticalChance: 0.25f,
                    rarity: 3,
                    description: "전설의 용을 사냥한 검",
                    dropRate: 0.10f
                )
            }
        };

        /// <summary>
        /// 무기 정보 클래스
        /// 무기의 모든 데이터 저장
        /// </summary>
        public class WeaponInfo
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public int AttackPower { get; set; }
            public float CriticalChance { get; set; }
            public int Rarity { get; set; }
            public string Description { get; set; }
            public float DropRate { get; set; }

            /// <summary>
            /// 무기 정보 생성
            /// </summary>
            public WeaponInfo(
                string id,
                string name,
                int attackPower,
                float criticalChance,
                int rarity,
                string description,
                float dropRate)
            {
                Id = id;
                Name = name;
                AttackPower = attackPower;
                CriticalChance = criticalChance;
                Rarity = rarity;
                Description = description;
                DropRate = dropRate;
            }
        }

        /// <summary>
        /// 무기 생성
        /// ID로 무기 정보를 찾아 무기 객체 생성
        /// </summary>
        public static Weapon CreateWeapon(string weaponId)
        {
            if (!WeaponDatabase_Dict.TryGetValue(weaponId, out var info))
            {
                Debug.LogWarning($"무기 없음: {weaponId}");
                return null;
            }

            var weapon = new Weapon(
                id: info.Id,
                name: info.Name,
                attack: info.AttackPower,
                criticalChance: info.CriticalChance,
                rarity: info.Rarity,
                desc: info.Description
            );

            return weapon;
        }

        /// <summary>
        /// 무기 정보 조회
        /// 무기 정보 반환
        /// </summary>
        public static WeaponInfo GetWeaponInfo(string weaponId)
        {
            return WeaponDatabase_Dict.TryGetValue(weaponId, out var info) ? info : null;
        }

        /// <summary>
        /// 모든 무기 ID 반환
        /// </summary>
        public static string[] GetAllWeaponIds()
        {
            return new string[] { "iron_sword", "steel_sword", "gold_sword", "diamond_sword", "dragon_sword" };
        }
    }
}