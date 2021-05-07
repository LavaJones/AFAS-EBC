using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UnitController
/// </summary>
public static class UnitController
{
    public static List<Unit> getEmployerUnits()
    {
        UnitFactory uf = new UnitFactory();
        return uf.manufactureUnitList();
    }
}