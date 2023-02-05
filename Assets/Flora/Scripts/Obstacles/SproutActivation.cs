using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Flora.Scripts.Obstacles {
    [Serializable]
    public class SproutActivation : Activation {
        [Serializable]
        public struct SproutOffset {
            public Vector2Int offset;
            public float delay;
        }

        public List<SproutOffset> sproutOffsets;

        public override IEnumerator Activate(GameManager gameManager, float speedMultiplier, HashSet<Obstacle> blacklist) {
            var worldManager = gameManager.worldManager;

            var origin = new Vector2Int(
                Random.Range(-worldManager.width, worldManager.width),
                Random.Range(-worldManager.height, worldManager.height)
            );

            foreach (var sproutOffset in sproutOffsets) {
                if (sproutOffset.delay > 0) {
                    yield return new WaitForSeconds(sproutOffset.delay * speedMultiplier);
                }

                var sprout = gameManager.worldManager.GetSproutAtLocation(origin + sproutOffset.offset);

                if (blacklist.Contains(sprout)) {
                    continue;
                }

                if (sprout == null) {
                    continue;
                }

                if (sprout.TryActivate(speedMultiplier)) {
                    blacklist.Add(sprout);
                }
            }
        }

        private Vector2Int GetSize() {
            Vector2Int size = Vector2Int.zero;
            foreach (var sproutOffset in sproutOffsets) {
                var offsetX = Mathf.Abs(sproutOffset.offset.x);
                var offsetY = Mathf.Abs(sproutOffset.offset.y);

                if (offsetX > size.x) {
                    size.x = offsetX;
                }

                if (offsetY > size.y) {
                    size.y = offsetY;
                }
            }
            return size;
        }
    }
}