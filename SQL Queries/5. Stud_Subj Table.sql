USE University

CREATE TABLE Students_Subjects
(
StudID INT NOT NULL,
SubjID INT NOT NULL,
PRIMARY KEY (StudID, SubjID),
CONSTRAINT FK_StudID FOREIGN KEY (StudID) REFERENCES Students(ID),
CONSTRAINT FK_SubjID FOREIGN KEY (SubjID) REFERENCES Subjects(ID) 
)

ALTER TABLE Students_Subjects
  ADD CONSTRAINT uq_students_subjects UNIQUE(StudID, SubjID);

INSERT INTO Students_Subjects
(StudID, SubjID)
VALUES
(1,1), (1,2),(1,3),(1,4),(1,5),
(2,2),(2,3),(2,4),(2,5),(2,6),
(3,3),(3,4),(3,5),(3,6),(3,7),
(4,4),(4,5),(4,6),(4,7),(4,8),
(5,5),(5,6),(5,7),(5,8),(5,9),
(6,6),(6,7),(6,8),(6,9),(6,10),
(7,7),(7,8),(7,9),(7,10),(7,11),
(8,8),(8,9),(8,10),(8,11),(8,12),
(9,9),(9,10),(9,11),(9,12),(9,13),
(10,10),(10,11),(10,12),(10,13),(10,14),
(11,11),(11,12),(11,13),(11,14),(11,1),
(12,12),(12,13),(12,14),(12,1),(12,2),
(13,13),(13,14),(13,1),(13,2),(13,3),
(14,14),(14,1),(14,2),(14,3),(14,4),
(15,1),(15,2),(15,3),(15,4),(15,5),
(16,2),(16,3),(16,4),(16,5),(16,6),
(17,3),(17,4),(17,5),(17,6),(17,7),
(18,4),(18,5),(18,6),(18,7),(18,8),
(19,5),(19,6),(19,7),(19,8),(19,9),
(20,6),(20,7),(20,8),(20,9),(20,10),
(21,7),(21,8),(21,9),(21,10),(21,11),
(22,8),(22,9),(22,10),(22,11),(22,12),
(23,9),(23,10),(23,11),(23,12),(23,13),
(24,10),(24,11),(24,12),(24,13),(24,14),
(25,11),(25,12),(25,13),(25,14),(25,1)

SELECT*FROM Students_Subjects

SELECT COUNT(*) FROM Students_Subjects WHERE StudID = 4

SELECT STRING_AGG(Subjects.Name, ', ') AS Subjects FROM Students_Subjects
INNER JOIN Subjects ON Students_Subjects.SubjID = Subjects.ID
WHERE StudID = 4

DROP TABLE Students_Subjects

(SELECT ID FROM Subjects WHERE Name = 'Physics')

INSERT INTO Students_Subjects (StudID, SubjID) VALUES ((SELECT ID FROM Subjects WHERE Name = 'Physics'), 2)
