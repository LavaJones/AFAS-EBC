USE [aca-demo]
GO

INSERT INTO [dbo].[tax_year] (tax_year) VALUES (2016)
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QueueStatus](
      [QueueStatusId] [int] IDENTITY(1,1) NOT NULL,
      [QueueStatusName] [nvarchar](50) NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,  CONSTRAINT [PK_QueueStatus] PRIMARY KEY CLUSTERED (
      [QueueStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT SELECT ON [dbo].[QueueStatus] TO [aca-user] AS [dbo]
GO

SET IDENTITY_INSERT dbo.[QueueStatus] ON
GO
INSERT INTO [dbo].[QueueStatus](QueueStatusId,QueueStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (1,'Pending','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO dbo.[QueueStatus](QueueStatusId,QueueStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (2,'Processing','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO dbo.[QueueStatus](QueueStatusId,QueueStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (3,'Completed','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO dbo.[QueueStatus](QueueStatusId,QueueStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (4,'Canceled','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO

SET IDENTITY_INSERT [dbo].[QueueStatus] OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransmissionStatus](
      [TransmissionStatusId] [int] IDENTITY(1,1) NOT NULL,
      [TransmissionStatusName] [nvarchar](50) NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,  CONSTRAINT [PK_TransmissionStatus] PRIMARY KEY CLUSTERED (
      [TransmissionStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

GRANT SELECT ON [dbo].[TransmissionStatus] TO [aca-user] AS [dbo]
GO

SET IDENTITY_INSERT [dbo].[TransmissionStatus] ON
GO

INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (1,'Initiated','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (2,'1094_Collected','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (3,'Review','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (4,'Certified','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (5,'CompanyApproved','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (6,'CASSGenerated','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (7,'CASSRecieved','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (8,'Print','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (9,'Printed','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (10,'Mailed','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (11,'Transmitting','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO
INSERT INTO [dbo].[TransmissionStatus](TransmissionStatusId,TransmissionStatusName,CreatedBy,CreatedDate,ModifiedBy,ModifiedDate) VALUES (12,'Answered','SYSTEM','2017-01-17 00:00:00','SYSTEM','2017-01-17 00:00:00');
GO

SET IDENTITY_INSERT [dbo].[TransmissionStatus] OFF
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployerTaxYearTransmission](
      [EmployerTaxYearTransmissionId] [bigint] IDENTITY(1,1) NOT NULL,
      [EmployerId] [int] NOT NULL,
      [TaxYearId] [int] NOT NULL,
      [ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
      [EntityStatusId] [int] NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,  CONSTRAINT [PK_EmployerTaxYearTransmission] PRIMARY KEY NONCLUSTERED (
      [EmployerTaxYearTransmissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[EmployerTaxYearTransmission] ADD  CONSTRAINT [DF_EmployerTaxYearTransmission_ResourceId]  DEFAULT (newid()) FOR [ResourceId] 
GO 
ALTER TABLE [dbo].[EmployerTaxYearTransmission]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmission_Employer] FOREIGN KEY([EmployerId]) REFERENCES [dbo].[employer] ([employer_id]) 
GO 
ALTER TABLE [dbo].[EmployerTaxYearTransmission] CHECK CONSTRAINT [FK_EmployerTaxYearTransmission_Employer]
GO
ALTER TABLE [dbo].[EmployerTaxYearTransmission]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmission_EntityStatus] FOREIGN KEY([EntityStatusId]) REFERENCES [dbo].[EntityStatus] ([EntityStatusId]) 
GO 
ALTER TABLE [dbo].[EmployerTaxYearTransmission] CHECK CONSTRAINT [FK_EmployerTaxYearTransmission_EntityStatus]
GO
ALTER TABLE [dbo].[EmployerTaxYearTransmission]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmission_TaxYear] FOREIGN KEY([TaxYearId]) REFERENCES [dbo].[tax_year] ([tax_year]) 
GO 
ALTER TABLE [dbo].[EmployerTaxYearTransmission] CHECK CONSTRAINT [FK_EmployerTaxYearTransmission_TaxYear]
GO

GRANT SELECT ON [dbo].[EmployerTaxYearTransmission] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployerTaxYearTransmissionStatus](
      [EmployerTaxYearTransmissionStatusId] [bigint] IDENTITY(1,1) NOT NULL,
      [EmployerTaxYearTransmissionId] [bigint] NOT NULL,
      [TransmissionStatusId] [int] NOT NULL,
      [StartDate] [datetime2](7) NOT NULL,
      [EndDate] [datetime2](7) NULL,
      [ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
      [EntityStatusId] [int] NOT NULL,
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,  CONSTRAINT [PK_EmployerTaxYearTransmissionStatus] PRIMARY KEY NONCLUSTERED (
      [EmployerTaxYearTransmissionStatusId] ASC )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatus] ADD  CONSTRAINT [DF_EmployerTaxYearTransmissionStatus_ResourceId]  DEFAULT (newid()) FOR [ResourceId] 
GO 
ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatus]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmissionStatus_EmployerTaxYearTransmission] FOREIGN KEY([EmployerTaxYearTransmissionId])
REFERENCES [dbo].[EmployerTaxYearTransmission] ([EmployerTaxYearTransmissionId]) 
GO 
ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatus] CHECK CONSTRAINT [FK_EmployerTaxYearTransmissionStatus_EmployerTaxYearTransmission]
GO
ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatus]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmissionStatus_EntityStatus] FOREIGN KEY([EntityStatusId]) REFERENCES [dbo].[EntityStatus] ([EntityStatusId]) 
GO 
ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatus] CHECK CONSTRAINT [FK_EmployerTaxYearTransmissionStatus_EntityStatus]
GO
ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatus]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmissionStatus_TransmissionStatus] FOREIGN KEY([TransmissionStatusId]) REFERENCES [dbo].[TransmissionStatus] ([TransmissionStatusId]) 
GO 
ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatus] CHECK CONSTRAINT [FK_EmployerTaxYearTransmissionStatus_TransmissionStatus]
GO

GRANT SELECT ON [dbo].[EmployerTaxYearTransmissionStatus] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EmployerTaxYearTransmissionStatusQueue](
      [EmployerTaxYearTransmissionStatusQueueId] [bigint] IDENTITY(1,1) NOT NULL,
      [EmployerTaxYearTransmissionStatusId] [bigint] NOT NULL,
      [QueueStatusId] [int] NOT NULL,
      [EntityStatusId] [int] NOT NULL,
      [Title] [nvarchar](100) NOT NULL,
      [Message] [nvarchar](255) NOT NULL,
      [Body] [nvarchar](max) NOT NULL,
      [ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_EmployerTaxYearTransmissionStatusQueue_ResourceId]  DEFAULT (newid()),
      [CreatedBy] [nvarchar](50) NOT NULL,
      [CreatedDate] [datetime2](7) NOT NULL,
      [ModifiedBy] [nvarchar](50) NOT NULL,
      [ModifiedDate] [datetime2](7) NOT NULL,
CONSTRAINT [PK_EmployerTaxYearTransmissionStatusQueue] PRIMARY KEY NONCLUSTERED 
(
      [EmployerTaxYearTransmissionStatusQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatusQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmissionStatusQueue_EmployerTaxYearTransmissionStatusId] FOREIGN KEY([EmployerTaxYearTransmissionStatusId])
REFERENCES [dbo].[EmployerTaxYearTransmissionStatus] ([EmployerTaxYearTransmissionStatusId])
GO

ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatusQueue] CHECK CONSTRAINT [FK_EmployerTaxYearTransmissionStatusQueue_EmployerTaxYearTransmissionStatusId]
GO

ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatusQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmissionStatusQueue_EntityStatus] FOREIGN KEY([EntityStatusId])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatusQueue] CHECK CONSTRAINT [FK_EmployerTaxYearTransmissionStatusQueue_EntityStatus]
GO

ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatusQueue]  WITH CHECK ADD  CONSTRAINT [FK_EmployerTaxYearTransmissionStatusQueue_QueueStatus] FOREIGN KEY([QueueStatusId])
REFERENCES [dbo].[QueueStatus] ([QueueStatusId])
GO

ALTER TABLE [dbo].[EmployerTaxYearTransmissionStatusQueue] CHECK CONSTRAINT [FK_EmployerTaxYearTransmissionStatusQueue_QueueStatus]
GO

GRANT SELECT ON [dbo].[EmployerTaxYearTransmissionStatusQueue] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_employers_in_transmission_status] 
      @taxYearId int,
      @transmissionStatusId int,
      @entityStatusId int = 1
AS
BEGIN

      SET NOCOUNT ON;

      SELECT
            e.employer_id,
            e.name,
            e.[address],
            e.city,
            s.abbreviation AS 'state',
            e.zip,
            e.bill_address,
            e.bill_city,
            e.bill_state,
            e.bill_zip
      FROM
            dbo.employer AS e
            INNER JOIN dbo.[state] AS s ON e.state_id = s.state_id
            INNER JOIN(
                        SELECT
                              etyt.EmployerId,
                              etyts.EmployerTaxYearTransmissionId, 
                              etyts.EmployerTaxYearTransmissionStatusId
                        FROM
                              EmployerTaxYearTransmissionStatus AS etyts
                              INNER JOIN EmployerTaxYearTransmission AS etyt ON etyts.EmployerTaxYearTransmissionId = etyt.EmployerTaxYearTransmissionId
                        WHERE
                              etyts.TransmissionStatusId = @transmissionStatusId
                                    AND
                              etyt.TaxYearId = @taxYearId
                                    AND
                              etyt.EntityStatusId = @entityStatusId
                                    AND
                              etyts.EntityStatusId = @entityStatusId
                  ) AS et ON e.employer_id = et.EmployerId;

END

GO

GRANT EXECUTE ON [dbo].[SELECT_employers_in_transmission_status] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_employer_transmission_tax_year_status]
      @employerTransmissionTaxYearStatusId int
AS
BEGIN

      SET NOCOUNT ON;

      SELECT
        [EmployerTaxYearTransmissionStatusId],
      [EmployerTaxYearTransmissionId],
      [TransmissionStatusId],
      [StartDate],
      [EndDate],
      [ResourceId],
      [EntityStatusId],
      [CreatedBy],
      [CreatedDate],
      [ModifiedBy],
      [ModifiedDate]
  FROM
      [dbo].[EmployerTaxYearTransmissionStatus]
  WHERE
      [EmployerTaxYearTransmissionStatusId] = @employerTransmissionTaxYearStatusId
END
GO

GRANT EXECUTE ON [dbo].[SELECT_employer_transmission_tax_year_status] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_employer_tax_year_transmission_status_queue]
      @batchSize int,
      @queueStatusId int = 1,
      @entityStatusId int = 1
AS
BEGIN

      SET NOCOUNT ON;

      SELECT TOP (@BatchSize) 
            [EmployerTaxYearTransmissionStatusQueueId], 
            [EmployerTaxYearTransmissionStatusId], 
            [QueueStatusId], 
            [EntityStatusId], 
            [Title],
            [Message],
            [Body],
            [ResourceId],
            [CreatedBy],
            [CreatedDate],
            [ModifiedBy],
            [ModifiedDate]
      FROM
            [dbo].[EmployerTaxYearTransmissionStatusQueue]
      WHERE
            ([QueueStatusId] = @QueueStatusId)
                  AND
            ([EntityStatusId] = @EntityStatusId)
      ORDER BY
            [CreatedDate] ASC
END
GO

GRANT EXECUTE ON [dbo].[SELECT_employer_tax_year_transmission_status_queue] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SELECT_employer_tax_year_transmission]
      @employerTaxYearTransmissionId int = 0,
      @employerId int = 0,
      @taxYearId int = 0
AS
BEGIN

      SET NOCOUNT ON;

      SELECT 
            EmployerTaxYearTransmissionId,
            EmployerId,
            TaxYearId,
            ResourceId,
            EntityStatusId,
            CreatedBy,
            CreatedDate,
            ModifiedBy,
            ModifiedDate
      FROM
            EmployerTaxYearTransmission
      WHERE
            (EmployerTaxYearTransmissionId = @employerTaxYearTransmissionId)
                  OR
            (EmployerId = @employerId AND TaxYearId = @taxYearId)
END

GO

GRANT EXECUTE ON [dbo].[SELECT_employer_tax_year_transmission] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_current_employers_tax_year_transmission_status]
      @taxYearId int,
      @transmissionStatusId int,
      @entityStatusId int = 1
AS
BEGIN

      SET NOCOUNT ON;

      DECLARE @execstatementsbatch nvarchar(max) = '';

      DECLARE @currentEmployerTaxYearTransmissionStatuses TABLE(
            EmployerTaxYearTransmissionStatusId int,
            EmployerTaxYearTransmissionId int,
            TransmissionStatusId int,
            ResourceId uniqueidentifier,
            EntityStatusId int,
            StartDate datetime2,
            EndDate datetime2,
            CreatedBy nvarchar(50),
            CreatedDate datetime2,
            ModifiedBy nvarchar(50),
            ModifiedDate datetime2
      )

      SELECT @execstatementsbatch += 'Exec SELECT_current_employer_tax_year_transmission_status ' + CAST(employer_id AS varchar)  + ', ' + CAST(@taxYearId AS varchar)+ '; '
      FROM employer

      print(@execstatementsbatch)

      Insert into @currentEmployerTaxYearTransmissionStatuses EXEC(@execstatementsbatch)

      SELECT
            cetyt.EmployerTaxYearTransmissionId,
            e.employer_id,
            e.ResourceId,
            e.name,
            e.[address],
            e.city,
            s.abbreviation AS 'state',
            e.zip,
            e.bill_address,
            e.bill_city,
            e.bill_state,
            e.bill_zip
      FROM
            employer AS e 
            INNER JOIN [state] as s ON e.state_id = s.state_id
            INNER JOIN (
                        SELECT
                              etyt.EmployerId, 
                              etyt.EmployerTaxYearTransmissionId
                        FROM
                              @currentEmployerTaxYearTransmissionStatuses as cetyts 
                              INNER JOIN EmployerTaxYearTransmission AS etyt ON cetyts.EmployerTaxYearTransmissionId = etyt.EmployerTaxYearTransmissionId
                        WHERE
                              TransmissionStatusId = @transmissionStatusId
                  ) AS cetyt ON e.employer_id = cetyt.EmployerId

END
GO

GRANT EXECUTE ON [dbo].[SELECT_current_employers_tax_year_transmission_status] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_current_employer_tax_year_transmission_status]
      @employerId int = 0,
      @taxYearId int = 0,
      @entityStatusId int = 1,
      @employerResourceId uniqueidentifier = NULL
AS
BEGIN

      SET NOCOUNT ON;
      
      DECLARE @employerTaxYearTransmissionStatusId INT = 0;
 
      IF @employerId > 0
      BEGIN

            SELECT TOP 1
                  @employerTaxYearTransmissionStatusId = etyts.EmployerTaxYearTransmissionStatusId
            FROM EmployerTaxYearTransmission AS etyt INNER JOIN EmployerTaxYearTransmissionStatus AS etyts 
            ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
            WHERE etyt.EmployerId = @employerId
                  AND etyt.TaxYearId = @taxYearId
                  AND etyts.EntityStatusId = @entityStatusId
                  AND etyts.EndDate IS NULL 

            IF @employerTaxYearTransmissionStatusId = 0
            BEGIN
                  SELECT TOP 1
                        @employerTaxYearTransmissionStatusId = etyts.EmployerTaxYearTransmissionStatusId
                  FROM EmployerTaxYearTransmission AS etyt INNER JOIN EmployerTaxYearTransmissionStatus AS etyts 
                  ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
                  WHERE etyt.EmployerId = @employerId
                        AND etyt.TaxYearId = @taxYearId
                        AND etyts.EntityStatusId = 1
                  ORDER BY etyts.EndDate DESC
            END

      END
      ELSE
      BEGIN
      
            SELECT TOP 1
                  @employerTaxYearTransmissionStatusId = etyts.EmployerTaxYearTransmissionStatusId
            FROM EmployerTaxYearTransmission AS etyt 
                  INNER JOIN EmployerTaxYearTransmissionStatus AS etyts ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
                  INNER JOIN (SELECT employer_id FROM employer WHERE ResourceId = @employerResourceId) AS e ON e.employer_id = etyt.EmployerId        
            WHERE etyt.TaxYearId = @taxYearId
                  AND etyts.EntityStatusId = @entityStatusId
                  AND etyts.EndDate IS NULL 

            IF @employerTaxYearTransmissionStatusId = 0
            BEGIN
                  SELECT TOP 1
                        @employerTaxYearTransmissionStatusId = etyts.EmployerTaxYearTransmissionStatusId
                  FROM EmployerTaxYearTransmission AS etyt 
                        INNER JOIN EmployerTaxYearTransmissionStatus AS etyts ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
                        INNER JOIN (SELECT employer_id FROM employer WHERE ResourceId = @employerResourceId) AS e ON e.employer_id = etyt.EmployerId
                  WHERE etyt.TaxYearId = @taxYearId
                        AND etyts.EntityStatusId = @entityStatusId
                  ORDER BY etyts.EndDate DESC
            END
                  
      END
      
      SELECT
            etyt.EmployerId,
            etyts.EmployerTaxYearTransmissionStatusId,
            etyts.EmployerTaxYearTransmissionId,
            etyts.TransmissionStatusId,
            etyts.ResourceId,
            etyts.EntityStatusId,
            etyts.StartDate,
            etyts.EndDate,
            etyts.CreatedBy,
            etyts.CreatedDate,
            etyts.ModifiedBy,
            etyts.ModifiedDate
      FROM EmployerTaxYearTransmission AS etyt 
            INNER JOIN EmployerTaxYearTransmissionStatus AS etyts ON etyt.EmployerTaxYearTransmissionId = etyts.EmployerTaxYearTransmissionId
      WHERE EmployerTaxYearTransmissionStatusId = @employerTaxYearTransmissionStatusId

END
GO

GRANT EXECUTE ON [dbo].[SELECT_current_employer_tax_year_transmission_status] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INSERT_UPDATE_employer_transmission_status_queue]
      @employerTaxYearTransmissionStatusQueueId int,
      @employerTaxYearTransmissionStatusId int,
      @queueStatusId int,
      @title nvarchar(100),
      @message nvarchar(255),
      @body nvarchar(max),
      @entityStatusId int,
      @createdBy nvarchar(50),
      @modifiedBy nvarchar(50)
AS

BEGIN TRY

      SET NOCOUNT ON;

      IF @employerTaxYearTransmissionStatusQueueId <= 0
      BEGIN
            INSERT INTO dbo.[EmployerTaxYearTransmissionStatusQueue](
                  EmployerTaxYearTransmissionStatusId,
                  QueueStatusId,
                  Title,
                  [Message],
                  Body,
                  EntityStatusId,
                  CreatedBy,
                  CreatedDate,
                  ModifiedBy,
                  ModifiedDate)
            VALUES(
                  @employerTaxYearTransmissionStatusId,
                  @queueStatusId,
                  @title,
                  @message,
                  @body,
                  @entityStatusId,
                  @createdBy,
                  GETDATE(),
                  @modifiedBy,
                  GETDATE())
      END
      ELSE
      BEGIN
            UPDATE dbo.[EmployerTaxYearTransmissionStatusQueue]
            SET EmployerTaxYearTransmissionStatusId = @employerTaxYearTransmissionStatusId,
                  QueueStatusId = @queueStatusId,
                  Title = @title,
                  [Message] = @message,
                  Body = @body,
                  EntityStatusId = @entityStatusId,
                  ModifiedBy = @modifiedBy,
                  ModifiedDate = GETDATE()
            WHERE @employerTaxYearTransmissionStatusQueueId = @employerTaxYearTransmissionStatusQueueId
      END

      IF @employerTaxYearTransmissionStatusQueueId <= 0
      BEGIN
            SET @employerTaxYearTransmissionStatusQueueId = SCOPE_IDENTITY();
      END

      SELECT @employerTaxYearTransmissionStatusQueueId;

END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [dbo].[INSERT_UPDATE_employer_transmission_status_queue] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INSERT_UPDATE_employer_tax_year_transmission_status]
      @employerTaxYearTransmissionStatusId int OUTPUT,
      @employerTaxYearTransmissionId int,
      @transmissionStatusId int,
      @entityStatusId int,
      @startDate [datetime2](7),
      @endDate [datetime2](7) = null,
      @createdBy [nvarchar](50),
      @modifiedBy [nvarchar](50)
AS

BEGIN TRY

      SET NOCOUNT ON;

      IF @employerTaxYearTransmissionStatusId <= 0
            BEGIN
                  INSERT INTO dbo.[EmployerTaxYearTransmissionStatus](
                        EmployerTaxYearTransmissionId,
                        TransmissionStatusId,
                        StartDate,
                        EndDate,
                        EntityStatusId,
                        CreatedBy,
                        CreatedDate,
                        ModifiedBy,
                        ModifiedDate) 
                  VALUES (
                        @employerTaxYearTransmissionId,
                        @transmissionStatusId,
                        @startDate,
                        @endDate,
                        @entityStatusId,
                        @createdBy,
                        GETDATE(),
                        @modifiedBy,
                        GETDATE());
            END
      ELSE
            BEGIN
                  UPDATE dbo.[EmployerTaxYearTransmissionStatus]
                  SET
                        EmployerTaxYearTransmissionId = @employerTaxYearTransmissionId,
                        TransmissionStatusId = @transmissionStatusId,
                        StartDate = @startDate,
                        EndDate = @endDate,
                        EntityStatusId = @entityStatusId,
                        ModifiedBy = @modifiedBy,
                        ModifiedDate = GETDATE()
                  WHERE
                        employerTaxYearTransmissionStatusId = @employerTaxYearTransmissionStatusId    
            END
      
    IF @employerTaxYearTransmissionStatusId <= 0
      BEGIN
            SET @employerTaxYearTransmissionStatusId = SCOPE_IDENTITY();
      END

      SELECT @employerTaxYearTransmissionStatusId;

END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [dbo].[INSERT_UPDATE_employer_tax_year_transmission_status] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[INSERT_UPDATE_employer_tax_year_transmission]
      @employerTaxYearTransmissionId int OUTPUT,
      @employerId int,
      @taxYearId int,
      @entityStatusId int,
      @createdBy [nvarchar](50),
      @modifiedBy [nvarchar](50)
AS

BEGIN TRY

      SET NOCOUNT ON;

      IF @employerTaxYearTransmissionId <= 0
            BEGIN
                  INSERT INTO dbo.EmployerTaxYearTransmission(
                        EmployerId,
                        TaxYearId,
                        EntityStatusId,
                        CreatedBy,
                        CreatedDate,
                        ModifiedBy,
                        ModifiedDate) 
                  VALUES (
                        @employerId,
                        @taxYearId,
                        @entityStatusId,
                        @createdBy,
                        GETDATE(),
                        @modifiedBy,
                        GETDATE());
            END
      ELSE
            BEGIN
                  UPDATE dbo.[EmployerTaxYearTransmission]
                  SET
                        EmployerId = @employerId,
                        TaxYearId = @taxYearId,
                        EntityStatusId = @entityStatusId,
                        ModifiedBy = @modifiedBy,
                        ModifiedDate = GETDATE()
                  WHERE
                        EmployerTaxYearTransmissionId = @employerTaxYearTransmissionId
            END

      IF @employerTaxYearTransmissionId <= 0
      BEGIN
            SET @employerTaxYearTransmissionId = SCOPE_IDENTITY();
      END

      SELECT @employerTaxYearTransmissionId

END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [dbo].[INSERT_UPDATE_employer_tax_year_transmission] TO [aca-user] AS [dbo]
GO

