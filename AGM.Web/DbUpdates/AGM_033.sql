USE agmsolutions_net_site
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


IF EXISTS ( SELECT * 
            FROM    sys.objects
            WHERE   object_id = OBJECT_ID(N'sp_rappgiorni')
                    AND type IN ( N'P', N'PC' ) ) 
BEGIN
    DROP PROCEDURE [dbo].[sp_rappgiorni]
END

GO


CREATE PROCEDURE sp_rappgiorni
	@userid int, 
	@month nvarchar(6)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	--DECLARE @userid int
	--SET @userid = 86

	--DECLARE @month nvarchar(6)
	--SET @month = '201611'

	DECLARE @vYear AS INT
	SET @vYear = CAST(SUBSTRING(@month, 1, 4) AS INT)
	DECLARE @vMonth AS INT
	SET @vMonth = CAST(SUBSTRING(@month, 5, 2) AS INT)
	DECLARE @sDate AS NVARCHAR(10)
	SET @sDate = CONVERT(NVARCHAR(4), @vYear) + '-' + RIGHT('0' + CONVERT(NVARCHAR(2), @vMonth), 2) + '-' + RIGHT('0' + CONVERT(NVARCHAR(2), 1), 2)
	DECLARE @vLastDays AS INT
	SET @vLastDays = DATEPART(DAY, DATEADD(dd, - (DAY(DATEADD(mm, 1, @sDate))), DATEADD(mm, 1, @sDate)))

	;WITH MonthlyCalendar AS (
		SELECT 1 AS num
		UNION ALL
		SELECT num + 1 AS num FROM MonthlyCalendar WHERE num + 1 < = @vLastDays
	)

	SELECT
	UserUngroupedDays.[Date],
	CONVERT(BIT, UserUngroupedDays.Festivity) AS Festivity,
	SUM(UserUngroupedDays.OrdinaryHours) AS OrdinaryHours,
	SUM(UserUngroupedDays.OvertimeHours) AS OvertimeHours
	FROM
	(
		SELECT
		UserDays.[Date],
		UserDays.Festivity,
		CASE WHEN UserDays.HoursType = 2 THEN 0.00 ELSE UserDays.[Hours] END AS OrdinaryHours,
		CASE WHEN UserDays.HoursType = 2 THEN UserDays.[Hours] ELSE 0.00 END AS OvertimeHours
		FROM
		(
			SELECT
			CONVERT(DATE, CONVERT(NVARCHAR(4), @vYear) + '-' + RIGHT('0' + CONVERT(NVARCHAR(2), @vMonth), 2) + '-' + RIGHT('0' + CONVERT(NVARCHAR(2), num), 2)) AS [Date],
			CASE WHEN NOT rappfestivi.idgiorno IS NULL AND rappfestivi.isDeleted = 0 THEN 1 ELSE 0 END AS Festivity,
			ISNULL(UserHours.ore, 0.00) AS [Hours],
			ISNULL(UserHours.idcausale, 0) AS [HoursType]
			FROM MonthlyCalendar LEFT OUTER JOIN
			rappfestivi ON CONVERT(NVARCHAR(4), @vYear) + '-' + RIGHT('0' + CONVERT(NVARCHAR(2), @vMonth), 2) + '-' + RIGHT('0' + CONVERT(NVARCHAR(2), num), 2) = rappfestivi.[date]
			LEFT OUTER JOIN
			(
				SELECT
				rappore.giorno,
				CONVERT(DECIMAL, REPLACE(rappore.ore, ',', '.')) AS ore,
				rappore.idcausale
				FROM rappore 
				WHERE idutente = @userid
				AND anno = @vYear
				AND mese = @vMonth
			) AS UserHours ON num = UserHours.giorno
		) AS UserDays
	) AS UserUngroupedDays
	GROUP BY
	UserUngroupedDays.[Date],
	UserUngroupedDays.Festivity
	ORDER BY UserUngroupedDays.[Date];

END
GO