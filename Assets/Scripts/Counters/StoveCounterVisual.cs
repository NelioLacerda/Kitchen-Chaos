using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
   [SerializeField] private GameObject stoveOnGameObject;
   [SerializeField] private GameObject particlesGameObject;
   
   [SerializeField] private StoveCounter counter;

   private void Start()
   {
      counter.OnStateChanged += CounterOnOnStateChanged;
   }

   private void CounterOnOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
   {
      bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried
                                                             || e.state == StoveCounter.State.Burned;
      stoveOnGameObject.SetActive(showVisual);
      particlesGameObject.SetActive(showVisual);
   }
}