create table FullAddress (
  Id int primary key identity(1,1) NOT NULL,
  City varchar(100) not null,
  Town varchar(100) not null,
  Street varchar(100) null,
  Code int not null
);
create table SystemUser (
  Id int primary key identity(1,1) NOT NULL,
  Username varchar(100) NOT NULL,
  LastActive date NOT NULL,
  UserPassword varchar(200) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()) 
);
create table Book (
  Id int  primary key identity(1,1) NOT NULL, 
  Name varchar(100) NOT NULL,
  ISBN varchar(100) NOT NULL,
  Description varchar(200) NOT NULL,
  Author varchar(100) NOT NULL,
  Price varchar(10) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  Status int NOT NULL DEFAULT(0) 
);
create table Course (
  Id int  primary key identity(1,1) NOT NULL,
  Name varchar(100) NOT NULL,
  Duration varchar(100) NOT NULL,
  Cost varchar(10) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  Status int NOT NULL DEFAULT(0) 
);
create table BookCourse (
  Id int  primary key identity(1,1) NOT NULL,
  CourseId int NOT NULL,
  BookId int NOT NULL,
  Status int NOT NULL DEFAULT(0),
  AddDate datetime DEFAULT(GETDATE()),
  FOREIGN KEY (CourseId) REFERENCES Course(Id),
  FOREIGN KEY (BookId) REFERENCES Book(Id) 
);
create table Student (
  Id int  primary key identity(1,1) NOT NULL,
  UserId int not null, 
  Name varchar(100) NOT NULL,
  Surname varchar(100) NOT NULL,
  Birthday date NOT NULL,
  Gender int NOT NULL, 
  BloodGroup varchar(100) NOT NULL,
  AddressId int NOT NULL,
  ContactNumber varchar(20) NOT NULL,
  Email varchar(100) NOT NULL,
  StudentNumber varchar(15) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  Status int NOT NULL DEFAULT(0),
  FOREIGN KEY (UserId) REFERENCES SystemUser(Id) ,
  FOREIGN KEY (AddressId) REFERENCES FullAddress(Id)
);
create table Subject (
  Id int  primary key identity(1,1) NOT NULL,
  Name varchar(100) NOT NULL,
  Duration varchar(100) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  Status int NOT NULL DEFAULT(0) 
);
create table CourseStudent (
  Id int  primary key identity(1,1) NOT NULL,
  StudentId int NOT NULL,
  CourseId int NOT NULL,
  StartDate date NOT NULL,
  EndDate date DEFAULT NULL,
  Status int NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  FOREIGN KEY (StudentId) REFERENCES Student(Id),
  FOREIGN KEY (CourseId) REFERENCES Course(Id) 
);
create table CourseSubject (
  Id int  primary key identity(1,1) NOT NULL,
  CourseId int NOT NULL,
  SubjectId int NOT NULL,
  Status int NOT NULL,
  AddDate datetime DEFAULT(GETDATE()), 
  FOREIGN KEY (CourseId) REFERENCES Course(Id),
  FOREIGN KEY (SubjectId) REFERENCES Subject(Id) 
);
create table AssessmentType (
  Id int  primary key identity(1,1) NOT NULL,
  Name varchar(100) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  Status int NOT NULL DEFAULT(0) 
); 
create table Mark (
  StudentId int NOT NULL,
  Id int  primary key identity(1,1) NOT NULL,
  SubjectId int NOT NULL,
  AssessmentTypeId int NOT NULL,
  AssessmentDate datetime NOT NULL,
  ObtainedMark decimal(10,0) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  Status int NOT NULL DEFAULT(0),
  FOREIGN KEY (SubjectId) REFERENCES Subject(Id),
  FOREIGN KEY (AssessmentTypeId) REFERENCES AssessmentType(Id),
  FOREIGN KEY (StudentId) REFERENCES Student(Id) 
);
create table Parent (
  Id int  primary key identity(1,1) NOT NULL,
  UserId int not null,
  AddressId int not null,
  Name varchar(100) NOT NULL,
  Email varchar(100) NOT NULL,
  ContactNumber varchar(15) NOT NULL,
  Address varchar(100) NOT NULL,
  Profession varchar(100) DEFAULT NULL,
  Status int NOT NULL DEFAULT(0),
  FOREIGN KEY (UserId) REFERENCES SystemUser(Id),
  FOREIGN KEY (AddressId) REFERENCES FullAddress(Id)
);
create table ParentStudent (
  Id int  primary key identity(1,1) NOT NULL,
  StudentId int NOT NULL,
  ParentId int NOT NULL,
  Relationship varchar(50) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  Status int NOT NULL DEFAULT(0),
  FOREIGN KEY (StudentId) REFERENCES Student(Id),
  FOREIGN KEY (ParentId) REFERENCES Parent(Id)
);
create table Teacher (
  Id int  primary key identity(1,1) NOT NULL,
  UserId int not null,
  AddressId int not null,
  Name varchar(100) NOT NULL,
  Birthday date NOT NULL,
  Gender int NOT NULL,
  ReligionId int NOT NULL,
  Address varchar(100) NOT NULL,
  ContactNumber varchar(15) NOT NULL,
  Email varchar(100) NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  Status int NOT NULL DEFAULT(0), 
  FOREIGN KEY (UserId) REFERENCES SystemUser(Id),
  FOREIGN KEY (AddressId) REFERENCES FullAddress(Id)
);
create table TeacherSubject (
  Id int  primary key identity(1,1) NOT NULL,
  TeacherId int NOT NULL,
  SubjectId int NOT NULL,
  StartDate datetime NOT NULL,
  EndDate datetime DEFAULT NULL,
  Status int NOT NULL,
  AddDate datetime DEFAULT(GETDATE()),
  FOREIGN KEY (TeacherId) REFERENCES Teacher(Id),
  FOREIGN KEY (SubjectId) REFERENCES Subject(Id) 
);