select * from Tag where ParentID IS NULL;

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
where ParentID IN (select id from dbo.Tag where TagName='Experience') order by TagName;

insert into [User](ID,Email,Password,ModifiedDate) values(NEWID(), 'example@live.com','123456','2011/10/10'); 

--Profile
insert into Tag values(NEWID(), '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8', 'Developer', 'BDD12E21-6DDB-41B8-891E-30FF2D25EE62');
insert into TagDetail values(NEWID(),'5D0D5DD2-F11F-4466-BA62-A052A6299485', 'FPT', '2008/7/10','2010/9/10', null, null);

insert into Tag values(NEWID(), '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8', 'Tester', 'BDD12E21-6DDB-41B8-891E-30FF2D25EE62');
insert into TagDetail values(NEWID(),'8C287CB6-A9DA-4419-8351-66D22A82B5F9', 'Gameloft', '2005/7/10','2007/9/10', null, null);

insert into Tag(ID, ObjectID, TagName, ParentID) values(NEWID(), '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8', 'Graphic design', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag(ID, ObjectID, TagName, ParentID) values(NEWID(), '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8', 'HTML/CSS', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag(ID, ObjectID, TagName, ParentID) values(NEWID(), '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8', 'Web Programming', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag(ID, ObjectID, TagName, ParentID) values(NEWID(), '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8', 'Testing', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');


--Job

insert into Job values('4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', '55CD50DE-6A7B-4C9A-8E0F-C0CDF37986F7', 'Tester', null, '2011/10/10');
insert into Job values('D27E4FCB-45DE-4399-9F81-4D6AB28853DD', '55CD50DE-6A7B-4C9A-8E0F-C0CDF37986F7', 'Web Developer', null, '2011/10/10');
insert into Job values(NEWID(), '55CD50DE-6A7B-4C9A-8E0F-C0CDF37986F7', 'Sales', null, '2011/10/10');

insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Graphic design', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'HTML/CSS', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Web Programming', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Front Page', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Vb.Net', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5',GETDATE());
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Developer', 'BDD12E21-6DDB-41B8-891E-30FF2D25EE62',GETDATE());

insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'Testing', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'Test case', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'Black box testing', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'HTML/CSS', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');

select Job.ID, Job.JobTitle, Tag.TagName, t1.TagName as Parent
from Job
inner join Tag on Job.ID = Tag.ObjectID
left JOIN Tag AS t1 ON Tag.ParentID = t1.ID
Group by Job.ID, Job.JobTitle, Tag.TagName, t1.TagName

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
--Job
DECLARE @listCol VARCHAR(2000)
DECLARE @query VARCHAR(4000)
SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
                        FROM Tag
                        inner join Job on Job.ID = Tag.ObjectID
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

--Profile
select [Profile].ID, [Profile].FirstName, Tag.TagName, t1.TagName as Parent
from [Profile]
inner join Tag on [Profile].ID = Tag.ObjectID
left JOIN Tag AS t1 ON Tag.ParentID = t1.ID
Group by [Profile].ID, [Profile].FirstName, Tag.TagName, t1.TagName

select [Profile].ID, TagName, Location, StartDate, EndDate 
from [Profile] 
inner join Tag on Tag.ObjectID = [Profile].ID
inner join TagDetail on Tag.ID = TagDetail.TagID
order by StartDate

DECLARE @listCol VARCHAR(2000)
DECLARE @query VARCHAR(4000)
SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
                        FROM Tag
                        inner join [Profile] on [Profile].ID = Tag.ObjectID
                        ORDER BY '],[' + ltrim(TagName)
                        FOR XML PATH('')), 1, 2, '') + ']'
 
SET @query = 'SELECT * FROM (
					select [Profile].ID, [Profile].FirstName, Tag.TagName
					from [Profile]
					inner join Tag on [Profile].ID = Tag.ObjectID
					Group by [Profile].ID, [Profile].FirstName, Tag.TagName
				)t
				PIVOT (COUNT(TagName) FOR TagName
				IN ('+@listCol+')) AS pvt'
EXECUTE (@query)

Exec GetPivotJobRequirement

Exec GetPivotProfile