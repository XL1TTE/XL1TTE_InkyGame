
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Project.Internal.ActorSystem;
using TMPro;
using UnityEngine;

namespace Project.Internal.Utilities
{
    public class FloatingDamageManager : MonoBehaviour
    {
        [SerializeField] protected FloatingDamage DamageTextPrefab;
        [SerializeField] protected Transform ObjectsPoolTransform;


        private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
        private const float positionCheckRadius = 0.15f;


        public static FloatingDamageManager instance;
        public IEnumerator Init()
        {
            if (instance == null)
            {
                instance = this;
            }
            yield return null;
        }

        protected Queue<FloatingDamage> FreeObjects = new Queue<FloatingDamage>();

        public void OnEnemyDamageTaken(Enemy enemy, float damage)
        {
            FloatingDamage floating_damage = null;
            if (FreeObjects.Count == 0)
            {
                floating_damage = Instantiate(DamageTextPrefab, ObjectsPoolTransform);
            }
            else
            {
                floating_damage = FreeObjects.Dequeue();
                floating_damage.gameObject.SetActive(true);
            }

            floating_damage.DamageText.text = damage.ToString();
            var enemy_position = enemy.gameObject.transform.position;
            StartCoroutine(FloatingAndFading(enemy_position, floating_damage));
        }

        private void ReturnObjectInPool(FloatingDamage floating_damage_object)
        {
            floating_damage_object.gameObject.SetActive(false);
            FreeObjects.Enqueue(floating_damage_object);
        }
        private IEnumerator FloatingAndFading(Vector3 StartPosition, FloatingDamage floating_text)
        {
            float elapsedTime = 0f;

            Vector3 newPosition = GetRandomPosition(StartPosition, floating_text.StartPositionShift);
            if (newPosition == Vector3.zero)
            {
                ReturnObjectInPool(floating_text);
                yield break;
            }

            Vector3 targetPosition = newPosition + floating_text.DisappearShift;

            while (elapsedTime < floating_text.fadeDuration)
            {
                float t = elapsedTime / floating_text.fadeDuration;
                floating_text.gameObject.transform.position = Vector3.Lerp(newPosition, targetPosition, t);
                floating_text.DamageText.color = new Color(floating_text.DamageText.color.r, floating_text.DamageText.color.g, floating_text.DamageText.color.b, 1 - t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            ReturnObjectInPool(floating_text);
            occupiedPositions.Remove(newPosition);
        }

        private Vector3 GetRandomPosition(Vector3 basePosition, Vector3 offset)
        {
            Vector3 randomShift;
            int attempts = 0;

            do
            {
                randomShift = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.5f, 0.5f), 0);
                Vector3 candidatePosition = basePosition + offset + randomShift;

                // Проверяем, занята ли позиция
                if (!IsPositionOccupied(candidatePosition))
                {
                    occupiedPositions.Add(candidatePosition);
                    return candidatePosition;
                }

                attempts++;

            } while (attempts < 10);

            return Vector3.zero;
        }

        private bool IsPositionOccupied(Vector3 position)
        {
            foreach (var occupiedPosition in occupiedPositions)
            {
                if (Vector3.Distance(occupiedPosition, position) < positionCheckRadius)
                {
                    return true; // Позиция занята
                }
            }
            return false; // Позиция свободна
        }
    }
}
