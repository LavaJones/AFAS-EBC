using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class ValidationHelper
{

    public static bool validateNewEmployerTaxYearTransmission(EmployerTaxYearTransmission newEmployerTaxYearTransmission, ILog logger)
    {
        if (newEmployerTaxYearTransmission.EmployerTaxYearTransmissionId > 0)
        {
            return true;
        }
        else
        {
            logger.Warn(string.Format("Did not create a new EmployerTaxYearTransmission with EmployerId: {0} and TaxYearId: {1}",
                        newEmployerTaxYearTransmission.EmployerId,
                        newEmployerTaxYearTransmission.TaxYearId));
            return false;
        }
    }

    public static bool validateNewEmployerTaxYearTransmissionStatus(EmployerTaxYearTransmissionStatus newEmployerTaxYearTransmissionStatus, ILog logger)
    {
        if (newEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionStatusId > 0)
        {
            return true;
        }else{
            logger.Warn(string.Format("Did not create a new EmployerTaxYearTransmissionStatus with EmployerTaxYearTransmissionId: {0}, StartDate: {1}, and EndDate: {2}",
                        newEmployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                        newEmployerTaxYearTransmissionStatus.StartDate,
                        newEmployerTaxYearTransmissionStatus.EndDate));
             return false;
        }
    }

}