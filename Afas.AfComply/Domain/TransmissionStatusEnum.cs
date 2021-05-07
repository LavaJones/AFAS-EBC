using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum TransmissionStatusEnum
{
    Initiated = 1,
    F1094_Collected = 2,
    Review = 3,
    Certified = 4,
    CompanyApproved = 5,
    CASSGenerated = 6,
    CASSRecieved = 7,
    Print = 8,
    Printed = 9,
    Mailed = 10,
    Transmit = 11,
    Answered = 12,
    ETL = 13,
    Halt = 14,
    SkipCASS = 15,
    Transmitted = 16,
    Accepted = 17,
    AcceptedWithErrors = 18,
    Rejected = 19,
    ReTransmit = 20,
    ReTransmitted = 21,
    StepThree = 22,
    StepFive = 23
}
