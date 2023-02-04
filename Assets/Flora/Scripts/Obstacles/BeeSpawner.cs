using System.Collections;
namespace Flora.Scripts.Obstacles {
    public class BeeSpawner : Obstacle {

        public override ObstacleType ObstacleType => ObstacleType.Bee;

        public override IEnumerator Activate(float speedMultiplier) {
            yield break;
        }
    }
}