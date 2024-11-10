using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    #region Events
    UnityAction<int> OnRoundChanges;
    UnityAction<int> OnPointsChange;
    #endregion

    #region Attributes
    private int _round, _points;

    public int Round {
        get => _round;
    }
    public int Points {
        get => _points;
        private set {
            _points = value;
        }
    }

    #endregion

    

    #region MonoBehaviour Methods

    private void Start()
    {
        
    }

    private void AddPoints(int pointsToAdd)
    {
        
    }
    #endregion
}
