using System;

namespace Task6_2.Models;

public class Loader
{
   public string Name { get; set; }
   public int ProducedCandyCount { get; set; }
   public int SugarUsedCount { get; set; }
   public double ProbabilityOfFailure { get; set; }
   public event Action ProduceCandyEvent;
   public event Action FailureEvent;

   public Loader(string name, int count, int sugar)
   {
      Name = name;
      ProducedCandyCount = count;
      ProbabilityOfFailure = 0.05;
      SugarUsedCount = sugar;
   }
   
   public void ProduceCandy()
   {
      if (new Random().NextDouble() < ProbabilityOfFailure)
      {
         ProduceCandyEvent?.Invoke();
      }
      else
      {
         FailureEvent?.Invoke();
      }
   }
}