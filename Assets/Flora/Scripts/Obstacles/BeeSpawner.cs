using System.Collections;
namespace Flora.Scripts.Obstacles {
    public class BeeSpawner : Obstacle {

        public override ObstacleType ObstacleType => ObstacleType.Bee;

        public override IEnumerator Act(float speedMultiplier) {
            yield break;
        }
    }
}