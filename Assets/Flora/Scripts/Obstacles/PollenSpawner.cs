using System;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Flora.Scripts.Obstacles {
    public class PollenSpawner : MonoBehaviour {
        public WorldManager worldManager;
        public float cooldown = 5;
        public Pollen pollen;
        private float _cooldown;

        private void Update() {
            _cooldown -= Time.deltaTime;
            if (_cooldown < 0) {
                Spawn();
                _cooldown = cooldown;
            }
        }

        private void Spawn() {
            var x = Random.Range(-worldManager.width, worldManager.width);
            var y = Random.Range(-worldManager.height, worldManager.height);
            Instantiate(pollen, new Vector3(x, 0, y), Quaternion.identity);
        }
    }
}