
Select Tag
from dbo.Tag
where ParentID IN (select id from dbo.Tag where Tag='Industry') order by Tag;

Select Tag
from dbo.Tag
where ParentID IN (select id from dbo.Tag where Tag='Function') order by Tag;

Select Tag
from dbo.Tag
where ParentID IN (select id from dbo.Tag where Tag='Language') order by Tag;

Select Tag
from dbo.Tag
where ParentID IN (select id from dbo.Tag where Tag='Job Type') order by Tag;

Select Tag
from dbo.Tag
where ParentID IN (select id from dbo.Tag where Tag='Job Experience') order by Tag;


insert into Tag values(NEWID(), '8cc8d547-f013-401d-8a92-c242f7951bfb', 'Graphic design', 'D7656123-4F37-4223-BE2C-FC6DA95E1399');
insert into Tag values(NEWID(), '8cc8d547-f013-401d-8a92-c242f7951bfb', 'HTML/CSS', 'D7656123-4F37-4223-BE2C-FC6DA95E1399');
insert into Tag values(NEWID(), '8cc8d547-f013-401d-8a92-c242f7951bfb', 'Web Programming', 'D7656123-4F37-4223-BE2C-FC6DA95E1399');

select * from Tag where ParentID IS NULL;

select Job.ID, Job.JobTitle, dbo.Tag.Tag
from Job
inner join Tag on Job.ID = Tag.ObjectID;

