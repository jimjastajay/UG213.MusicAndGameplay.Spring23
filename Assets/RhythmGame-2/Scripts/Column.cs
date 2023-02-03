using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Column : MonoBehaviour {

    Sequencer sequencer;

    public List<GridSquare> gridSquareList = new List<GridSquare>();

    #region Setup
    private void Awake() {
    }
    #endregion

    public void PlayColumn() {
        gridSquareList.ForEach(x => x.PlaySquare());
    }
    
}
