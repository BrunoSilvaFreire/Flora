using System;
using System.Collections.Generic;
using Flora.Scripts.Obstacles;
using UnityEngine;
namespace Flora.Scripts {
    public class GameManager : MonoBehaviour {

        public AnimationCurve speedOverTime;
        public ObstacleEvent[] events;

        private Dictionary<ObstacleType, List<Obstacle>> _obstaclesByType;

        private float time;

        private void Cache() {
            _obstaclesByType = new Dictionary<ObstacleType, List<Obstacle>>();

            foreach (var obstacle in FindObjectsOfType<Obstacle>()) {
                var obstacleType = obstacle.ObstacleType;
                if (!_obstaclesByType.TryGetValue(obstacleType, out var array)) {
                    array = new List<Obstacle>();
                }

                array.Add(obstacle);
            }
        }

        private void Restart() {
            time = 0;
        }

        private void Update() {
            time += Time.deltaTime;
        }

        public List<Obstacle> GetAllObstaclesOfType(ObstacleType obstacleType) {
            return _obstaclesByType[obstacleType];
        }
    }
}