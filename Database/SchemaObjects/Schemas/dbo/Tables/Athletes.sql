CREATE TABLE [dbo].[Athletes]
(
    [Id] [bigint] NOT NULL PRIMARY KEY IDENTITY(1,1),
    [FirstName] [nvarchar](256),
    [LastName] [nvarchar](256),
    [Position] [nvarchar](256)
)
