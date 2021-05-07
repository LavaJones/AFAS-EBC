using System;

namespace Afas.AfComply.Domain
{

    /// <summary>
    /// Maps to the different ACA Status fields in the ACA database. Air has its own values.
    /// </summary>
    public enum ACAStatusEnum
    {
        Seasonal = 1,
        PartTime = 2,
        Termed = 3,
        CobraElected = 4,
        FullTime = 5,
        SpecialUnpaidLeave = 6,
        InitialImport = 7,
        Retiree = 8,
    }

}
