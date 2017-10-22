Use baza2

CREATE TABLE City (
	CityId SMALLINT CONSTRAINT pk_City PRIMARY KEY IDENTITY,
	CityName NVARCHAR(20) 
);
INSERT INTO City (CityName) VALUES 
('Wroc³aw') , ('Warszawa') , ('Kraków'), ('Poznañ'),('£ódŸ');
GO

CREATE TABLE UserRole (
	UserRoleId SMALLINT CONSTRAINT pk_Role PRIMARY KEY,
	RoleName NVARCHAR(30) CONSTRAINT notnull_User_Role_TypeRole NOT NULL
);
INSERT INTO UserRole
VALUES (1, 'Administrator'), (2, 'Stomatolog'), (3, 'Recepcjonistka'), (4, 'Pacjent');
GO

CREATE TABLE Gender (
	GenderId SMALLINT CONSTRAINT pk_Gender PRIMARY KEY,
	GenderName NVARCHAR(10) CONSTRAINT notnull_Gender_GenderName NOT NULL 
);
INSERT INTO Gender
VALUES (0, 'Nieznana'), (1, 'Kobieta'), (2, 'Mê¿czyzna');
GO


CREATE TABLE Account (
	AccountId INT CONSTRAINT pk_Account PRIMARY KEY IDENTITY,
	AccountLogin NVARCHAR(30) CONSTRAINT notnull_AccountLogin NOT NULL CONSTRAINT unique_Account_Login UNIQUE,
	Email NVARCHAR(30) CONSTRAINT notnull_Account_Email NOT NULL CONSTRAINT unique_Account_Email UNIQUE,
	AccountPassword CHAR(32) CONSTRAINT notnull_Account_AccountPassword NOT NULL,
	InsuranceNumber NVARCHAR(20) CONSTRAINT notnull_Account_InsuranceNumber NOT NULL,
	Pesel NVARCHAR(15) CONSTRAINT notnull_Account_Pesel NOT NULL,
	FirstName NVARCHAR(20) CONSTRAINT notnull_Account_FirstName NOT NULL,
	LastName NVARCHAR(30) CONSTRAINT notnull_Account_LastName NOT NULL,
	[Address] NVARCHAR(70) CONSTRAINT notnull_Account_Address NOT NULL,
	PostalCode NVARCHAR(6) CONSTRAINT notnull_Account_PostalCode NOT NULL,
	Phone NVARCHAR(12) CONSTRAINT notnull_Account_Phone NOT NULL,
	CityId SMALLINT NOT NULL REFERENCES City,
	GenderId SMALLINT REFERENCES Gender,
	UserRoleId SMALLINT REFERENCES UserRole
);
INSERT INTO [dbo].[Account]
           ([AccountLogin]
           ,[Email]
           ,[AccountPassword]
		   ,[InsuranceNumber]
		   ,[Pesel]
           ,[FirstName]
           ,[LastName]
		   ,[Address]
		   ,[PostalCode]
		   ,[Phone]
           ,[CityId]
           ,[GenderId]
           ,[UserRoleId])
     VALUES
           (
		   'admin'
           ,'204406@student.pwr.edu.pl'
           ,'21232f297a57a5a743894a0e4a801fc3'
		   ,'00514387/222'
		   ,'92080811751'
           ,'Micha³'
           ,'Stê¿a³a'
		   ,'Kamienna 12'
		   ,'50-131'
		   ,'536259249'
           ,1
           ,1
           ,1);
GO



CREATE TABLE TimeTable
(
	TimeTableId INT CONSTRAINT pk_TimeTable PRIMARY KEY IDENTITY,
	AccountId INT NOT NULL REFERENCES Account,
	Active BIT,
	[DayOfWeek] SMALLINT,
	[Time] SMALLINT
); 


INSERT INTO TimeTable(AccountId,Active,[DayOfWeek],[Time])
				VALUES(101,1,1,9) ,(101,1,1,10) ,(101,1,1,11) ,(101,1,1,12)
					  ,(102,1,1,11) ,(102,1,1,12) ,(102,1,1,13) ,(102,1,1,14) ,(102,1,1,15) 
					  ,(103,1,1,16) ,(103,1,1,17) ,(103,1,1,18) ,(103,1,1,19) ,(103,1,1,20) 
					  
					  ,(101,1,2,9) ,(101,1,2,10) ,(101,1,2,11) ,(101,1,2,12)
					  ,(102,1,2,11) ,(102,1,2,12) ,(102,1,2,13) ,(102,1,2,14) ,(102,1,2,15) 
					  ,(103,1,2,16) ,(103,1,2,17) ,(103,1,2,18) ,(103,1,2,19) ,(103,1,2,20)

					  ,(101,1,3,9) ,(101,1,3,10) ,(101,1,3,11) ,(101,1,3,12)
					  ,(102,1,3,11) ,(102,1,3,12) ,(102,1,3,13) ,(102,1,3,14) ,(102,1,3,15) 
					  ,(103,1,3,16) ,(103,1,3,17) ,(103,1,3,18) ,(103,1,3,19) ,(103,1,3,20)

					  ,(101,1,4,9) ,(101,1,4,10) ,(101,1,4,11) ,(101,1,4,12),(101,1,4,16),(101,1,4,17),(101,1,4,18), (101,1,4,19), (101,1,4,20)
					  ,(102,1,4,16) ,(102,1,4,17) ,(102,1,4,18) ,(102,1,4,19) ,(102,1,4,20) 

					  ,(101,1,5,9) ,(101,1,5,10) ,(101,1,5,11) ,(101,1,5,12),(101,1,5,16),(101,1,5,17),(101,1,5,18), (101,1,5,19), (101,1,5,20)
					  ,(102,1,5,16) ,(102,1,5,17) ,(102,1,5,18) ,(102,1,5,19) ,(102,1,5,20); 


CREATE TABLE VisitState
(
	VisitStateId INT CONSTRAINT pk_VisitState PRIMARY KEY IDENTITY,
	VisitStateName NVARCHAR(20)
);
INSERT INTO VisitState(VisitStateName) VALUES ('Utworzona') , ('Zatwierdzona') , ('Zrealizowana') , ('Anulowana');

CREATE TABLE Visit
(
	VisitId INT CONSTRAINT pk_Visit PRIMARY KEY IDENTITY,
	DentistId INT NOT NULL REFERENCES Account,
	PateintId Int  NULL REFERENCES Account,
	VisitStateId INT NOT NULL REFERENCES VisitState,
	DateOfVisit Date,
	TimeOfVisit INT,
	MakeServices NVARCHAR(100),
	Price money,
	RecipeId  int REFERENCES Recipe
);


CREATE TABLE SavePatient2Visit
(
	SavePatient2VisitId int primary key identity,
	PatientId int not null references Account,
	VisitId int not null references Visit,
	Cost money
)

CREATE TABLE Recipe
(
  RecipeId  int NOT NULL primary key IDENTITY , 
  Medicamente varchar(255), 
  isFree bit 
  );
 
 CREATE TABLE PatientCard 
 (
	PatientCardId   int PRIMARY KEY IDENTITY NOT NULL, 
	PateintId int not null References Account,
	Descryption varchar(255) 
 );

 




	CREATE PROCEDURE CreatePatient
	AS
	DECLARE @counter INT = 1;
	DECLARE @id NVARCHAR(20);

	BEGIN
		WHILE @counter <13
		BEGIN
			SET @id = 'patient' + CAST (@counter as NVARCHAR);
			
			INSERT INTO Account([AccountLogin],[Email],[AccountPassword],[InsuranceNumber],[Pesel],[FirstName],[LastName], [Address] ,[PostalCode] ,[Phone] ,[CityId] ,[GenderId] ,[UserRoleId] )
			 VALUES
				   (
				   @id
				   ,@id + '@.pwr.edu.pl'
				   ,'2D7521E242C8B26E4022683F2DE7F628'
				   ,'00514387/'+@id
				   ,'9208081' 
				   ,'Pacjent'
				   ,'testowy' +@id
				   ,'Testowa ' + @id
				   ,'50-131'
				   ,'53625'
				   ,1
				   ,1
				   ,4);
		SET  @counter = @counter +1;
		END 
	END

EXEC CreatePatient;
DROP PROCEDURE CreatePatient;




INSERT INTO Account([AccountLogin],[Email],[AccountPassword],[InsuranceNumber],[Pesel],[FirstName],[LastName], [Address] ,[PostalCode] ,[Phone] ,[CityId] ,[GenderId] ,[UserRoleId] )
			 VALUES
				   (
				   'jnowak'
				   ,'jnowak' + '@.pwr.edu.pl'
				   ,'2D7521E242C8B26E4022683F2DE7F628'
				   ,'00514387/'+'jnowak'
				   ,'9208081' 
				   ,'Jan'
				   ,'Nowak' + '- Stomatolog'
				   ,'Testowa'
				   ,'50-131'
				   ,'53625'
				   ,1
				   ,1
				   ,2);



				   SELECT * From Visit;

