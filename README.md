# CDRInfo
API to
- Uploading new CDR files.
- Retrieve individual CDR by the CDR Reference.
- Retrieve all CDRs for a specific Caller ID.
- Retrieve N most expensive calls, in GBP, for a specific Caller ID.

Current Version
The current version is 0.1

A database called CDRinfo, table calledtechtest_cdr_dataset,  a view called CDRData and a stored procedure called  spInsertCDRData are used in this projct.

Below are the Sql code to create them.

```
CREATE DATABASE [CDRinfo]
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO


CREATE TABLE [dbo].[techtest_cdr_dataset](
	[caller_id] [varchar](50) NULL,
	[recipient] [varchar](50) NULL,
	[call_date] [varchar](50) NULL,
	[end_time] [varchar](50) NULL,
	[duration] [varchar](50) NULL,
	[cost] [varchar](50) NULL,
	[reference] [varchar](50) NOT NULL,
	[currency] [varchar](3) NULL,
	[type] [varchar](1) NULL,
 CONSTRAINT [PK_techtest_cdr_dataset] PRIMARY KEY CLUSTERED 
(
	[reference] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE PROCEDURE [dbo].[spInsertCDRData]
	-- Add the parameters for the stored procedure here
	@caller_id varchar(50),
	@recipient varchar(50),
	@call_date varchar(50),
        @end_time varchar(50),
        @duration varchar(50),
        @cost	  varchar(50),
        @reference varchar(50),
        @currency varchar(50),
        @type	  varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    insert into techtest_cdr_dataset (
    [caller_id],
    [recipient],
    [call_date],
    [end_time],
    [duration],
    [cost],
    [reference],
    [currency],
    [type]
    )
	SELECT
	nullif(@caller_id,''),
	nullif(@recipient,''),
	nullif(@call_date,''),
	nullif(@end_time ,''), 
	nullif(@duration ,''), 
	nullif(@cost	 ,''),	  
	nullif(@reference,''),
	nullif(@currency ,''), 
	nullif(@type	 ,'')	  


END
GO

CREATE VIEW [dbo].[CDRData]
AS
select 
       isnull([caller_id],'') [caller_id]
      ,[recipient]
      ,case when TRY_PARSE([duration] as int) is not null
      then  FORMAT (DATEADD(second, TRY_PARSE([duration] as int) * -1, DATEADD(day, DATEDIFF(day,'19000101',try_convert(datetime,call_date,103) ), CAST(end_time AS DATETIME2(7)))), 'dd/MM/yyyy HH:mm:ss')
      else FORMAT (try_convert(datetime,call_date,103), 'dd/MM/yyyy HH:mm:ss')   
      end  [call_date]
      ,FORMAT (DATEADD(day, DATEDIFF(day,'19000101',try_convert(datetime,call_date,103) ), CAST(end_time AS DATETIME2(7))), 'dd/MM/yyyy HH:mm:ss') [end_time]
      ,case when TRY_PARSE([duration] as int) is not null
      then TRY_PARSE([duration] as int)
      else 0 
      end [duration]
      ,cast([cost] as decimal(6,3)) [cost]
      ,[reference]
      ,[currency]
      ,[type]
from [techtest_cdr_dataset]
GO

```

## To test the API

## From PowerShell use the following commands use the API

### To upload a file
```
$file_contents = Get-Item techtest_cdr_dataset.csv
Invoke-RestMethod -uri http://localhost:63570/api/File/Upload -Method Post -Form @{ file = $file_contents }
```

### To get all calls from a caller id
`Invoke-RestMethod -uri http://localhost:63570/api/cdr?callerId=4.41773E%2B11 -Method Get`

Note + need to be HTML encoded to %2B

### To get a call by reference
`Invoke-RestMethod -uri http://localhost:63570/api/cdr?reference=C000776B253F360AF7EA37632D8FEB98C -Method Get`

### To get most expensive 5 calls from a caller id
`Invoke-RestMethod -uri Invoke-RestMethod -uri http://localhost:63570/api/mostexpensive?callerid=4.42036E%2B11 -Method Get`

Note + need to be HTML encoded to %2B

## Using Web browser use the following URI
###To get most expensive 2 calls from a caller id
`http://localhost:63570/api/mostexpensive?callerid=4.42036E%2B11&&number=2`

Note + need to be HTML encoded to %2B
