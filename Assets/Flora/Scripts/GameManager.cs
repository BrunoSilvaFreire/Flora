using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flora.Scripts.Obstacles;
using Flora.Scripts.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Flora.Scripts {
    public class GameManager : MonoBehaviour {
        public static GameManager Instance {
            get;
            private set;
        }

        public float endDuration = 10;
        public AnimationCurve speedOverTime;
        public AnimationCurve activationCooldownCurve;
        public ObstacleEvent[] events;
        public bool step;
        public WorldManager worldManager;
        public float initialTimeGrace = 10;
        public EndScreen endScreen;

        private Dictionary<ObstacleType, List<Obstacle>> _obstaclesByType;
        private float time;
        private float activationCooldown;
        private bool ending;

        private void Start() {
            Instance = this;

            if (!worldManager.Generated) {
                worldManager.Generate();
            }

            Cache();

            activationCooldown = initialTimeGrace;
            time = 0;
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

        public void Restart() {
            activationCooldown = initialTimeGrace;
            time = 0;

            foreach (var pulla in FindObjectsOfType<Pulla>()) {
                pulla.Revive();
            }

            endScreen.Hide();
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

        public void NotifyDeath(Pulla NewPulla) {
            if (ending) {
                return;
            }
            if (FindObjectsOfType<Pulla>().All(p => p.Dead)) {
                EndGame();
            }
        }
        private void EndGame() {
            StartCoroutine(EndGameRoutine());
        }
        
        private IEnumerator EndGameRoutine() {
            ending = true;
            step = false;
            endScreen.Show();
            yield return new WaitForSeconds(endDuration);
            Restart();
            step = true;
            ending = false;
        }
    }
}