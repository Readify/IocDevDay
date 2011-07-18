CREATE TABLE [tinysheets].[TimeEntry]
(
	TimeEntryId INT PRIMARY KEY IDENTITY,
	[Description] VARCHAR(256) NOT NULL,
	[Hours] FLOAT NOT NULL,
	[IsApproved] BIT NOT NULL
)
