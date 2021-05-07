using System;
using System.Collections.Generic;
using System.Linq;

using Afas.AfComply.Domain.POCO;

namespace Afas.AfComply.Domain
{
    
    public static class IListOfAverageHoursExtensionMethods
    {

        /// <summary>
        /// Returns all of the AverageHours.MonthlyAverageHours greater than or equal to 100 and less than 130.
        /// </summary>
        public static IQueryable<AverageHours> FilterForCautionMonthly(this IList<AverageHours> averageHours)
        {

            return (
                    from AverageHours theAverageHours in averageHours
                    where
                        theAverageHours.MonthlyAverageHours >= 100
                            &&
                        theAverageHours.MonthlyAverageHours < 130
                    select theAverageHours
                ).AsQueryable();

        }

        /// <summary>
        /// Returns all of the AverageHours.TrendingMonthlyAverageHours greater than or equal to 100 and less than 130.
        /// </summary>
        public static IQueryable<AverageHours> FilterForCautionTrending(this IList<AverageHours> averageHours)
        {

            return (
                    from AverageHours theAverageHours in averageHours
                    where
                        theAverageHours.TrendingMonthlyAverageHours >= 100
                            &&
                        theAverageHours.TrendingMonthlyAverageHours < 130
                    select theAverageHours
                ).AsQueryable();

        }

        /// <summary>
        /// Returns all of the AverageHours.MonthlyAverageHours less than 100.
        /// </summary>
        public static IQueryable<AverageHours> FilterForNotOntrackMonthly(this IList<AverageHours> averageHours)
        {

            return (
                    from AverageHours theAverageHours in averageHours
                    where theAverageHours.MonthlyAverageHours < 100
                    select theAverageHours
                ).AsQueryable();

        }

        /// <summary>
        /// Returns all of the AverageHours.MonthlyAverageHours greater than or equal to 130.
        /// </summary>
        public static IQueryable<AverageHours> FilterForOntrackMonthly(this IList<AverageHours> averageHours)
        {

            return (
                    from AverageHours theAverageHours in averageHours
                    where theAverageHours.MonthlyAverageHours >= 130
                    select theAverageHours
                ).AsQueryable();

        }

        /// <summary>
        /// Returns all of the AverageHours.TrendingMonthlyAverageHours less than 100.
        /// </summary>
        public static IQueryable<AverageHours> FilterForNotOntrackTrending(this IList<AverageHours> averageHours)
        {

            return (
                    from AverageHours theAverageHours in averageHours
                    where theAverageHours.TrendingMonthlyAverageHours < 100
                    select theAverageHours
                ).AsQueryable();

        }

        /// <summary>
        /// Returns all of the AverageHours.TrendingMonthlyAverageHours greater than or equal to 130.
        /// </summary>
        public static IQueryable<AverageHours> FilterForOntrackTrending(this IList<AverageHours> averageHours)
        {

            return (
                    from AverageHours theAverageHours in averageHours
                    where theAverageHours.TrendingMonthlyAverageHours >= 130
                    select theAverageHours
                ).AsQueryable();

        }

        /// <summary>
        /// Filters for the AverageHours tied to the new hire measurement period.
        /// </summary>
        public static IQueryable<AverageHours> FilterForAnIntialMeasurementPeriod(this IList<AverageHours> averageHours)
        {

            return (
                    from AverageHours theAverageHours in averageHours
                    where theAverageHours.IsNewHire == true
                    select theAverageHours
                ).AsQueryable();

        }

        /// <summary>
        /// Filters for the AverageHours tied to an ongoing measurement period.
        /// </summary>
        public static IQueryable<AverageHours> FilterForAnOngoingMeasurementPeriod(this IList<AverageHours> averageHours)
        {

            return (
                    from AverageHours theAverageHours in averageHours
                    where theAverageHours.IsNewHire == false
                    select theAverageHours
                ).AsQueryable();

        }

    }

}
