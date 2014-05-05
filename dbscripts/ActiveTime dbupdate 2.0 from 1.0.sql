--begin transaction;

-- -------------------------------------------------------
-- comments table
-- -------------------------------------------------------

-- rename current table
alter table [comments] rename to [comments_old];

-- create new table with correct structure
CREATE TABLE [comments] (
  [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  [date] DATE NOT NULL UNIQUE,
  [comment] VARCHAR(1024) NOT NULL);

-- import data from old table
insert into [comments]([id], [date], [comment])
select [id], [date], [comment]
from [comments_old];

-- drop old table
drop table [comments_old];


-- -------------------------------------------------------
-- records table
-- -------------------------------------------------------

-- rename current table
alter table [records] rename to [records_old];

-- create new table with correct structure
CREATE TABLE [records] (
  [id] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  [date] DATE NOT NULL,
  [start_time] TIME NOT NULL,
  [end_time] TIME NOT NULL,
  [type] INTEGER NOT NULL,
  UNIQUE([date], [start_time], [end_time]));

-- import data from old table
insert into [records]([id], [date], [start_time], [end_time], [type])
select [id], [date], [start_time], [end_time], 0
from [records_old];

-- drop old table
drop table [records_old];


-- -------------------------------------------------------
-- dbinfo table
-- -------------------------------------------------------

-- create new version table
CREATE TABLE [dbinfo] (
  [application] VARCHAR(64),
  [author] VARCHAR(64),
  [version] VARCHAR(32));

-- add dbinfo data
insert into [dbinfo] ([application], [author], [version])
values ("Active Time", "Dust in the Wind", "2.0");

-- -------------------------------------------------------
-- end
-- -------------------------------------------------------

--commit;