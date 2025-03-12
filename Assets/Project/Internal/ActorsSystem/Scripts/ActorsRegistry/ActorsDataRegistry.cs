using System.Collections;
using System.Collections.Generic;
using Project.Internal.SkillsSystem;


namespace Project.Internal.ActorSystem
{
    public static class ActorsDataRegistry
    {
        public static List<HeroData> Heroes = new List<HeroData>{
            new HeroData{
                ActorID = "Warrior",
                ActorName = "Victor",
                Stats = new HeroStats{
                    Health = 250.0f,
                    PhysicalDamage = 10.0f,
                    Attributes = new ActorAttributes{
                        Strenght = 15.0f,
                        Dexterity = 10.0f,
                        Intelligence = 5.0f
                    }
                },
                Skills = new List<BaseSkill>{
                    SkillsRegistry.GetSkillDataByID("FistPunch")
                }
            },
            new HeroData{
                ActorID = "Mage",
                ActorName = "Elizabeth",
                Stats = new HeroStats{
                    Health = 100.0f,
                    PhysicalDamage = 1.0f,
                    Attributes = new ActorAttributes{
                        Strenght = 5.0f,
                        Dexterity = 5.0f,
                        Intelligence = 15.0f
                    }
                }
            },
            new HeroData{
                ActorID = "Ranger",
                ActorName = "Robin",
                Stats = new HeroStats{
                    Health = 150.0f,
                    PhysicalDamage = 10.0f,
                    Attributes = new ActorAttributes{
                        Strenght = 7.5f,
                        Dexterity = 15.0f,
                        Intelligence = 7.5f
                    }
                }
            },
            new HeroData{
                ActorID = "Rogue",
                ActorName = "Kate",
                Stats = new HeroStats{
                    Health = 100.0f,
                    PhysicalDamage = 25.0f,
                    Attributes = new ActorAttributes{
                        Strenght = 10.0f,
                        Dexterity = 25.0f,
                        Intelligence = 10.0f
                    }
                }
            }
        };

        public static List<EnemyData> Enemies = new List<EnemyData>{
            new EnemyData{
                ActorID = "Witch",
                ActorName = "Witch",
                Stats = new EnemyStats{
                    Health = 250.0f,
                    Attributes = new ActorAttributes{
                        Strenght = 5,
                        Intelligence = 15,
                        Dexterity = 5
                    },
                    PhysicalDamage = 0f
                }
            },
            new EnemyData{
                ActorID = "Wolf",
                ActorName = "Wolf",
                Stats = new EnemyStats{
                    Health = 150,
                    Attributes = new ActorAttributes{
                        Strenght = 10,
                        Intelligence = 5,
                        Dexterity = 15
                    },
                    PhysicalDamage = 2f
                }
            }
        };


        public static HeroData GetHeroDataByID(string ID)
        {
            return Heroes.Find(hero => hero.ActorID == ID).Clone<HeroData>();
        }

        public static EnemyData GetEnemyDataByID(string ID)
        {
            return Enemies.Find(e => e.ActorID == ID).Clone<EnemyData>();
        }

    }
}
