USE [aca]
GO

SET IDENTITY_INSERT [dbo].[equiv_position] ON 
GO
INSERT [dbo].[equiv_position] ([position_id], [name]) VALUES (1, N'Advisor')
GO
INSERT [dbo].[equiv_position] ([position_id], [name]) VALUES (2, N'Asst Coach')
GO
INSERT [dbo].[equiv_position] ([position_id], [name]) VALUES (3, N'Head Coach')
GO
INSERT [dbo].[equiv_position] ([position_id], [name]) VALUES (4, N'Para Professionals')
GO
SET IDENTITY_INSERT [dbo].[equiv_position] OFF
GO

SET IDENTITY_INSERT [dbo].[insurance_carrier] ON 
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (1, N'BCBS', 0, 0)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (2, N'Health Partners', 0, 0)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (3, N'Preferred One', 0, 0)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (4, N'PEIP', 0, 0)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (5, N'Medica', 0, 0)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (6, N'CHS', 0, 1)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (7, N'MidAmerica', 0, 1)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (8, N'Genisis', 0, 1)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (9, N'EBC HRA', 0, 1)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (10, N'Wellmark', 0, 0)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (1010, N'ACT-1', 0, 0)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (1011, N'ACT-2', 0, 1)
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (1012, N'ACT-Auto', 0, 0)
GO
SET IDENTITY_INSERT [dbo].[insurance_carrier] OFF
GO

SET IDENTITY_INSERT [dbo].[term] ON 
GO
INSERT [dbo].[term] ([term_id], [name], [description]) VALUES (1, N'Seasonal Employees: ', N'an employee in a position for which the customary annual employment is six months or less. The reference to customary means that by the nature of the position an employee in this position typically works for a period of six months or less, and that period should begin each calendar year in approximately the same part of the year, such as summer or winter. In certain unusual instances, the employee can still be considered a seasonal employee even if the seasonal employment is extended in a particular year beyond its customary duration (regardless of whether the customary duration is six months or is less than six months). Employers are permitted through 2014 to use reasonable, good faith interpretation of the term seasonal employee for purposes of the shared responsibility requirements. ')
GO
INSERT [dbo].[term] ([term_id], [name], [description]) VALUES (2, N'Stability period: ', N'The term stability period means a period selected by an applicable large employer member that immediately follows, and is associated with, a standard measurement period or an initial measurement period (and, if elected by the employer, the administrative period associated with that standard measurement period or initial measurement period), and is used by the applicable large employer member as part of the look-back measurement method.')
GO
INSERT [dbo].[term] ([term_id], [name], [description]) VALUES (3, N'Standard Measurement Period: ', N'The term standard measurement period means a period of at least three but not more than 12 consecutive months that is used by an applicable large employer member as part of the look-back measurement method.')
GO
INSERT [dbo].[term] ([term_id], [name], [description]) VALUES (4, N'Variable Hour Employee: ', N'The term variable hour employee means an employee if, based on the facts and circumstances at the employee''s start date, the applicable large employer member cannot determine whether the employee is reasonably expected to be employed on average at least 30 hours of service per week during the initial measurement period because the employee''s hours are variable or otherwise uncertain.')
GO
INSERT [dbo].[term] ([term_id], [name], [description]) VALUES (5, N'AFFORDABILITY SAFE HARBORS ', N'An employer may use one or more of the affordability safe harbors if it offers its full-time employees (and dependents) the opportunity to enroll in minimum essential coverage under a health plan that provides minimum value with respect to the self-only coverage offered to the employees.')
GO
INSERT [dbo].[term] ([term_id], [name], [description]) VALUES (6, N'Form W-2 Safe Harbor', N'Under the Form W-2 safe harbor, an employer may determine the affordability of its health coverage by reference only to an employee’s wages from that employer, instead of by reference to the employee’s household income. Wages for this purpose is the amount that is required to be reported in Box 1 of the employee’s Form W-2. 
An employer satisfies the Form W-2 safe harbor with respect to an employee if the employee’s required contribution for the calendar year for the employer’s lowest cost self-only coverage that provides minimum value during the entire calendar year (excluding COBRA or other continuation coverage except with respect to an active employee eligible for continuation coverage) does not exceed 9.56 percent of that employee’s Form W–2 wages from the employer for the calendar year.
Eligibility for the Form W-2 Safe Harbor
To be eligible for the Form W-2 safe harbor, the employee’s required contribution must remain a consistent amount or percentage of all Form W–2 wages during the calendar year (or during the plan year for plans with non-calendar year plan years). Thus, an applicable large employer is not permitted to make discretionary adjustments to the required employee contribution for a pay period. A periodic contribution that is based on a consistent percentage of all Form W–2 wages may be subject to a dollar limit specified by the employer.
Timing of the Form W-2 Safe Harbor 
Employers determine whether the Form W-2 safe harbor applies after the end of the calendar year and on an employee-by-employee basis, taking into account W-2 wages and employee contributions.
Partial-year Offers of Coverage
For an employee who was not offered coverage for an entire calendar year, the Form W-2 safe harbor is applied by:
•	Adjusting the employee’s Form W-2 wages to reflect the period when the employee was offered coverage; and
•	Comparing the adjusted wage amount to the employee’s share of the premium for the employer’s lowest cost self-only coverage that provides minimum value for the periods when coverage was offered.
Specifically, the amount of the employee’s compensation for purposes of the Form W-2 safe harbor is determined by multiplying the wages for the calendar year by a fraction equal to the number of calendar months for which coverage was offered over the number of calendar months in the employee’s period of employment with the employer during the calendar year. For this purpose, if coverage is offered during at least one day during the calendar month, or the employee is employed for at least one day during the calendar month, the entire calendar month is counted in determining the applicable fraction.
')
GO
INSERT [dbo].[term] ([term_id], [name], [description]) VALUES (7, N'Rate of Pay Safe Harbor', N'The rate of pay safe harbor was designed to allow employers to prospectively satisfy affordability without the need to analyze every employee’s wages and hours.
For hourly employees, the rate of pay safe harbor allows an employer to:
•	Take the lower of the hourly employee’s rate of pay as of the first day of the coverage period (generally, the first day of the plan year) or the employee’s lowest hourly rate of pay during the calendar month;
•	Multiply that rate by 130 hours per month (the benchmark for full-time status for a month); and
•	Determine affordability for the calendar month based on the resulting monthly wage amount.
Specifically, the employee’s monthly contribution amount (for the self-only premium of the employer’s lowest cost coverage that provides minimum value) is affordable for a calendar month if it is equal to or lower than 9.566 (2015) percent of the computed monthly wages (that is, the employee’s applicable hourly rate of pay multiplied by 130 hours).
The final regulations, unlike the proposed regulations, permit an employer to use the rate of pay safe harbor even if an hourly employee’s rate of pay is reduced during the year.
For salaried employees, monthly salary as of the first day of the coverage period would be used instead of hourly salary multiplied by 130 hours. However, if the monthly salary is reduced, including due to a reduction in work hours, the rate of pay safe harbor may not be used.
')
GO
INSERT [dbo].[term] ([term_id], [name], [description]) VALUES (8, N'Federal Poverty Line Safe Harbor', N'An employer may also rely on a design-based safe harbor using the federal poverty line (FPL) for a single individual. The FPL safe harbor allows employers to disregard certain employees in determining the affordability of health coverage (that is, employees who cannot receive an Exchange subsidy because of their income level or eligibility for Medicare, and therefore cannot trigger an employer’s liability for a shared responsibility penalty). The FPL safe harbor also provides employers with a predetermined maximum amount of employee contribution that in all cases will result in the coverage being deemed affordable.
Employer-provided coverage is considered affordable under the FPL safe harbor if the employee’s required contribution for the calendar month for the lowest cost self-only coverage that provides minimum value does not exceed 9.56 percent of the FPL for a single individual for the applicable calendar year, divided by 12. The final regulations allow employers to use any of the poverty guidelines in effect within six months before the first day of the plan year for purposes of this safe harbor.
')
GO
SET IDENTITY_INSERT [dbo].[term] OFF
GO
