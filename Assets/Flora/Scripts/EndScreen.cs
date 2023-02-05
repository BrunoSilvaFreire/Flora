using System;
using System.Linq;
using Flora.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;
namespace Flora.Scripts {
    public class EndScreen : MonoBehaviour {
        [Serializable]
        public class EndEntry {
            public Text playerTitle, score;
        }
        public EndEntry[] entries;
        public void Show() {
            gameObject.SetActive(true);
            var pullas = FindObjectsOfType<Pulla>().ToList();
            pullas.Sort((a, b) => a.score.CompareTo(b.score));
            for (var i = 0; i < entries.Length; i++) {
                var entry = entries[i];
                if (i >= pullas.Count) {
                    entry.playerTitle.text = "Not playing";
                    entry.score.text = "";
                } else {
                    var pulla = pullas[i];
                    entry.playerTitle.text = $"P{pulla.id}";
                    entry.score.text = pulla.score.ToString();
                    entry.playerTitle.color = pulla.color;
                    entry.score.color = pulla.color;
                }
            }
        }
        public void Hide() {
            gameObject.SetActive(false);
        }
    }
}