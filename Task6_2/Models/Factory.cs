using System;

namespace Task6_2.Models;

public class Factory
{
    private Loader _loader;
    public string Name { get; set; }
    public int SugarCount { get; set; }
    public bool Worked { get; set; }
    public event Action SugarEndedEvent;
    

    public Factory(string name, int count, Loader loader)
    {
        Name = name;
        SugarCount = count;
        _loader = loader;
        _loader.ProduceCandyEvent += OnProduceCandy;
        Worked = true;
    }

    private void OnProduceCandy()
    {
        if (SugarCount - _loader.SugarUsedCount <= 0)
        {
            _loader.SugarUsedCount = 0;
            SugarEndedEvent?.Invoke(); 
        }
        else
        {
            SugarCount -= _loader.SugarUsedCount;
        }
    }

    private void OnFailure()
    {
        Worked = false;
    }
}