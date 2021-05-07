using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Summary description for StateShow
/// </summary>
public class StateShow
{
    private ILog Log = LogManager.GetLogger(typeof(StateShow));

    public State findState(int _stateID)
    {
        State tempState = null;

        foreach (State s in StateController.getStates())
        {
            if (s.State_ID == _stateID)
            {
                tempState = s;
                break;
            }
        }

        return tempState;
    }

    public State findState(string _stateAbbreviation)
    {
        State tempState = null;

        foreach (State s in StateController.getStates())
        {
            if (s.State_Abbr.ToLower() == _stateAbbreviation.ToLower())
            {
                tempState = s;
                break;
            }
        }

        return tempState;
    }

    public int findStateID(string _stateAbbreviation)
    {
        int _stateID = 0;

        try
        {
            foreach (State s in StateController.getStates())
            {
                if (s.State_Abbr.ToLower() == _stateAbbreviation.ToLower())
                {
                    _stateID = s.State_ID;
                    break;
                }
            }
        }
        catch (Exception exception)
        {
            Log.Warn("Suppressing errors.", exception);
            return _stateID;
        }

        return _stateID;
    }
}