USE University

CREATE TABLE Teachers
(
ID INT IDENTITY (1,1),
SubjID INT NOT NULL,
FirstName VARCHAR (15) NOT NULL,
LastName VARCHAR (15) NOT NULL,
Age INT,
Experience INT,
PRIMARY KEY (ID),
FOREIGN KEY (SubjID) REFERENCES Subjects (ID) ON DELETE CASCADE
)

INSERT INTO Teachers
(SubjID, FirstName, LastName, Age, Experience)
VALUES
(1, 'Sergey', 'Petrov', 45, 20),
(2,'Oleg', 'Makarov', 70, 35),
(3,'Sergey', 'Nizhnik', 38,  13),
(4,'Dyadin', 'Nicolay', 80, 55),
(5,'Anatoly', 'Kisly', 56, 16),
(6,'Yuri', 'Skob', 42, 19),
(7,'Yuri ', 'Ivanov', 45, 20),
(8,'Andrey', 'Chumachenko', 57, 33),
(9,'Alexander', 'Kislitsyn', 73, 49),
(10,'Sergey', 'Tkachenko', 32, 7),
(11,'Irina', 'Uvarova', 36, 16),
(12,'Andrey', 'Momot', 45, 20),
(13,'Evgeny', 'Kachurov', 42, 18),
(14,'Inna', 'Ivakhnenko', 55, 35)

SELECT*FROM Teachers

SELECT Teachers.*, Subjects.*
FROM Teachers LEFT JOIN Subjects ON Teachers.SubjID = Subjects.ID 
