CREATE TABLE [dbo].[Profile.Education] (
    [ProfileEducationId]   UNIQUEIDENTIFIER NOT NULL,
    [UserId]               NVARCHAR (128)   NOT NULL,
    [SchoolUniversityId]   UNIQUEIDENTIFIER NULL,
    [SchoolUniversityName] NVARCHAR (128)   NOT NULL,
    [Major]                NVARCHAR (128)   NOT NULL,
    [StartDate]            DATE             NULL,
    [EndDate]              DATE             NULL,
    [CurrentlyLearn]       BIT              NULL,
    [Description]          NVARCHAR (512)   NULL
);

