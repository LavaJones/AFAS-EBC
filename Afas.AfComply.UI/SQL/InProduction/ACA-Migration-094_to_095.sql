USE [aca]

UPDATE dbo.TransmissionStatus
SET TransmissionStatusName = 'Transmit'
WHERE TransmissionStatusId = 11

SET IDENTITY_INSERT dbo.TransmissionStatus ON

INSERT INTO dbo.TransmissionStatus (TransmissionStatusId, TransmissionStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
VALUES(16, 'Transmitted', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE())

INSERT INTO dbo.TransmissionStatus (TransmissionStatusId, TransmissionStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
VALUES(17, 'Accepted', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE())

INSERT INTO dbo.TransmissionStatus (TransmissionStatusId, TransmissionStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
VALUES(18, 'AcceptedWithErrors', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE())

INSERT INTO dbo.TransmissionStatus (TransmissionStatusId, TransmissionStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
VALUES(19, 'Rejected', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE())

INSERT INTO dbo.TransmissionStatus (TransmissionStatusId, TransmissionStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
VALUES(20, 'ReTransmit', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE())

INSERT INTO dbo.TransmissionStatus (TransmissionStatusId, TransmissionStatusName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate)
VALUES(21, 'ReTransmitted', 'SYSTEM', GETDATE(), 'SYSTEM', GETDATE())

SET IDENTITY_INSERT dbo.TransmissionStatus OFF


