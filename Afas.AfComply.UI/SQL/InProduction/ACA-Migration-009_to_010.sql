USE aca
UPDATE dbo.term SET [description] = 'An employee in a position for which the customary annual employment is six months or less. The reference to customary means that by the nature of the position an employee in this position typically works for a period of six months or less, and that period should begin each calendar year in approximately the same part of the year, such as summer or winter. In certain unusual instances, the employee can still be considered a seasonal employee even if the seasonal employment is extended in a particular year beyond its customary duration (regardless of whether the customary duration is six months or is less than six months).'
WHERE name = 'Seasonal Employees:'

UPDATE dbo.term SET [description] = 'The term stability period means a period selected by an applicable large employer member that immediately follows, and is associated with, a standard measurement period or an initial measurement period (and, if elected by the employer, the administrative period associated with that standard measurement period or initial measurement period), and is used by the applicable large employer member as part of the look-back measurement method.'
WHERE name = 'Stability period:'

UPDATE dbo.term SET [description] = 'The term standard measurement period means a period of at least three but not more than 12 consecutive months that is used by an applicable large employer member as part of the look-back measurement method.'
WHERE name = 'Standard Measurement Period:'

UPDATE dbo.term SET [description] = 'The term variable hour employee means an employee if, based on the facts and circumstances at the employee''s start date, the applicable large employer member cannot determine whether the employee is reasonably expected to be employed on average at least 30 hours of service per week during the initial measurement period because the employee''s hours are variable or otherwise uncertain.'
WHERE name = 'Variable Hour Employee:'

UPDATE dbo.term SET [description] = 'An employer may use one or more of the affordability safe harbors if it offers its full-time employees (and dependents) the opportunity to enroll in minimum essential coverage under a health plan that provides minimum value with respect to the self-only coverage offered to the employees. Use of any of the safe harbors is optional for an applicable large employer member, and an applicable large employer member may choose to apply the safe harbors for any reasonable category of employees, provided it does so on a uniform and consistent basis for all employees in a category. Reasonable categories generally include specified job categories, nature of compensation (hourly or salary), geographic location, and similar bona fide business criteria.'
WHERE name = 'AFFORDABILITY SAFE HARBORS '

UPDATE dbo.term SET [description] = 'Under the Form W-2 safe harbor, an employer may determine the affordability of its health coverage by reference only to an employee�s wages from that employer, instead of by reference to the employee�s household income. Wages for this purpose is the amount that is required to be reported in Box 1 of the employee�s Form W-2. An employer satisfies the Form W-2 safe harbor with respect to an employee if the employee�s required contribution for the calendar year for the employer�s lowest cost self-only coverage that provides minimum value during the entire calendar year (excluding COBRA or other continuation coverage except with respect to an active employee eligible for continuation coverage) does not exceed 9.5% (as adjusted yearly for inflation; in 2016 9.66%) of that employee�s Form W�2 wages from the employer for the calendar year. To be eligible for the Form W-2 safe harbor, the employee�s required contribution must remain a consistent amount or percentage of all Form W�2 wages during the calendar year (or during the plan year for plans with non-calendar year plan years). Thus, an applicable large employer is not permitted to make discretionary adjustments to the required employee contribution for a pay period. A periodic contribution that is based on a consistent percentage of all Form W�2 wages may be subject to a dollar limit specified by the employer. Employers determine whether the Form W-2 safe harbor applies after the end of the calendar year and on an employee-by-employee basis, taking into account W-2 wages and employee contributions. For an employee who was not offered coverage for an entire calendar year, the Form W-2 safe harbor is applied by: Adjusting the employee�s Form W-2 wages to reflect the period when the employee was offered coverage; and Comparing the adjusted wage amount to the employee�s share of the cost for the employer�s lowest cost self-only coverage that provides minimum value for the periods when coverage was offered. Specifically, the amount of the employee�s compensation for purposes of the Form W-2 safe harbor is determined by multiplying the wages for the calendar year by a fraction equal to the number of calendar months for which coverage was offered over the number of calendar months in the employee�s period of employment with the employer during the calendar year. For this purpose, if coverage is offered during at least one day during the calendar month, or the employee is employed for at least one day during the calendar month, the entire calendar month is counted in determining the applicable fraction.'
WHERE name = 'Form W-2 Safe Harbor'

UPDATE dbo.term SET [description] = 'The rate of pay safe harbor was designed to allow employers to prospectively satisfy affordability. For hourly employees, the rate of pay safe harbor allows an employer to: Take the lower of the hourly employee�s rate of pay as of the first day of the coverage period (generally, the first day of the plan year) or the employee�s lowest hourly rate of pay during the calendar month; Multiply that rate by 130 hours per month (the benchmark for full-time status for a month); and Determine affordability for the calendar month based on the resulting monthly wage amount. Specifically, the employee�s monthly contribution amount (for the self-only cost of the employer�s lowest cost coverage that provides minimum value) is affordable for a calendar month if it is equal to or lower than 9.5% (as adjusted yearly for inflation; in 2016 9.66%) of the computed monthly wages (that is, the employee�s applicable hourly rate of pay multiplied by 130 hours). An employer may use the rate of pay safe harbor even if an hourly employee�s rate of pay is reduced during the year. For salaried employees, monthly salary as of the first day of the coverage period would be used instead of hourly salary multiplied by 130 hours. However, if the monthly salary is reduced, including due to a reduction in work hours, the rate of pay safe harbor may not be used.'
WHERE name = 'Rate of Pay Safe Harbor'

UPDATE dbo.term SET [description] = 'An employer may also rely on a design-based safe harbor using the federal poverty level (FPL) for a single individual. The FPL safe harbor provides employers with a predetermined maximum amount of employee contribution that in all cases will result in the coverage being deemed affordable. Employer-provided coverage is considered affordable under the FPL safe harbor if the employee�s required contribution for the calendar month for the lowest cost self-only coverage that provides minimum value does not exceed 9.5% (as adjusted yearly for inflation; in 2016 9.66%) of the FPL for a single individual for the applicable calendar year, divided by 12. Employers may use any of the poverty guidelines in effect within six months before the first day of the plan year for purposes of this safe harbor.'
WHERE name = 'Federal Poverty Line Safe Harbor'

