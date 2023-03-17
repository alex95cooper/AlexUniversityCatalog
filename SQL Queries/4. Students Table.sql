USE University

CREATE TABLE Students
(
ID INT IDENTITY (1,1),
FirstName VARCHAR (15) NOT NULL,
LastName VARCHAR (15) NOT NULL,
Age INT,
Year INT,
FacID INT NOT NULL,
PRIMARY KEY (ID),
FOREIGN KEY (FacID) REFERENCES Faculties (ID)
)

INSERT INTO Students 
(FirstName, LastName, Age, Year, FacID)
VALUES 
('Sergey', 'Ivanov',17, 1, 1),
('Ivan','Sergeev', 18, 1, 2),
('Vladimir', 'Chernov',17, 1, 3),
('Ruslana', 'Gongharova',18, 1, 4),
('Nicolay', 'Vasnetsov',17, 1, 5),
('Alexandr', 'Romanenko',18, 2, 1),
('Julia', 'Stepanenko',19, 2, 2),
('Artem', 'Shevchenko',18, 2, 3),
('Michael', 'Kovalenko',19, 2, 4),
('Rostislav', 'Glazun',18, 2, 5),
('Valentin', 'Andreev',19, 3, 1),
('Anton', 'Valeriev',20, 3, 2),
('Olga', 'Chikalova',19, 3, 3),
('Andrey', 'Amelin',20, 3, 4),
('Yuri', 'Tarasenko',19, 3, 5),
('Veronika', 'Alekseeva',20, 4, 1),
('Viktor', 'Shevchenko',21, 4, 2),
('Andrey', 'Bochar',20, 4, 6),
('Denis', 'Borisov',21, 4, 7),
('Natalia', 'Zaitova',20, 4, 8),
('Michael', 'Melnik',21, 5, 6),
('Vitaliy', 'Voznyak',22, 5, 7),
('Stanislav', 'Novikov',23, 5, 8),
('Roman', 'Zodorozhniy',21, 5, 7),
('Daria', 'Chernyak',22, 5, 8)

Select*FROM Students