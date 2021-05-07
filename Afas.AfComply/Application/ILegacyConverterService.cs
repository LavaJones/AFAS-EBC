using System;

using System.Data;

namespace Afas.AfComply.Application
{
    
    /// <summary>
    /// Handles the legacy formats into AFcomply format translations.
    /// </summary>
    public interface ILegacyConverterService
    {

        /// <summary>
        /// Handle the Demographics format conversion, first variant.
        /// </summary>
        DataTable ConvertDemographicsVariant1(String[] source, String employerFederalIdentificationNumber);

        /// <summary>
        /// Handle the Coverage format conversion, first variant.
        /// </summary>
        DataTable ConvertCoverageVariant1(String[] source, String employerFederalIdentificationNumber);

        /// <summary>
        /// Handle the Extended Offer format conversion, first variant.
        /// </summary>
        DataTable ConvertExtendedOfferVariant1(String[] source, String employerFederalIdentificationNumber);

        /// <summary>
        /// Handle the Ohio Payroll format conversion, first variant.
        /// </summary>
        DataTable ConvertPayrollOhioVariant1(String[] source, String employerFederalIdentificationNumber);

        /// <summary>
        /// Handle the Ohio Payroll format conversion, second variant.
        /// </summary>
        DataTable ConvertPayrollOhioVariant2(String[] source, String employerFederalIdentificationNumber, int daysInThePast);

        /// <summary>
        /// Handle the Payroll format conversion, first variant.
        /// </summary>
        DataTable ConvertPayrollVariant1(String[] source, String employerFederalIdentificationNumber, int daysInThePast);

    }

}
