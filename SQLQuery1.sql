set ansi_nulls on
go
set ansi_padding on
go
set quoted_identifier on 
go

create database ChinaStock
go

use ChinaStock
go

create table UserRole
(
	ID_Role int not null identity(1,1),
	Role_Name varchar(max) not null,
	constraint PK_Role primary key clustered (ID_Role ASC) on [PRIMARY]
)

create table [User]
(
	ID_User int not null identity(1,1),
	[Phone_Number] varchar(20) not null,
	[TG_Link] varchar(max) not null,
	[Password] varchar(max) not null,
	Role_ID int not null,
	constraint PK_User primary key clustered (ID_User ASC) on [PRIMARY],
	constraint CH_Password check ([Password] like '%[0-9]%' and [Password] like '%[A-Z]%' and [Password] like '%[!@#$%a^&*()]%' and len([Password]) >= 8),
	constraint FK_Role foreign key (Role_ID) references UserRole(ID_Role)
)

alter table [User] add Identical_Number varchar(5) not null unique

create table [Package]
(
	ID_Package int not null identity(1,1),
	Package_Name varchar(max) not null,
	Product_Art varchar(20) not null,
	Product_Link varchar(max) not null,
	Product_Count int not null,
	Product_Price float not null,
	constraint PK_Package primary key clustered (ID_Package ASC) on [PRIMARY]
)

alter table Package add TrackNumber varchar(20) not null unique

alter table Package add TypeDelivery varchar(max) not null

alter table Package add IsFinished bit not null

select * from [User]


create table [Packages_Of_Users]
(
	ID_Package_User int not null identity(1,1),
	Package_ID int not null,
	[User_ID] int not null,
	Identical_Number varchar(10) not null,
	constraint PK_Packages_Of_Users primary key clustered (ID_Package_User ASC) on [PRIMARY]
)

delete from [Packages_Of_Users]


insert into UserRole values ('Пользователь'), ('Администратор')

insert into [User] values ('89999999999', 'https://t.me/blyadovv', 'test12345!', 1, '12345')
insert into [User] values ('89999999997', 'https://t.me/blyadovv', 'test1234!', 2, '11111')

select * from [User]

create trigger PackageDelete on Package
instead of delete
as
begin
	declare @IdTodelete int
	set @IdToDelete = (select [ID_Package] from [deleted])
	delete from Packages_Of_Users where Package_ID = @IdTodelete
	delete from Package where ID_Package = @IdTodelete
end