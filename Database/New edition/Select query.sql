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

insert into Tag values(NEWID(), '62643156-3660-400F-A733-3EC98ED9B769', 'IT', 'B67DB5C2-5891-4DF9-88CF-3D502E9B499B',GETDATE());
insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'IT', 'B67DB5C2-5891-4DF9-88CF-3D502E9B499B',GETDATE());

insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'HTML/CSS', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Web Programming', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Front Page', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Vb.Net', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5',GETDATE());
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Developer', 'BDD12E21-6DDB-41B8-891E-30FF2D25EE62',GETDATE());
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'MVC', 'BDD12E21-6DDB-41B8-891E-30FF2D25EE62',GETDATE());
insert into Tag values(NEWID(), 'D27E4FCB-45DE-4399-9F81-4D6AB28853DD', 'Silverlight', 'BDD12E21-6DDB-41B8-891E-30FF2D25EE62',GETDATE());

insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'Testing', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'Test case', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'Black box testing', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');
insert into Tag values(NEWID(), '4ECC4CDC-EE7A-44A5-82EB-0B3DEBF22794', 'HTML/CSS', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5');


insert into Tag values(NEWID(), 'E5BDE8B6-2719-4C78-81F0-16EA5EF91DB0', 'Computer', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5', GETDATE());
insert into Tag values(NEWID(), 'E5BDE8B6-2719-4C78-81F0-16EA5EF91DB0', 'Market research', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5', GETDATE());
insert into Tag values(NEWID(), 'E5BDE8B6-2719-4C78-81F0-16EA5EF91DB0', 'Customer service', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5', GETDATE());
insert into Tag values(NEWID(), 'E5BDE8B6-2719-4C78-81F0-16EA5EF91DB0', 'Communication', 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5', GETDATE());

DELETE Tag WHERE ObjectID IS NOT NULL AND ObjectID <> '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8'
DELETE JobApproval
DELETE Job

select JobTitle, Tag.TagName, Tag2.TagName from job 
inner join Tag on job.id = tag.ObjectID
inner join Tag as Tag2 on Tag2.ID = Tag.ParentID

DECLARE @ID char(36)
DECLARE @Industry char(36)
DECLARE @Skill char(36)
set @ID = NEWID()
set @Industry = 'D7656123-4F37-4223-BE2C-FC6DA95E1399'
set @Skill = 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5'
insert into Job values(@ID, '55CD50DE-6A7B-4C9A-8E0F-C0CDF37986F7', 'Developer', null, GETDATE());
insert into Tag values(NEWID(), @ID, 'Computer Software', @Industry, GETDATE());
insert into Tag values(NEWID(), @ID, '.NET Framework', @Skill, GETDATE());
insert into Tag values(NEWID(), @ID, 'C/C++', @Skill, GETDATE());
insert into Tag values(NEWID(), @ID, 'DBMS', @Skill, GETDATE());
insert into Tag values(NEWID(), @ID, 'ASP.NET', @Skill, GETDATE());
insert into JobApproval values(NEWID(), '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8', @ID, 1, 0, GETDATE())


DECLARE @jobID char(36),
		@profileID char(36),
		@Major char(36),
		@Skill char(36),
		@ID char(36);
set @ID = NEWID()
set @Major = 'B67DB5C2-5891-4DF9-88CF-3D502E9B499B'
set @Skill = 'E38556A2-9FA3-4783-8EDE-07DFF1EBA7A5'
SET @profileID = '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8'
DECLARE job_cursor CURSOR FOR 
SELECT Job.ID
FROM Job
INNER JOIN Tag ON Tag.ObjectID = Job.ID
WHERE Tag.TagName = 'MBA';

OPEN job_cursor;
FETCH NEXT FROM job_cursor 
INTO @jobID;

WHILE @@FETCH_STATUS = 0
BEGIN

	insert into Tag values(NEWID(), @jobID, '.NET', @Skill, GETDATE());
	--update JobApproval set IsApproved = 1
	--where JobID = @jobID	

FETCH NEXT FROM job_cursor 
    INTO @jobID;
END
CLOSE job_cursor;
DEALLOCATE job_cursor;

Exec GetPivotJobRequirement


--insert into Tag values(NEWID(), @ID, '.Net', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'Problem solving', @Skill, GETDATE());
insert into Tag values(NEWID(), @ID, 'C++', @Skill, GETDATE());
insert into Tag values(NEWID(), @ID, 'Java', @Skill, GETDATE());
insert into Tag values(NEWID(), @ID, 'Android', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'C#', @Skill, GETDATE());
insert into Tag values(NEWID(), @ID, 'ASP.Net', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'Silverlight', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'MVC', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'Database', @Skill, GETDATE());




--insert into Tag values(NEWID(), @ID, 'Graphic design', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'Computer Science', @Major, GETDATE());

insert into Tag values(NEWID(), @ID, 'C++', @Skill, GETDATE());
insert into Tag values(NEWID(), @ID, 'Objective-C', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'C#', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'Video processing', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'Problem solving', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'Hardware debug', @Skill, GETDATE());
--insert into Tag values(NEWID(), @ID, 'Software driver development', @Skill, GETDATE());


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
 
SET @query = 'CREATE VIEW PivotProfile
				AS
				SELECT * FROM (
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

DECLARE @jobID char(36),
		@profileID char(36);

SET @profileID = '44CD50DE-8F7B-4C9A-8E0F-C0CDF37986F8'
DECLARE job_cursor CURSOR FOR 
SELECT ID
FROM Job;

OPEN job_cursor;

FETCH NEXT FROM job_cursor 
INTO @jobID;

WHILE @@FETCH_STATUS = 0
BEGIN
	insert into JobApproval values(NEWID(), @profileID, @jobID, 0, 1, GETDATE())

FETCH NEXT FROM job_cursor 
    INTO @jobID;
END
CLOSE job_cursor;
DEALLOCATE job_cursor;