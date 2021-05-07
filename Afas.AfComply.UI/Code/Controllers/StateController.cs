using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for StateController
/// </summary>
public static class StateController
{
    /// <summary>
    /// 
    /// </summary>
    private static List<State> states = new List<State>();

    public static List<State> getStates()
    {
        if (states.Count() < 1)
        {
            generateStates();
            return states;
        }
        else
        {
            return states;
        }
    }

    public static void addState(State _state)
    {
        states.Add(_state);
    }


    public static bool generateStates()
    {
        if (states.Count() < 1)
        {
            StateFactory sf = new StateFactory();
            bool states_generated = false;

            states_generated = sf.manufactureStateList();

            return states_generated;
        }
        else
        {
            return true;
        }
    }

    public static State findState(int _stateID)
    {
        StateShow ss = new StateShow();
        return ss.findState(_stateID);
    }

    public static State findState(string _stateAbbreviation)
    {
        StateShow ss = new StateShow();
        return ss.findState(_stateAbbreviation);
    }

    public static int findStateID(string _stateAbbreviation)
    {
        StateShow ss = new StateShow();
        return ss.findStateID(_stateAbbreviation);
    }


}