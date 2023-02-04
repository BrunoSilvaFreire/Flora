using System.Collections;
namespace Flora.Scripts.Obstacles {


    public class LogSpawner : Obstacle {

        public override ObstacleType ObstacleType => ObstacleType.Log;

        public override IEnumerator Activate(float speedMultiplier) {
            yield break;
        }
    }
}