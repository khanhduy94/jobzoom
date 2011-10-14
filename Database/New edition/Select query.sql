
Select TagName
from dbo.Tag
where ParentID IN (select id from dbo.Tag where TagName='Industry') order by TagName;

Select TagName
from dbo.Tag
where ParentID IN (select id from dbo.Tag where TagName='Function') order by TagName;

Select TagName
from dbo.Tag
where ParentID IN (select id from dbo.Tag where TagName='Language') order by TagName;

Select TagName
from dbo.Tag
where ParentID IN (select id from dbo.Tag where TagName='Job Type') order by TagName;

Select TagName
from dbo.Tag
where ParentID IN (select id from dbo.Tag where TagName='Job Experience') order by TagName;

insert into Tag values(NEWID(), '8cc8d547-f013-401d-8a92-c242f7951bfb', 'Graphic design', 'D7656123-4F37-4223-BE2C-FC6DA95E1399');
insert into Tag values(NEWID(), '8cc8d547-f013-401d-8a92-c242f7951bfb', 'HTML/CSS', 'D7656123-4F37-4223-BE2C-FC6DA95E1399');
insert into Tag values(NEWID(), '8cc8d547-f013-401d-8a92-c242f7951bfb', 'Web Programming', 'D7656123-4F37-4223-BE2C-FC6DA95E1399');
insert into Tag values(NEWID(), '8cc8d547-f013-401d-8a92-c242f7951bfb', 'Front Page', NULL);

insert into Tag values(NEWID(), '98491a22-5455-4901-822c-6b72da8f04ec', 'Testing', NULL);
insert into Tag values(NEWID(), '98491a22-5455-4901-822c-6b72da8f04ec', 'Test case', NULL);
insert into Tag values(NEWID(), '98491a22-5455-4901-822c-6b72da8f04ec', 'Black box testing', NULL);
insert into Tag values(NEWID(), '98491a22-5455-4901-822c-6b72da8f04ec', 'HTML/CSS', NULL);

select * from Tag where ParentID IS NULL;

SELECT Tag.TagName, t1.TagName Tag
FROM Tag
INNER JOIN Tag AS t1 ON Tag.ID = t1.ParentID

select Job.ID, Job.JobTitle, Tag.TagName, t1.TagName as Parent
from Job
inner join Tag on Job.ID = Tag.ObjectID
left JOIN Tag AS t1 ON Tag.ParentID = t1.ID

--Pivot transformation Basic Example
SELECT * FROM
(
	select Job.ID, Job.JobTitle, Tag.TagName
	from Job
	inner join Tag on Job.ID = Tag.ObjectID
	Group by Job.ID, Job.JobTitle, Tag.TagName
)t
PIVOT (COUNT(TagName) FOR TagName
IN ([Web Programming],[HTML/CSS])) AS pvt

--Pivot dynamic columns
DECLARE @listCol VARCHAR(2000)
DECLARE @query VARCHAR(4000)
SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
                        FROM Tag
                        WHERE Tag.ObjectID IS NOT NULL
                        ORDER BY '],[' + ltrim(TagName)
                        FOR XML PATH('')), 1, 2, '') + ']'
 
SET @query = 'SELECT * FROM (
					select Job.ID, Job.JobTitle, Tag.TagName
					from Job
					inner join Tag on Job.ID = Tag.ObjectID
					Group by Job.ID, Job.JobTitle, Tag.TagName
				)t
				PIVOT (COUNT(TagName) FOR TagName
				IN ('+@listCol+')) AS pvt'
EXECUTE (@query)




