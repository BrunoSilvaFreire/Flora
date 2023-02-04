using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Flora.Scripts.Obstacles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Flora.Scripts {
    public class GameManager : MonoBehaviour {

        public AnimationCurve speedOverTime;
        public AnimationCurve activationCooldownCurve;
        public ObstacleEvent[] events;
        public bool step;
        public WorldManager worldManager;
        private Dictionary<ObstacleType, List<Obstacle>> _obstaclesByType;

        private float time;
        private float activationCooldown;

        private void Start() {
            if (!worldManager.Generated) {
                worldManager.Generate();
            }
            Cache();
        }

        private void Cache() {
            _obstaclesByType = new Dictionary<ObstacleType, List<Obstacle>>();

            foreach (var obstacle in FindObjectsOfType<Obstacle>()) {
                var obstacleType = obstacle.ObstacleType;
                if (!_obstaclesByType.TryGetValue(obstacleType, out var array)) {
                    array = new List<Obstacle>();
                    _obstaclesByType[obstacleType] = array;
                }

                array.Add(obstacle);
            }
        }

        private void Restart() {
            time = 0;
        }

        private void Update() {
            if (!step) {
                return;
            }

            Step();
        }

        private void Step() {
            time += Time.deltaTime;
            if (activationCooldown > 0) {
                activationCooldown -= Time.deltaTime;
            } else {
                ActivateAnEvent();
                ResetCooldown();
            }
        }

        private void ResetCooldown() {
            activationCooldown = activationCooldownCurve.Evaluate(time);
        }

        private void ActivateAnEvent() {
            var anEvent = SelectEvent();
            var speedMultiplier = 1 / speedOverTime.Evaluate(time);

            Debug.Log($"Activating event: {anEvent.name} @ time: {time}, speedMultiplier: {speedMultiplier}");
            StartCoroutine(anEvent.Activate(this, speedMultiplier));
        }

        private ObstacleEvent SelectEvent() {
            var eligible = events.Where(obstacleEvent => time >= obstacleEvent.minTimeToActivate && time <= obstacleEvent.maxTimeToActivate).ToList();

            var electedIndex = Random.Range(0, eligible.Count);
            return eligible[electedIndex];
        }

        public List<Obstacle> GetAllObstaclesOfType(ObstacleType obstacleType) {
            return _obstaclesByType[obstacleType];
        }
    }
}